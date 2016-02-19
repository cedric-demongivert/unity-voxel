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

    [MenuItem("CONTEXT/PrototypeTranslator/Load VoxelMesh...")]
    public static void LoadMeshInPlace(MenuCommand menuCommand)
    {
        PrototypeTranslator mt = menuCommand.context as PrototypeTranslator;

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

                mesh[x, y, z] = new Color32(r,g,b,a);
                cpt++;
            }
            /*
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        mesh[x, y, z] = new Color32((byte)Random.Range(0,255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 125);
*/
            br.Close();
        }

        return mesh;
    }

}
