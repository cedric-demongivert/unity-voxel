using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A Readonly octree.
  /// </summary>
  public class ReadonlyOctreeVoxelMesh : ReadonlyVoxelMesh, IOctreeVoxelMesh
  {
    private IOctreeVoxelMesh _writableMesh;

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public Dimensions3D ChildDimensions
    {
      get
      {
        return this._writableMesh.ChildDimensions;
      }
    }

    public ReadonlyOctreeVoxelMesh(IOctreeVoxelMesh mesh)
      : base(mesh)
    {
      this._writableMesh = mesh;
    }

    public ReadonlyOctreeVoxelMesh(ReadonlyOctreeVoxelMesh toCopy)
      : base(toCopy)
    {
      this._writableMesh = toCopy._writableMesh;
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      return this._writableMesh.GetChild(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public VoxelLocation ToChildCoordinates(VoxelLocation location)
    {
      return this._writableMesh.ToChildCoordinates(location);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public VoxelLocation ToLocaleCoordinates(VoxelLocation location)
    {
      return this._writableMesh.ToLocaleCoordinates(location);
    }
  }
}
