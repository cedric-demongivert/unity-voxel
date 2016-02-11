using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh.array
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A voxel mesh that store data in a buffer, this implementation is not writable.
  /// </summary>
  public sealed class ReadonlyArrayVoxelMesh : AbstractReadonlyVoxelMesh<IArrayVoxelMesh>, IArrayVoxelMesh
  {
    /// <summary>
    ///   Wrap in a readonly implementation, a writable voxel array implementation.
    /// </summary>
    /// <param name="writableArray"></param>
    public ReadonlyArrayVoxelMesh(IArrayVoxelMesh writableArray) 
      : base(writableArray)
    { }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadonlyArrayVoxelMesh(ReadonlyArrayVoxelMesh toCopy)
      : base((AbstractReadonlyVoxelMesh<IArrayVoxelMesh>) toCopy)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ReadonlyArrayVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return this;
    }
  }
}
