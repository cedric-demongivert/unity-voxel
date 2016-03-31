using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A infinite mesh based on a map.
  /// </summary>
  public sealed class MapVoxelMesh : AbstractVoxelMesh, IMapVoxelMesh
  {
    private Dictionary<VoxelLocation, IVoxelMesh> _chunks;
    private IChunckBuilder _chunkBuilder;

    private VoxelLocation _min;
    private VoxelLocation _max;
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Dimensions3D Dimensions
    {
      get { return new Dimensions3D(_min, _max); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override VoxelLocation Start
    {
      get { return _min; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public Dimensions3D ChildDimensions
    {
      get { return this._chunkBuilder.ChunckDimensions; }
    }

    /// <summary>
    ///   An empty default MapVoxelMesh.
    /// </summary>
    public MapVoxelMesh()
    {
      this._chunks = new Dictionary<VoxelLocation, IVoxelMesh>();
      this._chunkBuilder = new OctreeChunckBuilder();
      this._max = new VoxelLocation();
      this._min = new VoxelLocation();
    }

    /// <summary>
    ///   Create a MapVoxelMesh with a custom chunck configuration.
    /// </summary>
    /// <param name="builder"></param>
    public MapVoxelMesh(IChunckBuilder builder)
    {
      this._chunks = new Dictionary<VoxelLocation, IVoxelMesh>();
      this._chunkBuilder = builder;
      this._max = new VoxelLocation();
      this._min = new VoxelLocation();
    }

    /// <summary>
    ///   Copy an existing MapVoxelMesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public MapVoxelMesh(MapVoxelMesh toCopy)
    {
      this._chunks = new Dictionary<VoxelLocation, IVoxelMesh>();
      this._chunkBuilder = (IChunckBuilder) toCopy._chunkBuilder.Copy();
      this._max = toCopy._max;
      this._min = toCopy._min;

      VoxelMeshes.Copy(toCopy, this);
    }

    /// <summary>
    ///   Copy an existing VoxelMesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public MapVoxelMesh(IVoxelMesh toCopy)
    {
      this._chunks = new Dictionary<VoxelLocation, IVoxelMesh>();
      this._chunkBuilder = new OctreeChunckBuilder();

      VoxelMeshes.Copy(toCopy, this);
      this.EvaluateSize();
    }

    /// <summary>
    ///   Copy an existing VoxelMesh with a custom chunck configuration.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="builder"></param>
    public MapVoxelMesh(IVoxelMesh toCopy, IChunckBuilder builder)
    {
      this._chunks = new Dictionary<VoxelLocation, IVoxelMesh>();
      this._chunkBuilder = builder;

      VoxelMeshes.Copy(toCopy, this);
      this.EvaluateSize();
    }

    /// <summary>
    ///   Check the actual size of the map.
    /// </summary>
    private void EvaluateSize()
    {
      int minX = 0; int minY = 0; int minZ = 0;
      int maxX = 0; int maxY = 0; int maxZ = 0;

      foreach(VoxelLocation location in this._chunks.Keys) {
        if (location.X < minX) minX = location.X;
        if (location.Y < minY) minY = location.Y;
        if (location.Z < minZ) minZ = location.Z;

        if (location.X + 1 > maxX) maxX = location.X + 1;
        if (location.Y + 1 > maxY) maxY = location.Y + 1;
        if (location.Z + 1 > maxZ) maxZ = location.Z + 1;
      }

      this._min = new VoxelLocation(minX, minY, minZ).Mul(this.ChildDimensions);
      this._max = new VoxelLocation(maxX, maxY, maxZ).Mul(this.ChildDimensions);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public HashSet<VoxelLocation> Keys()
    {
      return new HashSet<VoxelLocation>(this._chunks.Keys);
    }

    /// <summary>
    ///   Transform an absolute location to a chunck location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public VoxelLocation ToChunckLocation(int x, int y, int z)
    {
      return new VoxelLocation(
        x / (float) this.ChildDimensions.Width,
        y / (float) this.ChildDimensions.Height,
        z / (float) this.ChildDimensions.Depth
      );
    }

    /// <summary>
    ///   Get the chunck that contains the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private IVoxelMesh GetChunckAt(int x, int y, int z)
    {
      VoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

      if (this._chunks.ContainsKey(chunckLocation))
      {
        return this._chunks[chunckLocation];
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    ///   Get the chunck that contains the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private void DeleteChunckAt(int x, int y, int z)
    {
      VoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

      this._chunks.Remove(chunckLocation);

      VoxelLocation max = new VoxelLocation(chunckLocation);
      VoxelLocation min = new VoxelLocation(chunckLocation);
      max.Add(1, 1, 1).Mul(this.ChildDimensions);
      min.Mul(this.ChildDimensions);

      if(min.AnyEquals(this._min) || max.AnyEquals(this._max))
      {
        this.EvaluateSize();
      }
    }

    /// <summary>
    ///   Create a chunck at a specified location.
    /// </summary>
    /// <param name="chunckLocation"></param>
    /// <returns></returns>
    private IVoxelMesh CreateChunck(VoxelLocation chunckLocation)
    {
      IVoxelMesh chunck = this._chunkBuilder.Build();
      this._chunks[chunckLocation] = chunck;

      VoxelLocation max = new VoxelLocation(chunckLocation).Add(1, 1, 1).Mul(this.ChildDimensions);
      VoxelLocation min = new VoxelLocation(chunckLocation).Mul(this.ChildDimensions);
      this._min = this._min.SetIfMin(min);
      this._max = this._max.SetIfMax(max);

      return chunck;
    }

    /// <summary>
    ///   Get (or create) the chunck that contains the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private IVoxelMesh GetChunckAtOrCreate(int x, int y, int z)
    {
      VoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

      if (this._chunks.ContainsKey(chunckLocation))
      {
        return this._chunks[chunckLocation];
      }
      else
      {
        return this.CreateChunck(chunckLocation);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      return this.GetChild(new VoxelLocation(x, y, z));
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(VoxelLocation location)
    {
      if (this._chunks.ContainsKey(location))
      {
        return this._chunks[location].ReadOnly();
      }
      else
      {
        return null;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public VoxelLocation ToLocale(int x, int y, int z)
    {
      VoxelLocation location = new VoxelLocation(x, y, z).Mod(this.ChildDimensions);
      
      return location.Add(
        (location.X < 0) ? this.ChildDimensions.Width : 0,
        (location.Y < 0) ? this.ChildDimensions.Height : 0,
        (location.Z < 0) ? this.ChildDimensions.Depth : 0
      );
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      IVoxelMesh chunck = this.GetChunckAt(x, y, z);
      if (chunck == null)
      {
        return Voxels.Empty;
      }
      else
      {
        return chunck[this.ToLocale(x, y, z)];
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      return this.Get(location.X, location.Y, location.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 color)
    {
      this.Set(location.X, location.Y, location.Z, color);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 color)
    {
      if (color.a == 255)
      {
        IVoxelMesh chunck = this.GetChunckAt(x, y, z);
        if (chunck != null)
        {
          chunck[this.ToLocale(x, y, z)] = Voxels.Empty;

          if (chunck.IsEmpty())
          {
            this.DeleteChunckAt(x, y, z);
          }
        }
      }
      else
      {
        this.GetChunckAtOrCreate(x, y, z)[
          this.ToLocale(x, y, z)
        ] = color;
      }

      this.Touch();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      foreach(IVoxelMesh chunck in this._chunks.Values)
      {
        if (!chunck.IsFull())
        {
          return false;
        }
      }

      return true;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsEmpty()
    {
      return this._chunks.Keys.Count == 0;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Clear()
    {
      this._chunks.Clear();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new MapVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return new ReadonlyMapVoxelMesh(this);
    }
  }
}
