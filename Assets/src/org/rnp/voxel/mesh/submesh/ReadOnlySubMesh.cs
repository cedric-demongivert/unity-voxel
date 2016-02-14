using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.submesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A read only implementation.
  /// </summary>
  public class ReadOnlySubMesh : AbstractReadonlyVoxelMesh<ISubMesh>, ISubMesh
  {
    /// <see cref="org.rnp.voxel.mesh.submesh.ISubMesh"></see>
    public IVoxelMesh ParentMesh
    {
      get { return this._writableMesh.ParentMesh.ReadOnly(); }
    }

    /// <see cref="org.rnp.voxel.mesh.submesh.ISubMesh"></see>
    public IVoxelLocation Offset
    {
      get { return this._writableMesh.Offset; }
    }

    /// <summary>
    ///   Wrap in a readonly implementation, a writable implementation.
    /// </summary>
    /// <param name="writableArray"></param>
    public ReadOnlySubMesh(ISubMesh writableSubMesh) 
      : base(writableSubMesh)
    { }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadOnlySubMesh(ReadOnlySubMesh toCopy)
      : base((AbstractReadonlyVoxelMesh<ISubMesh>)toCopy)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ReadOnlySubMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return this;
    }
  }
}
