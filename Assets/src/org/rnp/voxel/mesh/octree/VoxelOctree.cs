using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using rnp.voxel.mesh.octree;
using UnityEngine;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A voxel mesh that store data in an octree.
  /// </summary>
  public sealed class VoxelOctree : AbstractWritableVoxelMesh
  {
    private readonly IWritableVoxelMesh[,,] _childs;
    private readonly VoxelOctreeFormat _dimensions;
    private readonly IVoxelOctreeNodeBuilder _builder;

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override int Width
    {
      get { return _dimensions.Width; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override int Height
    {
      get { return _dimensions.Height; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override int Depth
    {
      get { return _dimensions.Depth; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override Color32 this[int x, int y, int z]
    {
      get { return this.Get(x, y, z); }
      set { this.Set(x, y, z, value); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation Start { get { return VoxelLocation.Zero; } }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation End
    {
      get
      {
        return new VoxelLocation(
           this.Width,
           this.Height,
           this.Depth
        );
      }
    }

    /// <summary>
    ///   An empty voxel octree.
    /// </summary>
    public VoxelOctree() : base()
    {
      this._childs = null;
      this._dimensions = VoxelOctreeFormat.Small;
      this._builder = null;
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="format"></param>
    public VoxelOctree(VoxelOctreeFormat format) : base()
    {
      this._childs = new IWritableVoxelMesh[2,2,2];
      this._dimensions = format;
      this._builder = new BaseVoxelOctreeNodeBuilder();
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="builder"></param>
    public VoxelOctree(VoxelOctreeFormat format, IVoxelOctreeNodeBuilder builder)
      : base()
    {
      this._childs = new IWritableVoxelMesh[2, 2, 2];
      this._dimensions = format;
      this._builder = builder.Copy();
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelOctree(IVoxelMesh toCopy) : base()
    {
      this._childs = new IWritableVoxelMesh[2, 2, 2];
      this._dimensions = VoxelOctreeFormat.GetFormat(toCopy.Width, toCopy.Height, toCopy.Depth);
      this._builder = new BaseVoxelOctreeNodeBuilder();

      IVoxelLocation end = toCopy.End;
      IVoxelLocation start = toCopy.Start;
      for (int i = start.X; i < end.X; ++i)
      {
        for (int j = start.Y; j < end.Y; ++j)
        {
          for (int k = start.Z; k < end.Z; ++k)
          {
            this[i - start.X, j - start.Y, k - start.Z] = toCopy[i, j, k];
          }
        }
      }
    }

    /// <summary>
    ///   Copy an existing octree.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelOctree(VoxelOctree toCopy) : base()
    {
      this._childs = new IWritableVoxelMesh[2, 2, 2];
      this._dimensions = toCopy._dimensions;
      this._builder = toCopy._builder.Copy();
      for (int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          for (int k = 0; k < this.Depth; ++k)
          {
            this[i, j, k] = toCopy[i, j, k];
          }
        }
      }
    }

    /// <summary>
    ///   Return a child node. An octree has 8 childs, each
    /// child is in a 2x2x2 cube.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public IWritableVoxelMesh GetChild(int x, int y, int z)
    {
      return this._childs[x, y, z];
    }
    
    /// <summary>
    ///   Get a voxel from the octree.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Color32 Get(int x, int y, int z)
    {
      int lx = x / this._dimensions.ChildWidth;
      int ly = y / this._dimensions.ChildHeight;
      int lz = z / this._dimensions.ChildDepth;
      IWritableVoxelMesh child = this.GetChild(lx, ly, lz);

      if (child != null)
      {
        return child[
          x - lx * this._dimensions.ChildWidth, 
          y - ly * this._dimensions.ChildHeight, 
          z - lz * this._dimensions.ChildDepth
        ];
      }
      else
      {
        return Voxels.Empty;
      }
    }

    /// <summary>
    ///   Return an octree node at a specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private IWritableVoxelMesh GetChildOrCreate(int x, int y, int z)
    {
      if (this._childs[x, y, z] == null)
      {
        this._childs[x, y, z] = this._builder.Build(
          this._dimensions.ChildWidth, 
          this._dimensions.ChildHeight,
          this._dimensions.ChildDepth
        );
      }

      return this._childs[x, y, z];
    }

    /// <summary>
    ///   Set a voxel in the octree.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="color"></param>
    public void Set(int x, int y, int z, Color32 color)
    {
      int lx = (int)(x / this._dimensions.ChildWidth);
      int ly = (int)(y / this._dimensions.ChildHeight);
      int lz = (int)(z / this._dimensions.ChildDepth);
      
      if (color.a == 0)
      {
        this.GetChildOrCreate(lx, ly, lz)[
          x - lx * this._dimensions.ChildWidth,
          y - ly * this._dimensions.ChildHeight,
          z - lz * this._dimensions.ChildDepth
        ] = color;
      }
      else if(this.GetChild(lx, ly, lz) != null)
      {
        this.GetChild(lx, ly, lz)[
          x - lx * this._dimensions.ChildWidth,
          y - ly * this._dimensions.ChildHeight,
          z - lz * this._dimensions.ChildDepth
        ] = color;

        if (this.GetChild(lx, ly, lz).IsEmpty())
        {
          this._childs[lx, ly, lz] = null;
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override void Clear()
    {
      for (int i = 0; i < 2; ++i)
      {
        for (int j = 0; j < 2; ++j)
        {
          for (int k = 0; k < 2; ++k)
          {
            this._childs[i, j, k] = null;
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override bool IsEmpty()
    {
      for (int i = 0; i < 2; ++i)
      {
        for (int j = 0; j < 2; ++j)
        {
          for (int k = 0; k < 2; ++k)
          {
            if (this._childs[i, j, k] != null && !this._childs[i, j, k].IsEmpty())
            {
              return false;
            }
          }
        }
      }

      return true;
    }
  }
}
