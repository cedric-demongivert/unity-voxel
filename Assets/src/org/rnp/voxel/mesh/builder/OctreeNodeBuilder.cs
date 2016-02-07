using UnityEngine;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.array;
using org.rnp.voxel.mesh.octree;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A base voxel octree node builder for simple octrees, this builder
  /// make octree node until a specific size limit is reached. Then, this builder
  /// will return somes ArrayVoxelMesh.
  /// </summary>
  public sealed class OctreeNodeBuilder : IOctreeNodeBuilder
  {
    private OctreeVoxelMeshFormat _format;

    public OctreeVoxelMeshFormat Format
    {
      get { return this._format; }
      set { this._format = value; }
    }

    private readonly OctreeVoxelMeshFormat _minFormat;

    /// <summary>
    ///   A basic builder that stop to produce octrees for
    /// 8x8x8 (or less) box.
    /// </summary>
    public OctreeNodeBuilder()
    {
      this._format = OctreeVoxelMeshFormat.Empty;
      this._minFormat = OctreeVoxelMeshFormat.GetFormat(4,4,4);
    }

    /// <summary>
    ///   A custom basic builder.
    /// </summary>
    /// <param name="minNodeSize"></param>
    public OctreeNodeBuilder(OctreeVoxelMeshFormat minFormat)
    {
      this._format = OctreeVoxelMeshFormat.Empty;
      this._minFormat = minFormat;
    }

    /// <summary>
    ///   Copy an existing builder.
    /// </summary>
    /// <param name="toCopy"></param>
    public OctreeNodeBuilder(OctreeNodeBuilder toCopy)
    {
      this._format = toCopy._format;
      this._minFormat = toCopy._minFormat;
    }

    /// <summary>
    ///   Check if the VoxelMesh to instanciate must be a leaf, or a node.
    /// </summary>
    /// <returns></returns>
    private bool IsLeaf()
    {
      return this._format.Order <= this._minFormat.Order || this._format.Order <= 1;
    }

    /// <see cref="org.rnp.voxel.mesh.builder.IVoxelMeshBuilder"/>
    public IVoxelMesh Build()
    {
      if (this.IsLeaf())
      {
        return new ArrayVoxelMesh(
          this._format.Width,
          this._format.Height,
          this._format.Depth
        );
      }
      else
      {
        return new OctreeVoxelMesh(
          this._format,
          this
        );
      }
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"/>
    public IVoxelMeshBuilder Copy()
    {
      return new OctreeNodeBuilder(this);
    }
  }
}