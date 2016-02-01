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
  public abstract class AbstractWritableVoxelMesh : IWritableVoxelMesh
  {
    protected VoxelLocation _meshLocation;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public IVoxelLocation Start
    {
      get { return _meshLocation; }
      set { _meshLocation.Set(value);  }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public abstract int Width
    {
      get;
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public abstract int Height
    {
      get;
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public abstract int Depth
    {
      get;
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public abstract Color32 this[int x, int y, int z]
    {
      get;
      set;
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public Color32 this[IVoxelLocation location]
    {
      get 
      {
        return this[location.X, location.Y, location.Z];
      }
      set 
      {
        this[location.X, location.Y, location.Z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public Color32 this[Vector3 location]
    {
      get
      {
        return this[(int) location.x, (int) location.y, (int) location.z];
      }
      set
      {
        this[(int) location.x, (int) location.y, (int) location.z] = value;
      }
    }

    public AbstractWritableVoxelMesh()
    {
      this._meshLocation = new VoxelLocation();
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public void AbsoluteSet(int x, int y, int z, Color32 color)
    {
      this[x - this.Start.X, y - this.Start.Y, z - this.Start.Z] = color;
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public void AbsoluteSet(Vector3 location, Color32 color)
    {
      this[(int)location.x - this.Start.X, (int)location.y - this.Start.Y, (int)location.z - this.Start.Z] = color;
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public void AbsoluteSet(IVoxelLocation location, Color32 color)
    {
      this[location.X - this.Start.X, location.Y - this.Start.Y, location.Z - this.Start.Z] = color;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public Color32 AbsoluteGet(int x, int y, int z)
    {
      return this[x - this.Start.X, y - this.Start.Y, z - this.Start.Z];
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public Color32 AbsoluteGet(Vector3 location)
    {
      return this[(int)location.x - this.Start.X, (int)location.y - this.Start.Y, (int)location.z - this.Start.Z];
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public Color32 AbsoluteGet(IVoxelLocation location)
    {
      return this[location.X - this.Start.X, location.Y - this.Start.Y, location.Z - this.Start.Z];
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public abstract void Clear();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool Contains(int x, int y, int z)
    {
      return x >= 0 && y >= 0 && z >= 0 && x < this.Width && y < this.Height && z < this.Depth;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool Contains(Vector3 location)
    {
      return this.Contains((int)location.x, (int)location.y, (int)location.z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool Contains(IVoxelLocation location)
    {
      return this.Contains(location.X, location.Y, location.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool AbsoluteContains(int x, int y, int z)
    {
      return this.Contains(x - this.Start.X, y - this.Start.Y, z - this.Start.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool AbsoluteContains(Vector3 location)
    {
      return this.Contains(
        (int)location.x - this.Start.X, 
        (int)location.y - this.Start.Y, 
        (int)location.z - this.Start.Z
      );
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public bool AbsoluteContains(IVoxelLocation location)
    {
      return this.Contains(
        location.X - this.Start.X,
        location.Y - this.Start.Y,
        location.Z - this.Start.Z
      );
    }
  }
}
