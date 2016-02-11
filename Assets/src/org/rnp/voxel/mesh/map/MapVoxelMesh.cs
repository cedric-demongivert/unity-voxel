using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.builder;
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
    private Dictionary<IVoxelLocation, IVoxelMesh> _chunks;
    private IChunckBuilder _chunkBuilder;

    private IVoxelLocation _min;
    private IVoxelLocation _max;

    private ReadonlyMapVoxelMesh _readOnly = null;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override int Width
    {
      get { return this._max.X - this._min.X; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override int Height
    {
      get { return this._max.Y - this._min.Y; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override int Depth
    {
      get { return this._max.Z - this._min.Z; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelLocation Start
    {
      get { return new VoxelLocation(this._min); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelLocation End
    {
      get { return new VoxelLocation(this._max); }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildWidth
    {
      get { return this._chunkBuilder.Width; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildHeight
    {
      get { return this._chunkBuilder.Height; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildDepth
    {
      get { return this._chunkBuilder.Depth; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 this[int x, int y, int z]
    {
      get
      {
        return this.Get(x, y, z);
      }
      set
      {
        this.Set(x, y, z, value);
      }
    }

    /// <summary>
    ///   An empty default MapVoxelMesh.
    /// </summary>
    public MapVoxelMesh()
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
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
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
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
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = (IChunckBuilder)toCopy._chunkBuilder.Copy();
      this._max = new VoxelLocation(toCopy._max);
      this._min = new VoxelLocation(toCopy._min);

      this.Copy(toCopy.Start, toCopy.End, toCopy.Start, toCopy);
    }

    /// <summary>
    ///   Copy an existing VoxelMesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public MapVoxelMesh(IVoxelMesh toCopy)
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = new OctreeChunckBuilder();

      this.Copy(toCopy.Start, toCopy.End, toCopy.Start, toCopy);
      this.EvaluateSize();
    }

    /// <summary>
    ///   Copy an existing VoxelMesh with a custom chunck configuration.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="builder"></param>
    public MapVoxelMesh(IVoxelMesh toCopy, IChunckBuilder builder)
    {
      this._chunks = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._chunkBuilder = builder;

      this.Copy(toCopy.Start, toCopy.End, toCopy.Start, toCopy);
      this.EvaluateSize();
    }

    /// <summary>
    ///   Check the actual size of the map.
    /// </summary>
    private void EvaluateSize()
    {
      this._min.Set(0,0,0);
      this._max.Set(0,0,0);

      foreach(IVoxelLocation location in this._chunks.Keys) {
        if (location.X < this._min.X) this._min.X = location.X;
        if (location.Y < this._min.Y) this._min.Y = location.Y;
        if (location.Z < this._min.Z) this._min.Z = location.Z;

        if (location.X + 1 > this._max.X) this._max.X = location.X + 1;
        if (location.Y + 1 > this._max.Y) this._max.Y = location.Y + 1;
        if (location.Z + 1 > this._max.Z) this._max.Z = location.Z + 1;
      }

      this._min.Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);
      this._max.Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public HashSet<IVoxelLocation> Keys()
    {
      return new HashSet<IVoxelLocation>(this._chunks.Keys);
    }

    /// <summary>
    ///   Transform an absolute location to a chunck location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public IVoxelLocation ToChunckLocation(int x, int y, int z)
    {
      if (x < 0) x -= this.ChildWidth;
      if (y < 0) y -= this.ChildHeight;
      if (z < 0) z -= this.ChildDepth;

      return new VoxelLocation(x / this.ChildWidth, y / this.ChildHeight, z / this.ChildDepth);
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
      IVoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

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
      IVoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

      this._chunks.Remove(chunckLocation);

      VoxelLocation max = new VoxelLocation(chunckLocation);
      VoxelLocation min = new VoxelLocation(chunckLocation);
      max.Add(1, 1, 1).Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);
      min.Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);

      if(min.X == this._min.X || min.Y == this._min.Y || min.Z == this._min.Z ||
         max.X == this._max.X || max.Y == this._max.Y || max.Z == this._max.Z)
      {
        this.EvaluateSize();
      }
    }

    /// <summary>
    ///   Create a chunck at a specified location.
    /// </summary>
    /// <param name="chunckLocation"></param>
    /// <returns></returns>
    private IVoxelMesh CreateChunck(IVoxelLocation chunckLocation)
    {
      IVoxelMesh chunck = this._chunkBuilder.Build();
      this._chunks[chunckLocation] = chunck;

      VoxelLocation max = new VoxelLocation(chunckLocation);
      VoxelLocation min = new VoxelLocation(chunckLocation);
      max.Add(1, 1, 1).Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);
      min.Mul(this.ChildWidth, this.ChildHeight, this.ChildDepth);

      if (min.X < this._min.X) this._min.X = min.X;
      if (min.Y < this._min.Y) this._min.Y = min.Y;
      if (min.Z < this._min.Z) this._min.Z = min.Z;

      if (max.X > this._max.X) this._max.X = max.X;
      if (max.Y > this._max.Y) this._max.Y = max.Y;
      if (max.Z > this._max.Z) this._max.Z = max.Z;

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
      IVoxelLocation chunckLocation = this.ToChunckLocation(x, y, z);

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
    public IReadonlyVoxelMesh GetChild(IVoxelLocation location)
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
    public IVoxelLocation ToLocale(int x, int y, int z)
    {
      VoxelLocation location = new VoxelLocation(
        x % this.ChildWidth, y % this.ChildHeight, z % this.ChildDepth
      );

      if (location.X < 0) location.X += this.ChildWidth;
      if (location.Y < 0) location.Y += this.ChildHeight;
      if (location.Z < 0) location.Z += this.ChildDepth;

      return location;
    }

    /// <summary>
    ///   Get a voxel of the map.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Color32 Get(int x, int y, int z)
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

    /// <summary>
    ///   Set a voxel of the map.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    public void Set(int x, int y, int z, Color32 color)
    {
      if (color.a == 255)
      {
        IVoxelMesh chunck = this.GetChunckAt(x, y, z);
        if (chunck != null)
        {
          chunck[
            this.ToLocale(x, y, z)
          ] = Voxels.Empty;

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
      if (this._readOnly == null)
      {
        this._readOnly = new ReadonlyMapVoxelMesh(this);
      }

      return this._readOnly;
    }
  }
}
