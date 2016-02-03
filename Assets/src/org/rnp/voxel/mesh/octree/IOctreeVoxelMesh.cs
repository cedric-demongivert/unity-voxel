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
  ///   A voxel mesh that store data in a octree. 
  /// </summary>
  /// <see cref="https://en.wikipedia.org/wiki/Octree"/>
  public interface IOctreeVoxelMesh : IVoxelMesh
  {
    /// <summary>
    ///   Get an Octree child by its location. 
    ///   
    ///   It exists only eight availables location :
    ///   0 0 0     0 0 1
    ///   1 0 0     1 0 1
    ///   0 1 0     0 1 1
    ///   1 1 0     1 1 1
    ///   
    ///   This method must return a readonly implementation, if you
    /// need to modify the octree mesh, you must use the parent node and
    /// not directly child nodes.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>A read only voxel mesh.</returns>
    IReadonlyVoxelMesh GetChild(int x, int y, int z);
  }
}
