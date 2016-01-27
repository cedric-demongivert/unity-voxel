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
    Color32 this[uint x, uint y, uint z]
    {
      get;
    }

    Color32 this[int x, int y, int z]
    {
      get;
    }

    Color32 this[Vector3 location]
    {
      get;
    }
  }
}
