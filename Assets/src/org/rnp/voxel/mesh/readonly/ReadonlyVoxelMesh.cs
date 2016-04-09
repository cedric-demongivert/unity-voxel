using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh.exceptions;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A read-only voxel mesh.
  /// </summary>
  public class ReadonlyVoxelMesh : VoxelMesh, IVoxelMeshCommitListener
  {
    /// <summary>
    ///   Wrapped voxel mesh.
    /// </summary>
    [SerializeField]
    private VoxelMesh _writableMesh;

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override Dimensions3D Dimensions
    {
      get { return this._writableMesh.Dimensions; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelLocation Start
    {
      get { return this._writableMesh.Start; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsReadonly
    {
      get { return true; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsDirty
    {
      get
      {
        return this._writableMesh.IsDirty;
      }
    }

    /// <summary>
    ///   Wrap a writable voxel mesh in a readonly implementation.
    /// </summary>
    /// <param name="writableMesh"></param>
    public ReadonlyVoxelMesh(VoxelMesh writableMesh) : base()
    {
      this._writableMesh = writableMesh;
      this._writableMesh.RegisterCommitListener(this);
    }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadonlyVoxelMesh(ReadonlyVoxelMesh toCopy) : base()
    {
      this._writableMesh = toCopy._writableMesh;
      this._writableMesh.RegisterCommitListener(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      return this._writableMesh.Get(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      return this._writableMesh.Get(location);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void MarkDirty()
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Clear()
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Copy()
    {
      return new ReadonlyVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Readonly()
    {
      return this;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Commit()
    {
      this._writableMesh.Commit();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnRegister(VoxelMesh mesh)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitBegin(VoxelMesh mesh)
    {
      foreach(IVoxelMeshCommitListener listener in this.Listeners)
      {
        listener.OnCommitBegin(mesh);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitEnd(VoxelMesh mesh)
    {
      foreach (IVoxelMeshCommitListener listener in this.Listeners)
      {
        listener.OnCommitEnd(mesh);
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/ScriptableObject.html"/>
    public override void Destroy()
    {
      this._writableMesh.UnregisterCommitListener(this);
      base.Destroy();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnUnregister(VoxelMesh mesh)
    {
      this.Destroy();
    }
  }
}
