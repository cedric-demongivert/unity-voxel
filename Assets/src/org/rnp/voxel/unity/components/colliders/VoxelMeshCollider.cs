using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.unity.components.meshes;

namespace org.rnp.voxel.unity.components.colliders
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Standard component for voxel colliders.
  /// </summary>
  public abstract class VoxelMeshCollider : MonoBehaviour
  {
    [SerializeField]
    protected VoxelMesh _mesh;

    /// <summary>
    ///   Mesh used to create the collider.
    /// </summary>
    public virtual VoxelMesh MeshToFit
    {
      get 
      {
        return _mesh;
      }
      set
      {
        this._mesh = value;
      }
    }

    /// <summary>
    ///   Recalculate the collider bounds.
    /// </summary>
    public abstract void RefreshCollider();
  }
}
