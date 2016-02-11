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
  public sealed class OctreeVoxelMeshFormat
  {
    /// <summary>
    ///   A 0x0x0 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Empty = new OctreeVoxelMeshFormat();
    
    /// <summary>
    ///   A 64x64x64 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Tiny = new OctreeVoxelMeshFormat(6);

    /// <summary>
    ///   A 128x128x128 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Small = new OctreeVoxelMeshFormat(7);

    /// <summary>
    ///   A 256x256x256 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Medium = new OctreeVoxelMeshFormat(8);

    /// <summary>
    ///   A 512x512x512 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Big = new OctreeVoxelMeshFormat(9);

    /// <summary>
    ///   A 1024x1024x1024 voxel octree.
    /// </summary>
    public static readonly OctreeVoxelMeshFormat Gigantic = new OctreeVoxelMeshFormat(10);

    /// <summary>
    ///   For determinate childrens.
    /// </summary>
    private static readonly OctreeVoxelMeshFormat[] _formats = new OctreeVoxelMeshFormat[] {
      new OctreeVoxelMeshFormat(0), new OctreeVoxelMeshFormat(1), new OctreeVoxelMeshFormat(2),
      new OctreeVoxelMeshFormat(3), new OctreeVoxelMeshFormat(4), new OctreeVoxelMeshFormat(5),
      Tiny, Small, Medium, Big, Gigantic
    };

    /// <summary>
    ///   Get optimized OctreeVoxelMeshFormat.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private static OctreeVoxelMeshFormat Get(int order)
    {
      if(order >= 0 && order <= 10) return OctreeVoxelMeshFormat._formats[order];
      else return new OctreeVoxelMeshFormat(order);
    }

    /// <summary>
    ///   Get a octree format for a specific size.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public static OctreeVoxelMeshFormat GetFormat(int width, int height, int depth)
    {
      int order = 0;
      int value = 1;
      
      if (width == 0 && height == 0 && depth == 0) return OctreeVoxelMeshFormat.Empty;

      while (width - value > 1 || height - value > 1 || depth - value > 1)
      {
        order += 1;
        value = value << 1;
      }

      return OctreeVoxelMeshFormat.Get(order);
    }

    /// <summary>
    ///   Node width.
    /// </summary>
    public readonly int Width;

    /// <summary>
    ///   Node height.
    /// </summary>
    public readonly int Height;

    /// <summary>
    ///   Node depth.
    /// </summary>
    public readonly int Depth;

    /// <summary>
    ///   Node order of two.
    /// </summary>
    public readonly int Order;

    /// <summary>
    ///   Child singleton.
    /// </summary>
    private OctreeVoxelMeshFormat _childFormat = null;

    /// <summary>
    ///   Get node child format.
    /// </summary>
    public OctreeVoxelMeshFormat ChildFormat {
      get {
        if(this.Order <= 0) return null;

        if(this._childFormat == null) {
          this._childFormat = OctreeVoxelMeshFormat.Get(this.Order - 1);
        }

        return this._childFormat;
      }
    }

    /// <summary>
    ///   Create an empty octree format.
    /// </summary>
    public OctreeVoxelMeshFormat()
    {
      this.Width = this.Height = this.Depth = 0;
      this.Order = 0;
    }

    /// <summary>
    ///   Create a custom format.
    /// </summary>
    /// <param name="order">The octree size will be 2^order</param>
    public OctreeVoxelMeshFormat(int order)
    {
      this.Width = this.Height = this.Depth = 1 << order;
      this.Order = order;
    }
  }
}
