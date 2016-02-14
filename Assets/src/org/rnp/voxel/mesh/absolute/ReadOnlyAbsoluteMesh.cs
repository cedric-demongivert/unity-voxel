using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.absolute
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A read only implementation.
  /// </summary>
  public class ReadOnlyAbsoluteMesh : AbstractReadonlyVoxelMesh<IAbsoluteVoxelMesh>, IAbsoluteVoxelMesh
  {
    /// <see cref="org.rnp.voxel.mesh.absolute.IAbsoluteVoxelMesh"></see>
    public IVoxelMesh RelativeMesh
    {
      get { return this._writableMesh.RelativeMesh.ReadOnly(); }
    }

    /// <summary>
    ///   Wrap in a readonly implementation, a writable implementation.
    /// </summary>
    /// <param name="writableArray"></param>
    public ReadOnlyAbsoluteMesh(IAbsoluteVoxelMesh writableMesh) 
      : base(writableMesh)
    { }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadOnlyAbsoluteMesh(ReadOnlyAbsoluteMesh toCopy)
      : base((AbstractReadonlyVoxelMesh<IAbsoluteVoxelMesh>)toCopy)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ReadOnlyAbsoluteMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return this;
    }
  }
}
