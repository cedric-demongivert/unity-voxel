using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   An infinite voxel mesh based on a map of finished voxel mesh.
  /// </summary>
  public interface IMapVoxelMesh : IVoxelMesh
  {
    /// <summary>
    ///   Get existing childs of the map.
    /// </summary>
    /// <returns></returns>
    HashSet<VoxelLocation> Keys();

    /// <summary>
    ///   Get a specific child of the map.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    IReadonlyVoxelMesh GetChild(int x, int y, int z);

    /// <summary>
    ///   Get a specific child of the map.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    IReadonlyVoxelMesh GetChild(VoxelLocation location);

    /// <summary>
    ///   Transform an absolute location to a local chunck location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    VoxelLocation ToLocale(int x, int y, int z);

    /// <summary>
    ///   Get child dimensions.
    /// </summary>
    /// <returns></returns>
    Dimensions3D ChildDimensions
    {
      get;
    }
  }
}
