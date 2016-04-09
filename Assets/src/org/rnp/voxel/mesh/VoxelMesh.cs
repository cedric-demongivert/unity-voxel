using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <summary>
  ///   A delegate that create new instances of voxel mesh.
  /// </summary>
  /// <param name="dimensions"></param>
  /// <returns></returns>
  public delegate VoxelMesh VoxelMeshFactory(Dimensions3D dimensions);

  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A color matrix that store voxel mesh datas, it can be finite or no.
  /// </summary>
  public abstract class VoxelMesh
  {
    /// <summary>
    ///     Listeners for commit.
    /// </summary>
    private HashSet<IVoxelMeshCommitListener> _listeners;

    /// <summary>
    ///   True if the mesh changed and had not been commited.
    /// </summary>
    private bool _isDirty;

    /// <summary>
    ///   Dimensions of the voxel mesh.
    /// </summary>
    public abstract Dimensions3D Dimensions
    {
      get;
    }

    /// <summary>
    ///   Get the minimum point of that voxel mesh (inclusive).
    /// </summary>
    public abstract VoxelLocation Start
    {
      get;
    }

    /// <summary>
    ///   True if the mesh is in a readonly state.
    /// </summary>
    public abstract bool IsReadonly
    {
      get;
    }

    /// <summary>
    ///   Listener enumerator.
    /// </summary>
    protected IEnumerable<IVoxelMeshCommitListener> Listeners
    {
      get
      {
        foreach(IVoxelMeshCommitListener listener in _listeners)
        {
          yield return listener;
        }
      }
    }

    /// <summary>
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual Color32 this[int x, int y, int z]
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
    ///   Get or set a voxel in the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual Color32 this[VoxelLocation location]
    {
      get
      {
        return this.Get(location);
      }
      set
      {
        this.Set(location, value);
      }
    }

    /// <summary>
    ///   True if the mesh changed and had not been commited.
    /// </summary>
    public virtual bool IsDirty
    {
      get
      {
        return this._isDirty;
      }
    }

    /// <summary>
    ///   Base constructor.
    /// </summary>
    public VoxelMesh()
    {
      this._listeners = new HashSet<IVoxelMeshCommitListener>();
      this._isDirty = false;
    }

    /// <summary>
    ///   Register a new listener that listen for commits of this mesh.
    /// </summary>
    /// <param name="listener"></param>
    public virtual void RegisterCommitListener(IVoxelMeshCommitListener listener)
    {
      if(!this._listeners.Contains(listener))
      {
        this._listeners.Add(listener);
        listener.OnRegister(this);
      }
    }

    /// <summary>
    ///   Unregister a registered commit listener.
    /// </summary>
    /// <param name="listener"></param>
    public virtual void UnregisterCommitListener(IVoxelMeshCommitListener listener)
    {
      if(this._listeners.Contains(listener))
      {
        this._listeners.Remove(listener);
        listener.OnUnregister(this);
      }
    }

    /// <summary>
    ///   Notify changes of the mesh.
    /// </summary>
    public virtual void Commit()
    {
      if (this.IsDirty)
      {
        foreach (IVoxelMeshCommitListener listener in this._listeners)
        {
          listener.OnCommitBegin(this);
        }

        foreach (IVoxelMeshCommitListener listener in this._listeners)
        {
          listener.OnCommitEnd(this);
        }

        this.MarkFresh();
      }
    }
    
    /// <summary>
    ///   Clean the voxel mesh.
    /// </summary>
    public virtual void Destroy()
    {
      foreach(IVoxelMeshCommitListener listener in new HashSet<IVoxelMeshCommitListener>(this._listeners))
      {
        this.UnregisterCommitListener(listener);
      }
    }

    /// <summary>
    ///   Mark this object has dirty.
    /// </summary>
    public virtual void MarkDirty()
    {
      this._isDirty = true;
    }

    /// <summary>
    /// </summary>
    public virtual void MarkFresh()
    {
      this._isDirty = false;
    }

    /// <summary>
    ///   Clear the voxel mesh.
    ///   All voxels of the mesh must be set to Voxels.Empty.
    /// </summary>
    public virtual void Clear()
    {
      VoxelMeshes.Fill(this, Voxels.Empty);
      this.MarkDirty();
    }

    /// <summary>
    ///   Get a Readonly implementation.
    ///   If the mesh is already in an Readonly state, this method must return it.
    /// </summary>
    /// <returns></returns>
    public abstract VoxelMesh Readonly();

    /// <summary>
    ///   Return a deep-copy of the voxel mesh.
    /// </summary>
    /// <returns></returns>
    public abstract VoxelMesh Copy();

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual bool Contains(int x, int y, int z)
    {
      VoxelLocation start = this.Start;
      Dimensions3D dimensions = this.Dimensions;

      return x >= start.X
             && y >= start.Y
             && z >= start.Z
             && x < start.X + dimensions.Width
             && y < start.Y + dimensions.Height
             && z < start.Z + dimensions.Depth;
    }

    /// <summary>
    ///   Check if a location is in the voxel mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual bool Contains(VoxelLocation location)
    {
      return this.Contains(location.X, location.Y, location.Z);
    }

    /// <summary>
    ///   Check if the mesh don't contains gaps.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsFull()
    {
      Dimensions3D dimensions = this.Dimensions;

      for (int x = 0; x < dimensions.Width; ++x)
      {
        for (int y = 0; y < dimensions.Height; ++y)
        {
          for (int z = 0; z < dimensions.Depth; ++z)
          {
            if (Voxels.IsEmpty(this[x, y, z]))
            {
              return false;
            }
          }
        }
      }

      return true;
    }

    /// <summary>
    ///   Check if the mesh contains data.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsEmpty()
    {
      Dimensions3D dimensions = this.Dimensions;

      for (int x = 0; x < dimensions.Width; ++x)
      {
        for (int y = 0; y < dimensions.Height; ++y)
        {
          for (int z = 0; z < dimensions.Depth; ++z)
          {
            if (Voxels.IsNotEmpty(this[x, y, z]))
            {
              return false;
            }
          }
        }
      }

      return true;
    }

    /// <summary>
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual bool IsEmpty(int x, int y, int z)
    {
      return !this.Contains(x,y,z) || Voxels.IsEmpty(this[x, y, z]);
    }

    /// <summary>
    ///   Check if the mesh contains data at a specific location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual bool IsEmpty(VoxelLocation location)
    {
      return !this.Contains(location) || Voxels.IsEmpty(this[location]);
    }

    /// <summary>
    ///   Set a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="value"></param>
    public abstract void Set(int x, int y, int z, Color32 value);

    /// <summary>
    ///   Set a voxel of the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="value"></param>
    public abstract void Set(VoxelLocation location, Color32 value);

    /// <summary>
    ///   Get a voxel of the mesh.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public abstract Color32 Get(int x, int y, int z);

    /// <summary>
    ///   Get a voxel of the mesh.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public abstract Color32 Get(VoxelLocation location);
  }
}
