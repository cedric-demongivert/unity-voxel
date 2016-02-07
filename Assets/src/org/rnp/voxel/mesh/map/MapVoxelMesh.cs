using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.builder;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A infinite mesh based on a map.
  /// </summary>
  public sealed class MapVoxelMesh : AbstractVoxelMesh, IMapVoxelMesh
  {
    private Dictionary<IVoxelLocation, IVoxelMesh> _chunks;
    private IChunckBuilder _chunkBuilder;

    public int ChildWidth
    {
      get { return this._chunkBuilder.Width; }
    }

    public int ChildHeight
    {
      get { return this._chunkBuilder.Height; }
    }

    public int ChildDepth
    {
      get { return this._chunkBuilder.Depth; }
    }

    public MapVoxelMesh()
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = new OctreeChunckBuilder();
    }

    public MapVoxelMesh(IChunckBuilder builder)
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = builder;
    }

    public MapVoxelMesh(MapVoxelMesh toCopy)
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = toCopy._chunkBuilder.Copy();
    }
  }
}
