using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.unity.components.translators;

public static class VoxelLoader
{

    [MenuItem("CONTEXT/MapTranslator/Load VoxelMesh...")]
    public static void LoadMeshInPlace(MenuCommand menuCommand)
    {
        MapTranslator mt = menuCommand.context as MapTranslator;




        GameObject go = mt.gameObject;
        VoxelMesh vm2 = go.AddComponent<VoxelMesh>();
        vm2.Mesh= LoadVoxelStruct();
        mt.VoxelMesh = vm2;
    }


    public static IVoxelMesh LoadVoxelStruct()
    {
        string path = EditorUtility.OpenFilePanel("Load a Voxel file.", "Assets/MeshStruct/", "vxl"); //Homemade extension
        if (string.IsNullOrEmpty(path)) return null;

        path = FileUtil.GetProjectRelativePath(path);

        IVoxelMesh mesh = new MapVoxelMesh();
        mesh.Clear();
        // Create a file to write to.
        using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open)))
        {
            // Use BaseStream.
            int length = (int)br.BaseStream.Length;

            int width = br.ReadInt32();
            int height = br.ReadInt32();
            int depth = br.ReadInt32();
            int pos = sizeof(int)*3;

            /*
            int x = br.ReadInt32();
            int y = br.ReadInt32();
            int z = br.ReadInt32();
            */

            while (pos < length)
            {
                int w = br.ReadInt32();
                int h = br.ReadInt32();
                int d = br.ReadInt32();
                pos += sizeof(int) * 3;

                byte r = br.ReadByte();
                byte g = br.ReadByte();
                byte b = br.ReadByte();
                byte a = br.ReadByte();
                pos += sizeof(byte) * 4;


                mesh[w, h, d] = new Color32(r, g, b, a);

                
            }

            br.Close();
        }

        return mesh;
    }

}
