using UnityEditor;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.unity.components.translators;
using Assets.src.org.rnp.voxel.utils;

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
        return VoxelFile.Load(path);
    }

}
