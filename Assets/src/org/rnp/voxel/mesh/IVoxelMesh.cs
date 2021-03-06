﻿using UnityEngine;
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
    ///   Get voxel mesh width.
    /// </summary>
    int Width
    {
      get;
    }

    /// <summary>
    ///   Get voxel mesh height.
    /// </summary>
    int Height
    {
      get;
    }

    /// <summary>
    ///   Get voxel mesh depth.
    /// </summary>
    int Depth
    {
      get;
    }

    /// <summary>
    ///   Get the minimum point of that voxel mesh (inclusive).
    /// </summary>
    VoxelLocation Start { get; }

    /// <summary>
    ///   Get the end point of that voxel mesh (exclusive).
    /// </summary>
    VoxelLocation End { get; }

    /// <summary>
    ///   Get the center of that voxel mesh.
    /// </summary>
    VoxelLocation Center { get; }

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
    ///   Copy a part of another voxel mesh.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="where"></param>
    /// <param name="toCopy"></param>
    void Copy(VoxelLocation from, VoxelLocation to, VoxelLocation where, IVoxelMesh toCopy);

    /// <summary>
    ///   Copy a part of another voxel mesh.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="where"></param>
    /// <param name="toCopy"></param>
    void Copy(VoxelLocation start, IDimensions3D size, VoxelLocation where, IVoxelMesh toCopy);

    /// <summary>
    ///   Fille a part of the voxel mesh with a color.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    void Fill(VoxelLocation start, IDimensions3D size, Color color);
  }
}
