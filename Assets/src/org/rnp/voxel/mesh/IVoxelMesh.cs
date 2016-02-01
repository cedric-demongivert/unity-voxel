using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail]</author>
  /// 
  /// <summary>
  ///   A voxel mesh full of colors.
  /// </summary>
  public interface IVoxelMesh : IDimensions3D
  {
    /// <summary>
    ///   Starting X,Y,Z position of the mesh.
    /// </summary>
    IVoxelLocation Start
    {
      get;
      set;
    }

    Color32 this[int x, int y, int z]
    {
      get;
    }

    Color32 this[IVoxelLocation location]
    {
      get;
    }

    Color32 this[Vector3 location]
    {
      get;
    }

    bool Contains(int x, int y, int z);
    bool Contains(Vector3 location);
    bool Contains(IVoxelLocation location);

    bool AbsoluteContains(int x, int y, int z);
    bool AbsoluteContains(Vector3 location);
    bool AbsoluteContains(IVoxelLocation location);

    Color32 AbsoluteGet(int x, int y, int z);
    Color32 AbsoluteGet(Vector3 location);
    Color32 AbsoluteGet(IVoxelLocation location);
  }
}
