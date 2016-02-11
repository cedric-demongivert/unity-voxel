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
  public sealed class ArrayVoxelMesh : AbstractVoxelMesh, IArrayVoxelMesh
  {
    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private Dimensions3D _dimensions;

    /// <summary>
    ///   ReadOnly implementation.
    /// </summary>
    private ReadonlyArrayVoxelMesh _readOnly;

    /// <summary>
    ///   Mesh data.
    /// </summary>
    private readonly Color32[, ,] _datas;

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Width
    {
      get { return this._dimensions.Width; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Height
    {
      get { return this._dimensions.Height; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Depth
    {
      get { return this._dimensions.Depth; }
    } 

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override Color32 this[int x, int y, int z]
    {
      get { return this._datas[x, y, z]; }
      set { this._datas[x, y, z] = value; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation Start
    {
      get { return VoxelLocation.Zero; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation End
    {
      get
      {
        return new VoxelLocation(this.Width, this.Height, this.Depth);
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
    public ArrayVoxelMesh(IDimensions3D dimensions) : base()
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
    public ArrayVoxelMesh(IVoxelMesh toCopy) : base()
    {
      this._dimensions = new Dimensions3D(toCopy.Width, toCopy.Height, toCopy.Depth);
      this._datas = new Color32[
        this.Width,
        this.Height,
        this.Depth
      ];
      this.Copy(toCopy.Start, toCopy.End, VoxelLocation.Zero, toCopy);
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public override void Clear()
    {
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            this._datas[x, y, z] = Voxels.Empty;
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsEmpty()
    {
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            if (this._datas[x, y, z].a != 255)
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
      if (this._readOnly == null)
      {
        this._readOnly = new ReadonlyArrayVoxelMesh(this);
      }
      return this._readOnly;
    }
  }
}
