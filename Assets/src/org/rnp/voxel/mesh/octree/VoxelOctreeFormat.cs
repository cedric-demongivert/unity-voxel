using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A voxel octree node size, voxel octree nodes must have a size that is
  /// a power of 2.
  /// </summary>
  public sealed class VoxelOctreeFormat
  {
    /// <summary>
    ///   A 0x0x0 voxel octree.
    /// </summary>
    public static readonly  VoxelOctreeFormat Empty = new VoxelOctreeFormat();

    /// <summary>
    ///   A 64x64x64 voxel octree.
    /// </summary>
    public static readonly VoxelOctreeFormat Tiny = new VoxelOctreeFormat(6);

    /// <summary>
    ///   A 128x128x128 voxel octree.
    /// </summary>
    public static readonly VoxelOctreeFormat Small = new VoxelOctreeFormat(7);

    /// <summary>
    ///   A 256x256x256 voxel octree.
    /// </summary>
    public static readonly VoxelOctreeFormat Medium = new VoxelOctreeFormat(8);

    /// <summary>
    ///   A 512x512x512 voxel octree.
    /// </summary>
    public static readonly VoxelOctreeFormat Big = new VoxelOctreeFormat(9);

    /// <summary>
    ///   A 1024x1024x1024 voxel octree.
    /// </summary>
    public static readonly VoxelOctreeFormat Gigantic = new VoxelOctreeFormat(10);

    /// <summary>
    ///   Get a octree format for a specific size.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public static VoxelOctreeFormat GetFormat(int width, int height, int depth)
    {
      int order = 0;
      int value = 1;
      
      if (width == 0 && height == 0 && depth == 0) return VoxelOctreeFormat.Empty;

      while (width - value > 1 || height - value > 1 || depth - value > 1)
      {
        order += 1;
        value = value << 1;
      }

      switch (order)
      {
        case 6:
          return VoxelOctreeFormat.Tiny;
        case 7:
          return VoxelOctreeFormat.Small;
        case 8:
          return VoxelOctreeFormat.Medium;
        case 9:
          return VoxelOctreeFormat.Big;
        case 10:
          return VoxelOctreeFormat.Gigantic;
        default:
          return new VoxelOctreeFormat(order);
      }
    }

    public readonly int Width;
    public readonly int Height;
    public readonly int Depth;
    public readonly int ChildWidth;
    public readonly int ChildHeight;
    public readonly int ChildDepth;

    /// <summary>
    ///   Create an empty octree format.
    /// </summary>
    public VoxelOctreeFormat()
    {
      this.Width = this.Height = this.Depth = this.ChildWidth = this.ChildHeight = this.ChildDepth = 0;
    }

    /// <summary>
    ///   Create a custom format.
    /// </summary>
    /// <param name="order">The octree size will be 2^order</param>
    public VoxelOctreeFormat(int order)
    {
      this.Width = this.Height = this.Depth = 1 << order;
      this.ChildWidth = this.ChildDepth = this.ChildHeight = 1 << (order - 1);
    }
  }
}
