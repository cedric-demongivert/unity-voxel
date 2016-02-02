using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A simple voxel mesh that store data in an array.
  /// </summary>
  public sealed class VoxelArray : IWritableVoxelMesh
  {
    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private Dimensions3D _dimensions;

    /// <summary>
    ///   Mesh data.
    /// </summary>
    private readonly Color32[, ,] _datas;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public Vector3 Start
    {
      get { return Vector3.zero; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Width
    {
      get { return this._dimensions.Width; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Height
    {
      get { return this._dimensions.Height; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public int Depth
    {
      get { return this._dimensions.Depth; }
    } 

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[uint x, uint y, uint z] 
    {
      get
      {
        Color32 copy = this._datas[x, y, z];
        return copy;
      }
      set
      {
        this._datas[x, y, z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[int x, int y, int z]
    {
      get
      {
        Color32 copy = this._datas[x, y, z];
        return copy;
      }
      set
      {
        this._datas[x, y, z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[Vector3 location]
    {
      get
      {
        Color32 copy = this._datas[(int)location.x, (int)location.y, (int)location.z];
        return copy;
      }
      set
      {
        this._datas[(int)location.x, (int)location.y, (int)location.z] = value;
      }
    }

    /// <summary>
    ///   Create an empty voxel mesh.
    /// </summary>
    public VoxelArray()
    {
      this._dimensions = new Dimensions3D();
      this._datas = new Color32[0, 0, 0];
    }

    /// <summary>
    ///   Create a custom voxel mesh.
    /// </summary>
    /// 
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public VoxelArray(int width, int height, int depth)
    {
      this._dimensions = new Dimensions3D(width, height, depth);
      this._datas = new Color32[width, height, depth];
      this.Clear();
    }

    /// Create a custom voxel mesh.
    /// 
    /// <param name="dimensions"></param>
    public VoxelArray(IDimensions3D dimensions)
    {
      this._dimensions = new Dimensions3D(dimensions);
      this._datas = new Color32[
        this.Width, 
        this.Height, 
        this.Depth
      ];
      this.Clear();
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelArray(IVoxelMesh toCopy)
    {
      this._dimensions = new Dimensions3D(toCopy);
      this._datas = new Color32[
        this.Width,
        this.Height,
        this.Depth
      ];

      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            this._datas[x, y, z] = toCopy[x, y, z];
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public void Clear()
    {
      Color32 empty = new Color32(0, 0, 0, 255);
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            this._datas[x, y, z] = empty;
          }
        }
      }
    }
  }
}
