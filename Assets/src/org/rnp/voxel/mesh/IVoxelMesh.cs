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
  public interface IVoxelMesh
  {
    /// <summary>
    ///   Return the last mesh update time in milliseconds.
    /// </summary>
    long LastUpdateTime
    {
      get;
    }

    /// <summary>
    ///   Dimensions of the voxel mesh.
    /// </summary>
    Dimensions3D Dimensions
    {
      get;
    }

    /// <summary>
    ///   Get the minimum point of that voxel mesh (inclusive).
    /// </summary>
    VoxelLocation Start { 
      get;
    }
    
    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    Color32 this[int x, int y, int z]
    {
      get;
      set;
    }

    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    Color32 this[VoxelLocation location]
    {
      get;
      set;
    }

    /// <summary>
    ///   Clear the voxel mesh.
    /// </summary>
    void Clear();

    /// <summary>
    ///   Get a ReadOnly implementation.
    /// </summary>
    /// <returns></returns>
    IReadonlyVoxelMesh ReadOnly();

    /// <summary>
    ///   Copy the voxel mesh.
    /// </summary>
    /// <returns></returns>
    IVoxelMesh Copy();

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
    bool Contains(VoxelLocation location);

    /// <summary>
    ///   Check if the mesh don't contains gaps.
    /// </summary>
    /// <returns></returns>
    bool IsFull();

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
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool IsEmpty(Vector3 location);

    /// <summary>
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    bool IsEmpty(VoxelLocation location);
    
    /// <summary>
    ///   Set a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="value"></param>
    void Set(int x, int y, int z, Color32 value);

    /// <summary>
    ///   Set a voxel of the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="value"></param>
    void Set(VoxelLocation location, Color32 value);

    /// <summary>
    ///   Get a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    Color32 Get(int x, int y, int z);

    /// <summary>
    ///   Get a voxel of the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    Color32 Get(VoxelLocation location);

    /// <summary>
    ///   Notify a mesh update.
    /// </summary>
    void Touch();
  }
}
