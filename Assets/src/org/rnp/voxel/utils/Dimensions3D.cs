using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// 
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
    /// 
    /// <param name="toCopy"></param>
    public Dimensions3D(IDimensions3D toCopy)
    {
      this._width = toCopy.Width;
      this._height = toCopy.Height;
      this._depth = toCopy.Depth;
    }

    /// <summary>
    ///   Return true if the dimensions is null (0,0,0)
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
      return this._width == 0 && this._height == 0 && this._depth == 0;
    }
  }
}
