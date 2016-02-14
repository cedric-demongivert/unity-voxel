using System;
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

    private IVoxelLocation _hittedVoxel;

    private VoxelMeshCollider _hittedCollider;

    public RaycastVoxelHit(RaycastHit result)
    {
      this._hitResult = result;
      this.Initialize(result);
    }

    private void Initialize(RaycastHit result)
    {
      GameObject hittedObject = result.collider.gameObject;
      VoxelMesh mesh = hittedObject.GetComponent<VoxelMesh>(); // maybe use an interface and search the collider ?
      GameObject meshObject = mesh.gameObject; // For a later use. See top.

      this._hittedMesh = null;
      this._hittedVoxel = null;

      if(mesh != null)
      {
        this._hittedMesh = mesh;
        Vector3 localized = meshObject.transform.worldToLocalMatrix.MultiplyPoint3x4(result.point);
        this._hittedVoxel = new VoxelLocation(localized);
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

    public IVoxelLocation HittedVoxel
    {
      get
      {
        return this._hittedVoxel;
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
