using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.physics
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
    private RaycastHit _hitInfo;

    public VoxelHit(VoxelLocation hittedVoxel, RaycastHit hitInfo)
    {
      this._hittedVoxel = hittedVoxel;
      this._hitInfo = hitInfo;
    }

    public RaycastHit HitInfo
    {
      get
      {
        return this._hitInfo;
      }
    }

    public Vector3 HitPoint
    {
      get
      {
        return this._hitInfo.point;
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
        return this.HittedVoxel.Add(this._hitInfo.normal);
      }
    }
  }
}
