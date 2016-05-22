using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A game object that hold a voxel mesh.
  /// </summary>
  [ExecuteInEditMode]
  public abstract class VoxelMeshContainer : MonoBehaviour
  {
    /// <summary>
    ///   Return the voxel mesh hold by the container.
    /// </summary>
    public abstract VoxelMesh Mesh
    {
      get;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void OnDestroy()
    {
      if(this.Mesh != null)
      {
        this.Mesh.Destroy();
      }
    }
  }
}
