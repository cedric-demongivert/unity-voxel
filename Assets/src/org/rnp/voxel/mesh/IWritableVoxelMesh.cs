using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh
{
  /// <auhor>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A voxel mesh that can be modified.
  /// </summary>
  public interface IWritableVoxelMesh : IVoxelMesh
  {
    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    new Color32 this[int x, int y, int z]
    {
      get;
      set;
    }

    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    new Color32 this[IVoxelLocation location]
    {
      get;
      set;
    }

    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    new Color32 this[Vector3 location]
    {
      get;
      set;
    }

    /// <summary>
    ///   Clear the voxel mesh.
    /// </summary>
    void Clear();
  }
}
