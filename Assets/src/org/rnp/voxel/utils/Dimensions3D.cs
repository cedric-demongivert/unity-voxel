using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A simple struct for 3D voxel cubes dimensions.
  /// </summary>
  public struct Dimensions3D : IDimensions3D
  {
    private int _width;
    private int _height;
    private int _depth;

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Width
    {
      get { return _width; }
      set { _width = value; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Height
    {
      get { return _height; }
      set { _height = value; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Depth
    {
      get { return _depth; }
      set { _depth = value; }
    }

    /// <summary>
    ///   Create a custom Dimensions3D struct.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public Dimensions3D(int width, int height, int depth)
    {
      this._width = width;
      this._height = height;
      this._depth = depth;
    }

    /// <summary>
    ///   Create a copy of an existing IDimensions3D element.
    /// </summary>
    /// <param name="toCopy"></param>
    public Dimensions3D(IDimensions3D toCopy)
    {
      this._width = toCopy.Width;
      this._height = toCopy.Height;
      this._depth = toCopy.Depth;
    }

    /// <summary>
    ///   Return true if any dimension is null.
    /// </summary>
    /// <returns></returns>
    public bool HasNull()
    {
      return this._width == 0 || this._height == 0 || this._depth == 0;
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public IDimensions3D Set(int x, int y, int z)
    {
      this._width = x;
      this._height = y;
      this._depth = z;
      return this;
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public IDimensions3D Set(IDimensions3D other)
    {
      this._width = other.Width;
      this._height = other.Height;
      this._depth = other.Depth;
      return this;
    }

    /// <summary>
    ///   Add a vector3
    /// </summary>
    /// <param name="toAdd"></param>
    public IDimensions3D Add(Vector3 toAdd)
    {
      this._width += (int) toAdd.x;
      this._height += (int) toAdd.y;
      this._depth += (int) toAdd.z;
      return this;
    }

    /// <summary>
    ///   Add another dimension 3D
    /// </summary>
    /// <param name="other"></param>
    public IDimensions3D Add(IDimensions3D other)
    {
      this._width += other.Width;
      this._height += other.Height;
      this._depth += other.Depth;
      return this;
    }

    /// <summary>
    ///   Subtract a vector3
    /// </summary>
    /// <param name="toSub"></param>
    public IDimensions3D Sub(Vector3 toSub)
    {
      this._width -= (int) toSub.x;
      this._height -= (int) toSub.y;
      this._depth -= (int) toSub.z;
      return this;
    }

    /// <summary>
    ///   Subtract another dimension 3D
    /// </summary>
    /// <param name="other"></param>
    public IDimensions3D Sub(IDimensions3D other)
    {
      this._width -= other.Width;
      this._height -= other.Height;
      this._depth -= other.Depth;
      return this;
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public bool IsEmpty()
    {
      return this._width == 0 && this._height == 0 && this._depth == 0;
    }

    /// <see cref="object"/>
    public override string ToString()
    {
      return "Dimensions3D (" + this._width + ", " + this._height + ", " + this._depth + ")";
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"></see>
    public IDimensions3D Copy()
    {
      return new Dimensions3D(this);
    }
  }
}
