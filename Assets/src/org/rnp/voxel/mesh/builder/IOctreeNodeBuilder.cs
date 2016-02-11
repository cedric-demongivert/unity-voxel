using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh.octree;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   An octree node builder.
  /// </summary>
  public interface IOctreeNodeBuilder : IVoxelMeshBuilder
  {
    /// <summary>
    ///   Node format to build.
    /// </summary>
    OctreeVoxelMeshFormat Format
    {
      get;
      set;
    }
  }
}
