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
    new Color32 this[int x, int y, int z]
    {
      get;
      set;
    }

    new Color32 this[IVoxelLocation location]
    {
      get;
      set;
    }

    new Color32 this[Vector3 location]
    {
      get;
      set;
    }

    /// <summary>
    ///   Clear the voxel mesh.
    /// </summary>
    void Clear();

    void AbsoluteSet(int x, int y, int z, Color32 color);
    void AbsoluteSet(Vector3 location, Color32 color);
    void AbsoluteSet(IVoxelLocation location, Color32 color);
  }
}
