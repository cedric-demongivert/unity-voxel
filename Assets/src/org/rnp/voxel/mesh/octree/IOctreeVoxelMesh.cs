using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   An Octree based voxel mesh.
  /// </summary>
  public interface IOctreeVoxelMesh : IVoxelMesh
  {
    /// <summary>
    ///   Dimensions of octree childs.
    /// </summary>
    Dimensions3D ChildDimensions
    {
      get;
    }

    /// <summary>
    ///   Transform an absolute location into a child coordinates.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    VoxelLocation ToChildCoordinates(VoxelLocation location);

    /// <summary>
    ///   Transform an absolute location into a location relative to a child.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    VoxelLocation ToLocaleCoordinates(VoxelLocation location);
    
    /// <summary>
    ///   Get a child of this octree.
    ///   
    ///   An octree has only 8 childs :
    ///   000 100
    ///   001 101
    ///   010 110
    ///   011 111
    /// </summary>
    /// 
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// 
    /// <returns></returns>
    IReadonlyVoxelMesh GetChild(int x, int y, int z);
  }
}
