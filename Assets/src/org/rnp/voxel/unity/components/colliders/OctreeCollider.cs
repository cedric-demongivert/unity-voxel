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

    private IList<BoxCollider> colliders;

    public void Awake()
    {
      this._walker = new OctreeOutlineWalker();
      this.colliders = new List<BoxCollider>();
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
      foreach(BoxCollider collider in this.colliders)
      {
        Destroy(collider);
      }

      this.colliders.Clear();
    }

    public override void RefreshCollider()
    {
      this.Clean();

      if (this._mesh == null) return;

      this._walker.SetRoot(this._mesh.Mesh);
      IVoxelMesh nextBlock = null;
      
      while((nextBlock = this._walker.Next()) != null)
      {
        this.CreateBoxColliderFor(nextBlock);
      }
    }

    protected void CreateBoxColliderFor(IVoxelMesh mesh)
    {
      BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
      Vector3 start = VoxelLocation.ToVector3(mesh.Start);
      Vector3 end = VoxelLocation.ToVector3(mesh.End);

      collider.center = (start + end) / 2;
      collider.size = (end - start);
    }
  }
}
