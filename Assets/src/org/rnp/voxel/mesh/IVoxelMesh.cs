using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail]</author>
  /// 
  /// <summary>
  ///   A voxel mesh full of colors.
  /// </summary>
  public interface IVoxelMesh : IDimensions3D
  {
    /// <summary>
    ///   Get the minimum point of that voxel mesh (inclusive).
    /// </summary>
    IVoxelLocation Start { get; }

    /// <summary>
    ///   Get the end point of that voxel mesh (exclusive).
    /// </summary>
    IVoxelLocation End { get; }

    /// <summary>
    ///   Get a voxel at a specific location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>A voxel.</returns>
    Color32 this[int x, int y, int z]
    {
      get;
    }

    /// <summary>
    ///   Get a voxel at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    Color32 this[IVoxelLocation location]
    {
      get;
    }

    /// <summary>
    ///   Get a voxel at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    Color32 this[Vector3 location]
    {
      get;
    }

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    bool Contains(int x, int y, int z);

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool Contains(Vector3 location);

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool Contains(IVoxelLocation location);

    /// <summary>
    ///   Check if the mesh contains data.
    /// </summary>
    /// <returns></returns>
    bool IsEmpty();

    /// <summary>
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    bool IsEmpty(int x, int y, int z);

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool IsEmpty(Vector3 location);

    /// <summary>
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool IsEmpty(IVoxelLocation location);
  }
}
