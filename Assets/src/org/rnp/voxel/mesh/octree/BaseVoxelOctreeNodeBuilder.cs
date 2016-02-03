using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using UnityEngine;

namespace rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A base voxel octree node builder for simple octrees, this builder
  /// make octree node until a specific size limit is reached. Then, this builder
  /// will return somes VoxelArray.
  /// </summary>
  public sealed class BaseVoxelOctreeNodeBuilder : IVoxelOctreeNodeBuilder
  {
    private readonly int _minSize;

    /// <summary>
    ///   A basic builder that stop to produce octrees for
    /// 8x8x8 (or less) box.
    /// </summary>
    public BaseVoxelOctreeNodeBuilder()
    {
      this._minSize = 4;
    }

    /// <summary>
    ///   A custom basic builder.
    /// </summary>
    /// <param name="minNodeSize"></param>
    public BaseVoxelOctreeNodeBuilder(int minNodeSize)
    {
      this._minSize = minNodeSize;
    }

    /// <summary>
    ///   Copy an existing builder.
    /// </summary>
    /// <param name="toCopy"></param>
    public BaseVoxelOctreeNodeBuilder(BaseVoxelOctreeNodeBuilder toCopy)
    {
      this._minSize = toCopy._minSize;
    }

    /// <summary>
    ///   Check if the VoxelMesh to instanciate must be a leaf, or a node.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private bool IsLeaf(int width, int height, int depth)
    {
      return width <= 2
             || height <= 2
             || depth <= 2
             || width <= this._minSize
             || height <= this._minSize
             || depth <= this._minSize;
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IVoxelOctreeNodeBuilder"/>
    public IWritableVoxelMesh Build(int width, int height, int depth)
    {
      if (this.IsLeaf(width, height, depth))
      {
        return new VoxelArray(width, height, depth);
      }
      else
      {
        return new VoxelOctree(VoxelOctreeFormat.GetFormat(width, height, depth), this.Copy());
      }
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IVoxelOctreeNodeBuilder"/>
    public IVoxelOctreeNodeBuilder Copy()
    {
      return new BaseVoxelOctreeNodeBuilder(this);
    }
  }
}