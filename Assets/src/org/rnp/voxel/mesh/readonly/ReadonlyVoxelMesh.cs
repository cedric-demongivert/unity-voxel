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
  public class ReadonlyVoxelMesh : VoxelMesh
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

    /// <summary>
    ///   Wrap a writable voxel mesh in a readonly implementation.
    /// </summary>
    /// <param name="writableMesh"></param>
    public ReadonlyVoxelMesh(VoxelMesh writableMesh)
    {
      this._writableMesh = writableMesh;
    }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadonlyVoxelMesh(ReadonlyVoxelMesh toCopy)
    {
      this._writableMesh = toCopy._writableMesh;
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
  }
}
