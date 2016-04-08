using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A infinite mesh based on a map.
  /// </summary>
  public sealed class MapVoxelMesh : ChunckedVoxelMesh
  {
    private Dictionary<VoxelLocation, ChunckVoxelMesh> _chunks;

    private VoxelMeshFactory _chunkFactory;
    private Dimensions3D _chunckDimensions;

    private VoxelLocation _min;
    private VoxelLocation _max;
    
    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Dimensions3D Dimensions
    {
      get { return new Dimensions3D(_min, _max); }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelLocation Start
    {
      get { return _min; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override Dimensions3D ChunckDimensions
    {
      get { return this._chunckDimensions; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.ChunckVoxelMesh"/>
    public override IEnumerable<VoxelLocation> ChunckLocations
    {
      get
      {
        foreach(VoxelLocation key in this._chunks.Keys)
        {
          yield return key;
        }
      }
    }
    
    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsReadonly
    {
      get { return false; }
    }

    /// <summary>
    ///   An empty default MapVoxelMesh.
    /// </summary>
    /// <param name="chunckDimensions"></param>
    public MapVoxelMesh(Dimensions3D chunckDimensions) : base()
    {
      this._chunks = new Dictionary<VoxelLocation, ChunckVoxelMesh>();
      this._chunkFactory = ArrayVoxelMesh.Create;
      this._chunckDimensions = chunckDimensions;
      this._max = new VoxelLocation();
      this._min = new VoxelLocation();
    }

    /// <summary>
    ///   Create a MapVoxelMesh with a custom chunck configuration.
    /// </summary>
    /// <param name="chunckDimensions"></param>
    /// <param name="factory"></param>
    public MapVoxelMesh(Dimensions3D chunckDimensions, VoxelMeshFactory factory) : base()
    {
      this._chunks = new Dictionary<VoxelLocation, ChunckVoxelMesh>();
      this._chunkFactory = factory;
      this._chunckDimensions = chunckDimensions;
      this._max = new VoxelLocation();
      this._min = new VoxelLocation();
    }

    /// <summary>
    ///   Copy an existing MapVoxelMesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public MapVoxelMesh(MapVoxelMesh toCopy) : base()
    {
      this._chunks = new Dictionary<VoxelLocation, ChunckVoxelMesh>();
      this._chunkFactory = toCopy._chunkFactory;
      this._chunckDimensions = toCopy.ChunckDimensions;
      this._max = toCopy._max;
      this._min = toCopy._min;

      VoxelMeshes.Copy(toCopy, this);
    }

    /// <summary>
    ///   Copy an existing VoxelMesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public MapVoxelMesh(Dimensions3D chunckDimensions, VoxelMesh toCopy) : base()
    {
      this._chunks = new Dictionary<VoxelLocation, ChunckVoxelMesh>();
      this._chunkFactory = ArrayVoxelMesh.Create;
      this._chunckDimensions = chunckDimensions;

      VoxelMeshes.Copy(toCopy, this);
      this.EvaluateSize();
    }

    /// <summary>
    ///   Copy an existing VoxelMesh with a custom chunck configuration.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="builder"></param>
    public MapVoxelMesh(Dimensions3D chunckDimensions, VoxelMesh toCopy, VoxelMeshFactory builder) : base()
    {
      this._chunks = new Dictionary<VoxelLocation, ChunckVoxelMesh>();
      this._chunkFactory = builder;
      this._chunckDimensions = chunckDimensions;

      VoxelMeshes.Copy(toCopy, this);
      this.EvaluateSize();
    }
    
    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override void Commit()
    {
      if(this.IsDirty)
      {
        foreach(IVoxelMeshCommitListener listener in this.Listeners)
        {
          listener.OnCommitBegin(this);
        }

        foreach (VoxelMesh child in this._chunks.Values)
        {
          child.Commit();
        }

        foreach (IVoxelMeshCommitListener listener in this.Listeners)
        {
          listener.OnCommitEnd(this);
        }

        this.MarkFresh();
      }
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

      this._min = new VoxelLocation(minX, minY, minZ).Mul(this._chunckDimensions);
      this._max = new VoxelLocation(maxX, maxY, maxZ).Mul(this._chunckDimensions);
    }

    /// <summary>
    ///   Get the chunck that contains the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private VoxelMesh GetWritableChunckAt(int x, int y, int z)
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
      max.Add(1, 1, 1).Mul(this._chunckDimensions);
      min.Mul(this._chunckDimensions);

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
    private VoxelMesh CreateChunck(VoxelLocation chunckLocation)
    {
      VoxelMesh chunckContainer = this._chunkFactory(this._chunckDimensions);
      ChunckVoxelMesh chunck = new ChunckVoxelMesh(this, chunckLocation, chunckContainer);
      this._chunks[chunckLocation] = chunck;

      VoxelLocation max = new VoxelLocation(chunckLocation).Add(1, 1, 1).Mul(this._chunckDimensions);
      VoxelLocation min = new VoxelLocation(chunckLocation).Mul(this._chunckDimensions);
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
    private VoxelMesh GetOrCreateChunckAt(int x, int y, int z)
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


    /// <see cref="org.rnp.voxel.mesh.ChunckedVoxelMesh"/>
    public override ChunckVoxelMesh GetChunck(int x, int y, int z)
    {
      return this.GetChunck(new VoxelLocation(x, y, z));
    }

    /// <see cref="org.rnp.voxel.mesh.ChunckedVoxelMesh"/>
    public override ChunckVoxelMesh GetChunck(VoxelLocation location)
    {
      if (this._chunks.ContainsKey(location))
      {
        return this._chunks[location];
      }
      else
      {
        return null;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.ChunckedVoxelMesh"/>
    public override ChunckVoxelMesh GetChunckAt(int x, int y, int z)
    {
      return this.GetChunckAt(new VoxelLocation(x, y, z));
    }

    /// <see cref="org.rnp.voxel.mesh.ChunckedVoxelMesh"/>
    public override ChunckVoxelMesh GetChunckAt(VoxelLocation location)
    {
      VoxelLocation chunckLocation = this.ToChunckLocation(location);

      if (this._chunks.ContainsKey(chunckLocation))
      {
        return this._chunks[chunckLocation];
      }
      else
      {
        return null;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      VoxelMesh chunck = this.GetWritableChunckAt(x, y, z);

      if (chunck == null)
      {
        return Voxels.Empty;
      }
      else
      {
        return chunck[this.ToLocale(x, y, z)];
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      return this.Get(location.X, location.Y, location.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 color)
    {
      this.Set(location.X, location.Y, location.Z, color);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 color)
    {
      if (color.a == 255)
      {
        VoxelMesh chunck = this.GetWritableChunckAt(x, y, z);
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
        this.GetOrCreateChunckAt(x, y, z)[
          this.ToLocale(x, y, z)
        ] = color;
      }

      this.MarkDirty();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsFull()
    {
      foreach(VoxelMesh chunck in this._chunks.Values)
      {
        if (!chunck.IsFull())
        {
          return false;
        }
      }

      return true;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsEmpty()
    {
      return this._chunks.Keys.Count == 0;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Clear()
    {
      this._chunks.Clear();
      this.MarkDirty();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Copy()
    {
      return new MapVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Readonly()
    {
      return new ReadonlyChunckedVoxelMesh(this);
    }
  }
}
