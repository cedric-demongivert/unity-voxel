﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.components.colliders;

namespace org.rnp.voxel.unity
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Result of a Raycast over a voxel object.
  /// </summary>
  public sealed class RaycastVoxelHit
  {
    private RaycastHit _hitResult;

    private VoxelMesh _hittedMesh;

    private VoxelLocation _hittedInnerVoxel;

    private VoxelLocation _hittedOutVoxel;

    private VoxelMeshCollider _hittedCollider;

    private bool _isVoxelMesh;

    public bool IsVoxelMesh
    {
      get
      {
        return this._isVoxelMesh;
      }
    }

    public RaycastVoxelHit(RaycastHit result)
    {
      this._hitResult = result;
      this.Initialize(result);
    }

    private void Initialize(RaycastHit result)
    {
      GameObject hittedObject = result.collider.gameObject;
      VoxelMesh mesh = hittedObject.GetComponent<VoxelMesh>(); // maybe use an interface and search the collider ?

      this._isVoxelMesh = mesh != null;
      this._hittedMesh = null;
      this._hittedInnerVoxel = null;
      this._hittedOutVoxel = null;

      if(mesh != null)
      {
        this._hittedMesh = mesh;
        Vector3 localized = hittedObject.transform.worldToLocalMatrix.MultiplyPoint3x4(result.point);
        this._hittedInnerVoxel = new VoxelLocation(localized - (result.normal / 2));
        this._hittedOutVoxel = new VoxelLocation(localized + (result.normal / 2));
      }

      this._hittedCollider = hittedObject.GetComponent<VoxelMeshCollider>();
    }

    public RaycastHit HitResult
    {
      get
      {
        return this._hitResult;
      }
    }

    public VoxelMesh HittedMesh
    {
      get
      {
        return this._hittedMesh;
      }
    }

    public VoxelLocation HittedInnerVoxel
    {
      get
      {
        return this._hittedInnerVoxel;
      }
    }

    public VoxelLocation HittedOutVoxel
    {
      get
      {
        return this._hittedOutVoxel;
      }
    }

    public VoxelMeshCollider HittedCollider
    {
      get
      {
        return this._hittedCollider;
      }
    }
  }
}
