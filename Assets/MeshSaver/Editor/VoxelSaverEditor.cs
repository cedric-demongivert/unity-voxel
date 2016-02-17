using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using org.rnp.voxel.unity.components.meshes;

public static class VoxelSaverEditor
{

    [MenuItem("CONTEXT/VoxelMesh/Save VoxelMesh...")]
    public static void SaveMeshInPlace(MenuCommand menuCommand)
    {
        VoxelMesh vm = menuCommand.context as VoxelMesh;

        SaveVoxelStruct(vm);

    }


    public static void SaveVoxelStruct(VoxelMesh vm)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/MeshStruct/", "", "vxl"); //Homemade extension
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);


        // Create a file to write to.
        using (FileStream fs = File.Create(path))
        {
            fs.WriteByte((byte)vm.Mesh.Width);
            fs.WriteByte((byte)vm.Mesh.Height);
            fs.WriteByte((byte)vm.Mesh.Depth);
            //fs.WriteByte((byte)vm.Mesh.Start);
            //fs.WriteByte((byte)vm.Mesh.End);
                
            for(int w=0; w< vm.Mesh.Width; w++)
            {
                for(int h=0; h<vm.Mesh.Height; h++)
                {
                    for(int d=0; d<vm.Mesh.Depth; d++)
                    {
                        fs.WriteByte((byte)w);
                        fs.WriteByte((byte)h);
                        fs.WriteByte((byte)d);
                        fs.WriteByte((byte)vm.Mesh[w, h, d].r);
                        fs.WriteByte((byte)vm.Mesh[w, h, d].g);
                        fs.WriteByte((byte)vm.Mesh[w, h, d].b);
                        fs.WriteByte((byte)vm.Mesh[w, h, d].a);
                            
                    }
                }
            }

            fs.Close();
        }            


    }

}
