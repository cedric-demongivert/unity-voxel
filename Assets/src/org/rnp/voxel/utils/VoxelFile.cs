using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

using System.IO;
using UnityEditor;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  static public class VoxelFile
  {
    static public VoxelMesh Load(string path)
    {
      if (string.IsNullOrEmpty(path)) return null;

      path = FileUtil.GetProjectRelativePath(path);

      VoxelMesh mesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
      mesh.Clear();
      // Create a file to write to.
      using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
      {
        // Use BaseStream.
        int length = (int)br.BaseStream.Length;

        int width = br.ReadInt32();
        int height = br.ReadInt32();
        int depth = br.ReadInt32();
        int pos = sizeof(int) * 3;

        int cpt = 0;
        while (pos < length)
        {
          int x = br.ReadInt32();
          int y = br.ReadInt32();
          int z = br.ReadInt32();
          pos += sizeof(int) * 3;

          byte r = br.ReadByte();
          byte g = br.ReadByte();
          byte b = br.ReadByte();
          byte a = br.ReadByte();
          pos += sizeof(byte) * 4;

          mesh[x, y, z] = new Color32(r, g, b, a);
          cpt++;
        }

        br.Close();
      }

      return mesh;
    }

    static public void Save(VoxelMesh vm, string path)
    {
      if (string.IsNullOrEmpty(path)) return;

      path = FileUtil.GetProjectRelativePath(path);

      // Create a file to write to.
      using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create)))
      {
        bw.Write(vm.Dimensions.Width);
        bw.Write(vm.Dimensions.Height);
        bw.Write(vm.Dimensions.Depth);

        VoxelLocation Start = vm.Start;
        VoxelLocation End = vm.Start.Add(vm.Dimensions);

        for (int w = Start.X; w < End.X; w++)
        {
          for (int h = Start.Y; h < End.Y; h++)
          {
            for (int d = Start.Z; d < End.Z; d++)
            {
              bw.Write(w);
              bw.Write(h);
              bw.Write(d);
              bw.Write(vm[w, h, d].r);
              bw.Write(vm[w, h, d].g);
              bw.Write(vm[w, h, d].b);
              bw.Write(vm[w, h, d].a);
          }
          }
        }

        bw.Close();
      }
    } 
  }
}
