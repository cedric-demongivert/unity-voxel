using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An octree walker state.
  /// </summary>
  public interface IWalkerState : ICopiable<IWalkerState>
  {
    IVoxelLocation Location
    {
      get;
      set;
    }

    /// <summary>
    ///   Return walked node.
    /// </summary>
    IVoxelMesh Node {
      get;
    }

    /// <summary>
    ///   Get next sub element of the actual node.
    ///   May return null if the actual node has no more sub element.
    /// </summary>
    /// <returns></returns>
    IVoxelMesh Next();

    /// <summary>
    ///   Return the location of the last returned mesh.
    /// </summary>
    /// <returns></returns>
    IVoxelLocation GetLocation();
  }
}
