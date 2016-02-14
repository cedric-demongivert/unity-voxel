using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.walker;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.unity.components.colliders
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A box collider for voxel mesh.
  /// </summary>
  public class OctreeCollider : VoxelMeshCollider
  {
    private OctreeOutlineWalker _walker;

    private IList<BoxCollider> _colliders;

    private IList<BoxCollider> _oldColliders;

    public void Awake()
    {
      this._walker = new OctreeOutlineWalker();
      this._colliders = new List<BoxCollider>();
      this._oldColliders = new List<BoxCollider>();
    }

    public void Start()
    {
      this.RefreshCollider();
    }

    public void Update()
    {

    }

    protected void Clean()
    {
      foreach (BoxCollider collider in this._oldColliders)
      {
        Destroy(collider);
      }

      this._oldColliders.Clear();
    }

    protected void Switch()
    {
      IList<BoxCollider> tmp = this._colliders;
      this._colliders = this._oldColliders;
      this._oldColliders = tmp;
    }

    public override void RefreshCollider()
    {
      this.Switch();

      if (this._mesh == null)
      {
        this.Clean();
        return;
      }

      this._walker.SetRoot(this._mesh.Mesh);
      IVoxelMesh nextBlock = null;
      
      while((nextBlock = this._walker.Next()) != null)
      {
        this.CreateBoxColliderFor(nextBlock);
      }

      this.Clean();
    }

    protected BoxCollider GetCollider()
    {
      if (this._oldColliders.Count > 0)
      {
        BoxCollider box = this._oldColliders[0];
        this._oldColliders.RemoveAt(0);
        return box;
      }
      else
      {
        return this.gameObject.AddComponent<BoxCollider>();
      }
    }

    protected void CreateBoxColliderFor(IVoxelMesh mesh)
    {
      BoxCollider collider = this.GetCollider();

      Vector3 start = VoxelLocation.ToVector3(mesh.Start);
      Vector3 end = VoxelLocation.ToVector3(mesh.End);

      collider.center = (start + end) / 2;
      collider.size = (end - start);

      this._colliders.Add(collider);
    }
  }
}
