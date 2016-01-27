using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT <cedric.demongivert@gmail.com></author>
  /// 
  /// <summary>
  ///   A simple struct for 3D voxel cubes dimensions.
  /// </summary>
  public struct Dimensions3D : IDimensions3D
  {
    private uint _Width;
    private uint _Height;
    private uint _Depth;

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Width
    {
      get
      {
        return _Width;
      }
      set
      {
        _Width = value;
      }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Height
    {
      get
      {
        return _Height;
      }
      set
      {
        _Height = value;
      }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Depth
    {
      get
      {
        return _Depth;
      }
      set
      {
        _Depth = value;
      }
    }

    /// <summary>
    ///   Create a custom Dimensions3D struct.
    /// </summary>
    /// 
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public Dimensions3D(uint width, uint height, uint depth)
    {
      this.Width = width;
      this.Height = height;
      this.Depth = depth;
    }

    /// <summary>
    ///   Create a copy of an existing IDimensions3D element.
    /// </summary>
    /// 
    /// <param name="toCopy"></param>
    public Dimensions3D(IDimensions3D toCopy) {
      this.Width = toCopy.Width;
      this.Height = toCopy.Height;
      this.Depth = toCopy.Depth;
    }
  }
}
