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
  ///   A read only voxel mesh base implementation.
  /// </summary>
  public abstract class AbstractReadonlyVoxelMesh <T> : AbstractVoxelMesh, IReadonlyVoxelMesh where T : IVoxelMesh
  {
    /// <summary>
    ///   Wrapped voxel mesh.
    /// </summary>
    protected T _writableMesh;

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Width
    {
      get { return this._writableMesh.Width; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Height
    {
      get { return this._writableMesh.Height; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Depth
    {
      get { return this._writableMesh.Depth; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Color32 this[int x, int y, int z]
    {
      get { return this._writableMesh[x, y, z]; }
      set { throw new UnmodifiableVoxelMeshException(this); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation Start
    {
      get { return this._writableMesh.Start; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation End
    {
      get { return this._writableMesh.End; }
    }

    /// <summary>
    ///   Wrap a writable voxel mesh in a readonly implementation.
    /// </summary>
    /// <param name="writableMesh"></param>
    public AbstractReadonlyVoxelMesh(T writableMesh)
    {
      this._writableMesh = writableMesh;
    }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public AbstractReadonlyVoxelMesh(AbstractReadonlyVoxelMesh<T> toCopy)
    {
      this._writableMesh = toCopy._writableMesh;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Clear()
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      return this._writableMesh.IsFull();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsEmpty()
    {
      return this._writableMesh.IsEmpty();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Copy(VoxelLocation from, VoxelLocation to, VoxelLocation where, IVoxelMesh toCopy)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Copy(VoxelLocation start, IDimensions3D size, VoxelLocation where, IVoxelMesh toCopy)
    {
      throw new UnmodifiableVoxelMeshException(this);
    }
  }
}
