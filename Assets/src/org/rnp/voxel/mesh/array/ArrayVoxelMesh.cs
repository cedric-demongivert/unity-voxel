using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.array
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A voxel mesh implementation that store data in a buffer.
  /// </summary>
  public sealed class ArrayVoxelMesh : AbstractVoxelMesh
  {
    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private Dimensions3D _dimensions;
    
    /// <summary>
    ///   Mesh data.
    /// </summary>
    private Color32[, ,] _datas;
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Dimensions3D Dimensions
    {
      get
      {
        return this._dimensions;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation Start
    {
      get 
      {
        return VoxelLocation.Zero;
      }
    }

    /// <summary>
    ///   Create an empty voxel mesh.
    /// </summary>
    public ArrayVoxelMesh() : base()
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
    public ArrayVoxelMesh(int width, int height, int depth) : base()
    {
      this._dimensions = new Dimensions3D(width, height, depth);
      this._datas = new Color32[width, height, depth];
      this.Clear();
    }

    /// Create a custom voxel mesh.
    /// 
    /// <param name="dimensions"></param>
    public ArrayVoxelMesh(Dimensions3D dimensions) : base()
    {
      this._dimensions = new Dimensions3D(dimensions);
      this._datas = new Color32[
        dimensions.Width,
        dimensions.Height,
        dimensions.Depth
      ];
      this.Clear();
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public ArrayVoxelMesh(IVoxelMesh toCopy) : base()
    {
      this._dimensions = new Dimensions3D(toCopy.Dimensions);
      this._datas = new Color32[
        this._dimensions.Width,
        this._dimensions.Height,
        this._dimensions.Depth
      ];
      VoxelMeshes.Copy(toCopy, this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 value)
    {
      this._datas[location.X, location.Y, location.Z] = value;
      this.Touch();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 value)
    {
      this._datas[x, y, z] = value;
      this.Touch();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      return this._datas[location.X, location.Y, location.Z];
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      return this._datas[x, y, z];
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Clear()
    {
      for (int x = 0; x < this._dimensions.Width; ++x)
      {
        for (int y = 0; y < this._dimensions.Height; ++y)
        {
          for (int z = 0; z < this._dimensions.Depth; ++z)
          {
            this._datas[x, y, z] = Voxels.Empty;
          }
        }
      }

      this.Touch();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      for (int x = 0; x < this._dimensions.Width; ++x)
      {
        for (int y = 0; y < this._dimensions.Height; ++y)
        {
          for (int z = 0; z < this._dimensions.Depth; ++z)
          {
            if (Voxels.IsEmpty(this._datas[x, y, z]))
            {
              return false;
            }
          }
        }
      }

      return true;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsEmpty()
    {
      for (int x = 0; x < this._dimensions.Width; ++x)
      {
        for (int y = 0; y < this._dimensions.Height; ++y)
        {
          for (int z = 0; z < this._dimensions.Depth; ++z)
          {
            if (!Voxels.IsEmpty(this._datas[x, y, z]))
            {
              return false;
            }
          }
        }
      }

      return true;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ArrayVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return new ReadonlyVoxelMesh(this);
    }
  }
}
