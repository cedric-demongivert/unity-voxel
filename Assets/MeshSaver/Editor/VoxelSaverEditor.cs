using UnityEditor;

using org.rnp.voxel.unity.components.meshes;
using Assets.src.org.rnp.voxel.utils;

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
        VoxelFile.Save(vm.Mesh, path);


    }

}

