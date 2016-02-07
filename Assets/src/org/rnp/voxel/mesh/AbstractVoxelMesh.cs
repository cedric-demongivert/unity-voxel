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
    ///   Get the minimum point of that voxel mesh (inclusive).
    /// </summary>
    public abstract IVoxelLocation Start { get; }

    /// <summary>
    ///   Get the end point of that voxel mesh (exclusive).
    /// </summary>
    public abstract IVoxelLocation End { get; }

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

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract Color32 this[int x, int y, int z]
    {
      get;
      set;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
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

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
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

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract void Clear();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(int x, int y, int z)
    {
      return x >= 0 && y >= 0 && z >= 0 && x < this.Width && y < this.Height && z < this.Depth;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(Vector3 location)
    {
      return this.Contains((int)location.x, (int)location.y, (int)location.z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool Contains(IVoxelLocation location)
    {
      return this.Contains(location.X, location.Y, location.Z);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract bool IsEmpty();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(int x, int y, int z)
    {
      return !this.Contains(x,y,z) || this[x, y, z].a == 255;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(Vector3 location)
    {
      return !this.Contains(location) || this[location].a == 255;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual bool IsEmpty(IVoxelLocation location)
    {
      return !this.Contains(location) || this[location].a == 255;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual void Copy(IVoxelLocation from, IVoxelLocation to, IVoxelLocation where, IVoxelMesh toCopy)
    {
      Dimensions3D size = new Dimensions3D(
        to.X - from.X, to.Y - from.Y, to.Z - from.Z
      );

      this.Copy(from, size, where, toCopy);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public virtual void Copy(IVoxelLocation start, IDimensions3D size, IVoxelLocation where, IVoxelMesh toCopy)
    {
      for (int x = 0; x < size.Width; ++x)
      {
        for (int y = 0; y < size.Height; ++y)
        {
          for (int z = 0; z < size.Depth; ++z)
          {
            this[where.X + x, where.Y + y, where.Z + z] = toCopy[start.X + x, start.Y + y, start.Z + z];
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract IVoxelMesh Copy();

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public abstract IReadonlyVoxelMesh ReadOnly();
  }
}
