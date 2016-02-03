using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A basic implementation.
  /// </summary>
  public sealed class VoxelLocation : IVoxelLocation
  {
    private int _x;
    private int _y;
    private int _z;

    public static readonly VoxelLocation Zero = new VoxelLocation();

    public static Vector3 ToVector3(IVoxelLocation location)
    {
      return new Vector3(
        location.X,
        location.Y,
        location.Z
      );
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public int X 
    {
      get { return this._x; }
      set { this._x = value; }
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public int Y
    {
      get { return this._y; }
      set { this._y = value; }
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public int Z
    {
      get { return this._z; }
      set { this._z = value; }
    }
    
    public VoxelLocation()
    {
      this._x = this._y = this._z = 0;
    }

    public VoxelLocation(int x, int y, int z)
    {
      this._x = x;
      this._y = y;
      this._z = z;
    }

    public VoxelLocation(IVoxelLocation toCopy)
    {
      this._x = toCopy.X;
      this._y = toCopy.Y;
      this._z = toCopy.Z;
    }

    public VoxelLocation(Vector3 vec)
    {
      this._x = (int)vec.x;
      this._y = (int)vec.y;
      this._z = (int)vec.z;
    }

    public VoxelLocation(Vector2 vec)
    {
      this._x = (int)vec.x;
      this._y = (int)vec.y;
      this._z = 0;
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public IVoxelLocation Set(IVoxelLocation location)
    {
      this._x = location.X;
      this._y = location.Y;
      this._z = location.Z;
      return this;
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public IVoxelLocation Set(int x, int y, int z)
    {
      this._x = x;
      this._y = y;
      this._z = z;
      return this;
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public IVoxelLocation Add(IVoxelLocation location)
    {
      this._x += location.X;
      this._y += location.Y;
      this._z += location.Z;
      return this;
    }

    /// <see cref="org.rnp.utils.IVoxelLocation"/>
    public IVoxelLocation Add(int x, int y, int z)
    {
      this._x += x;
      this._y += y;
      this._z += z;
      return this;
    }

    /// <see cref="Object"/>
    public override string ToString()
    {
      return "VoxelLocation (" + this._x 
             + ", " + this._y + ", " + this._z + ")";
    }

    /// <see cref="Object"/>
    public override int GetHashCode()
    {
      return ((this._x) * 31 + this._y) * 61 + this._z) * 97;
    }
  }
}
