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
  public sealed class VoxelMap : IWritableVoxelMesh
  {
    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private Dimensions3D Dimensions;

    /// <summary>
    ///   Mesh data.
    /// </summary>
    private Color32[, ,] Datas;

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Width
    {
      get
      {
        return this.Dimensions.Width;
      }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Height
    {
      get
      {
        return this.Dimensions.Height;
      }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public uint Depth
    {
      get
      {
        return this.Dimensions.Depth;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[uint x, uint y, uint z] 
    {
      get
      {
        Color32 copy = this.Datas[x, y, z];
        return copy;
      }
      set
      {
        this.Datas[x, y, z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[int x, int y, int z]
    {
      get
      {
        Color32 copy = this.Datas[x, y, z];
        return copy;
      }
      set
      {
        this.Datas[x, y, z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[Vector3 location]
    {
      get
      {
        Color32 copy = this.Datas[(int)location.x, (int)location.y, (int)location.z];
        return copy;
      }
      set
      {
        this.Datas[(int)location.x, (int)location.y, (int)location.z] = value;
      }
    }

    /// <summary>
    ///   Create an empty voxel mesh.
    /// </summary>
    public VoxelMap()
    {
      this.Dimensions = new Dimensions3D();
      this.Datas = new Color32[0, 0, 0];
    }

    /// <summary>
    ///   Create a custom voxel mesh.
    /// </summary>
    /// 
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public VoxelMap(uint width, uint height, uint depth)
    {
      this.Dimensions = new Dimensions3D(width, height, depth);
      this.Datas = new Color32[width, height, depth];
      this.Clear();
    }

    /// <summary>
    ///   Create a custom voxel mesh.
    /// </summary>
    /// 
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public VoxelMap(int width, int height, int depth)
    {
      this.Dimensions = new Dimensions3D((uint)width, (uint)height, (uint)depth);
      this.Datas = new Color32[width, height, depth];
      this.Clear();
    }

    /// Create a custom voxel mesh.
    /// 
    /// <param name="dimensions"></param>
    public VoxelMap(IDimensions3D dimensions)
    {
      this.Dimensions = new Dimensions3D(dimensions);
      this.Datas = new Color32[
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
    public VoxelMap(IVoxelMesh toCopy)
    {
      this.Dimensions = new Dimensions3D(toCopy);
      this.Datas = new Color32[
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
            this.Datas[x, y, z] = toCopy[x, y, z];
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
            this.Datas[x, y, z] = empty;
          }
        }
      }
    }
  }
}
