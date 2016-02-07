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
    HashSet<IVoxelLocation> Keys();

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
    IReadonlyVoxelMesh GetChild(IVoxelLocation location);

    /// <summary>
    ///   Get child dimensions.
    /// </summary>
    /// <returns></returns>
    int ChildWidth
    {
      get;
    }

    /// <summary>
    ///   Get child dimensions.
    /// </summary>
    /// <returns></returns>
    int ChildHeight
    {
      get;
    }

    /// <summary>
    ///   Get child dimensions.
    /// </summary>
    /// <returns></returns>
    int ChildDepth
    {
      get;
    }
  }
}
