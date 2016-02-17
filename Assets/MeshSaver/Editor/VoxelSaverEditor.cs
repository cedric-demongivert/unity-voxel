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
        using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            bw.Write(vm.Mesh.Width);
            bw.Write(vm.Mesh.Height);
            bw.Write(vm.Mesh.Depth);
            //bw.Write(vm.Mesh.Start);
            //bw.Write(vm.Mesh.End);
                
            for(int w=0; w< vm.Mesh.Width; w++)
            {
                for(int h=0; h<vm.Mesh.Height; h++)
                {
                    for(int d=0; d<vm.Mesh.Depth; d++)
                    {
                        bw.Write(w);
                        bw.Write(h);
                        bw.Write(d);
                        bw.Write(vm.Mesh[w, h, d].r);
                        bw.Write(vm.Mesh[w, h, d].g);
                        bw.Write(vm.Mesh[w, h, d].b);
                        bw.Write(vm.Mesh[w, h, d].a);
                            
                    }
                }
            }

            bw.Close();
        }            


    }

}
