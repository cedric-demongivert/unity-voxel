using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A readOnly octree voxel mesh implementation.
  /// </summary>
  public sealed class ReadonlyOctreeVoxelMesh : AbstractReadonlyVoxelMesh<IOctreeVoxelMesh>, IOctreeVoxelMesh
  {
    public ReadonlyOctreeVoxelMesh(IOctreeVoxelMesh writableMesh)
      : base(writableMesh)
    { }

    public ReadonlyOctreeVoxelMesh(ReadonlyOctreeVoxelMesh toCopy)
      : base((AbstractReadonlyVoxelMesh<IOctreeVoxelMesh>) toCopy)
    { }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      return this._writableMesh.GetChild(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ReadonlyOctreeVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return this;
    }
  }
}
