using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A helper implementation class.
  /// </summary>
  public abstract class AbstractVoxelMesh : IVoxelMesh
  {
    /// <summary>
    ///   Last update operation time in milliseconds.
    /// </summary>
    private long _lastUpdateTime;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public virtual long LastUpdateTime
    {
      get
      {
        return this._lastUpdateTime;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public abstract Dimensions3D Dimensions
    {
      get;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public abstract VoxelLocation Start
    {
      get;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public Color32 this[int x, int y, int z]
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

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public Color32 this[VoxelLocation location]
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
    ///   Basic constructor.
    /// </summary>
    public AbstractVoxelMesh()
    {
      this._lastUpdateTime = 0;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public void Touch()
    {
      this._lastUpdateTime = (long)(Time.time*1000);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract void Clear();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(int x, int y, int z)
    {
      VoxelLocation start = this.Start;
      Dimensions3D dimensions = this.Dimensions;
      return    x >= start.X 
             && y >= start.Y 
             && z >= start.Z 
             && x < start.X + dimensions.Width
             && y < start.Y + dimensions.Height
             && z < start.Z + dimensions.Depth;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(VoxelLocation location)
    {
      return this.Contains(location.X, location.Y, location.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(Vector3 location)
    {
      return this.Contains(new VoxelLocation(location));
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract bool IsFull();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract bool IsEmpty();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(int x, int y, int z)
    {
      return !this.Contains(x,y,z) || Voxels.IsEmpty(this[x, y, z]);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(Vector3 location)
    {
      return !this.Contains(location) || Voxels.IsEmpty(this[location]);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(VoxelLocation location)
    {
      return !this.Contains(location) || Voxels.IsEmpty(this[location]);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract void Set(int x, int y, int z, Color32 value);

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract void Set(VoxelLocation location, Color32 value);

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract Color32 Get(int x, int y, int z);

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract Color32 Get(VoxelLocation location);

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract IVoxelMesh Copy();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract IReadonlyVoxelMesh ReadOnly();
  }
}
