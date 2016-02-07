using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.octree;

namespace org.rnp.voxel.mesh.builder
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A builder that made octree chuncks.
  /// </summary>
  public sealed class OctreeChunckBuilder : IChunckBuilder
  {
    private readonly OctreeVoxelMeshFormat _chunckFormat;

    /// <see cref="org.rnp.voxel.mesh.builder.IChunckBuilder"/>
    public int Width
    {
      get { return this._chunckFormat.Width; }
    }

    /// <see cref="org.rnp.voxel.mesh.builder.IChunckBuilder"/>
    public int Height
    {
      get { return this._chunckFormat.Height; }
    }

    /// <see cref="org.rnp.voxel.mesh.builder.IChunckBuilder"/>
    public int Depth
    {
      get { return this._chunckFormat.Depth; }
    }

    public OctreeChunckBuilder() {
      this._chunckFormat = OctreeVoxelMeshFormat.Tiny;
    }

    public OctreeChunckBuilder(OctreeVoxelMeshFormat format)
    {
      this._chunckFormat = format;
    }

    public OctreeChunckBuilder(OctreeChunckBuilder toCopy)
    {
      this._chunckFormat = toCopy._chunckFormat;
    }

    /// <see cref="org.rnp.voxel.mesh.builder.IVoxelMeshBuilder"/>
    public IVoxelMesh Build()
    {
      return new OctreeVoxelMesh(this._chunckFormat);
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"/>
    public IVoxelMeshBuilder Copy()
    {
      return new OctreeChunckBuilder(this);
    }
  }
}
