using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <auhor>Cédric DEMONGIVERT <cedric.demongivert@gmail.com></author>
  /// 
  /// <summary>
  ///   A voxel mesh that can be modified.
  /// </summary>
  public interface IWritableVoxelMesh : IVoxelMesh
  {
    new Color32 this[uint x, uint y, uint z]
    {
      get;
      set;
    }

    new Color32 this[Vector3 location]
    {
      get;
      set;
    }
  }
}
