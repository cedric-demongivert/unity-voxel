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

    public int X 
    {
      get { return this._x; }
      set { this._x = value; }
    }

    public int Y
    {
      get { return this._y; }
      set { this._y = value; }
    }

    public int Z
    {
      get { return this._z; }
      set { this._z = value; }
    }

    public Vector3 Vector
    {
      get
      {
        return new Vector3(this._x, this._y, this._z);
      }
      set
      {
        this._x = (int) value.x;
        this._y = (int) value.y;
        this._z = (int) value.z;
      }
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

    public void Set(IVoxelLocation location)
    {
      this._x = location.X;
      this._y = location.Y;
      this._z = location.Z;
    }
  }
}
