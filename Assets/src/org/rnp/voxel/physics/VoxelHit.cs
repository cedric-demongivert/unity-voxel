using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace src.org.rnp.voxel.physics
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Voxel Hit Info
  /// </summary>
  [Serializable]
  public class VoxelHit
  {
    [SerializeField]
    private VoxelLocation _hittedVoxel;

    [SerializeField]
    private VoxelLocation _hittedFaceVoxel;

    [SerializeField]
    private Vector3 _hitPoint;

    public VoxelHit(VoxelLocation hittedVoxel, Vector3 hitPoint)
    {
      this._hittedVoxel = hittedVoxel;
      this._hitPoint = hitPoint;
    }

    public Vector3 HitPoint
    {
      get
      {
        return this._hitPoint;
      }
    }

    public VoxelLocation HittedVoxel
    {
      get
      {
        return this._hittedVoxel;
      }
    }

    public VoxelLocation HittedFaceVoxel
    {
      get
      {
        return this._hittedFaceVoxel;
      }
    }
  }
}
