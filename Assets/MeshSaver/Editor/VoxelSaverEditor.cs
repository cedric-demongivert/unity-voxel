using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using org.rnp.voxel.unity.components.meshes;
using System;
using org.rnp.voxel.utils;

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
        string path = EditorUtility.SaveFilePanel("Save Voxel Mesh Asset", "Assets/MeshStruct/", "", "vxl"); //Homemade extension
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        // Create a file to write to.
        using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            bw.Write(vm.Mesh.Width);
            bw.Write(vm.Mesh.Height);
            bw.Write(vm.Mesh.Depth);


            /*
            bw.Write(vm.Mesh.Start.X);
            bw.Write(vm.Mesh.Start.Y);
            bw.Write(vm.Mesh.Start.Z);
            bw.Write(vm.Mesh.End.X);
            bw.Write(vm.Mesh.End.Y);
            bw.Write(vm.Mesh.End.Z);
            */

            IVoxelLocation Start = vm.Mesh.Start;
            IVoxelLocation End = vm.Mesh.End;

            for (int w = Start.X; w < End.X; w++)
            {
                for (int h = Start.Y; h < End.Y; h++)
                {
                    for (int d = Start.Z; d < End.Z; d++)
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

