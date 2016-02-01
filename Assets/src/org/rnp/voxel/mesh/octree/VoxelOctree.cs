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
    private readonly static Color32 EMPTY = new Color32(0,0,0,255);

    private IWritableVoxelMesh[] _childs;
    private Dimensions3D _dimensions;
    private Dimensions3D _midDimensions;
    private IVoxelOctreeNodeBuilder _builder;

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

    /// <summary>
    ///   An empty voxel octree.
    /// </summary>
    public VoxelOctree() : base()
    {
      this._childs = null;
      this._dimensions = new Dimensions3D(0, 0, 0);
      this._midDimensions = new Dimensions3D(0, 0, 0);
      this._builder = null;
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public VoxelOctree(int width, int height, int depth) : base()
    {
      this._childs = new IWritableVoxelMesh[8];
      this._dimensions = new Dimensions3D(width, height, depth);
      this._midDimensions = new Dimensions3D(width/2, height/2, depth/2);
      this._builder = new BaseVoxelOctreeNodeBuilder();
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="builder"></param>
    public VoxelOctree(int width, int height, int depth, IVoxelOctreeNodeBuilder builder)
      : base()
    {
      this._childs = new IWritableVoxelMesh[8];
      this._dimensions = new Dimensions3D(width, height, depth);
      this._midDimensions = new Dimensions3D(width / 2, height / 2, depth / 2);
      this._builder = builder.Copy();
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelOctree(IVoxelMesh toCopy) : base()
    {
      this._childs = new IWritableVoxelMesh[8];
      this._dimensions = new Dimensions3D(toCopy);
      this._midDimensions = new Dimensions3D(this.Width / 2, this.Height / 2, this.Depth / 2);
      this._builder = new BaseVoxelOctreeNodeBuilder();
    }

    /// <summary>
    ///   Copy an existing octree.
    /// </summary>
    /// <param name="toCopy"></param>
    public VoxelOctree(VoxelOctree toCopy) : base()
    {
      this._childs = new IWritableVoxelMesh[8];
      this._dimensions = new Dimensions3D(toCopy);
      this._midDimensions = new Dimensions3D(toCopy._midDimensions);
      this._builder = toCopy._builder.Copy();
    }

    /// <summary>
    ///   Return a child node.
    /// </summary>
    /// <param name="indx"></param>
    /// <returns></returns>
    public IWritableVoxelMesh GetChild(int indx)
    {
      return this._childs[indx];
    }

    /// <summary>
    ///   Return a child index from a relative location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int GetChildIndex(int x, int y, int z)
    {
      int lx = (int)(x / this._midDimensions.Width);
      int ly = (int)(y / this._midDimensions.Height);
      int lz = (int)(z / this._midDimensions.Depth);

      return lx + ly * 2 + lz * 4;
    }

    /// <summary>
    ///   Return a child index from an absolute location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int GetAbsoluteChildIndex(int x, int y, int z)
    {
      return this.GetChildIndex(x - this.Start.X, y - this.Start.Y, z - this.Start.Z);
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
      IWritableVoxelMesh child = this.GetChild(this.GetChildIndex(x, y, z));
      if (child != null)
      {
        return child.AbsoluteGet(x, y, z);
      }
      else
      {
        return VoxelOctree.EMPTY;
      }
    }

    /// <summary>
    ///   Create a child node.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void CreateChild(int indx, int x, int y, int z)
    {
      int childWidth = this._midDimensions.Width;
      int childHeight = this._midDimensions.Height;
      int childDepth = this._midDimensions.Depth;

      if (x == 1) childWidth = this.Width - childWidth;
      if (y == 1) childHeight = this.Height - childHeight;
      if (z == 1) childDepth = this.Depth - childDepth;

      this._childs[indx] = this._builder.Build(childWidth, childHeight, childDepth);

      if (x == 1) this._childs[indx].Start.X = this._midDimensions.Width;
      if (y == 1) this._childs[indx].Start.Y = this._midDimensions.Height;
      if (z == 1) this._childs[indx].Start.Z = this._midDimensions.Depth;
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
      int lx = (int)(x / this._midDimensions.Width);
      int ly = (int)(y / this._midDimensions.Height);
      int lz = (int)(z / this._midDimensions.Depth);

      int indx = lx + ly * 2 + lz * 4;

      if (this._childs[indx] == null)
      {
        this.CreateChild(indx, lx, ly, lz);
      }

      return this._childs[indx];
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
      this.GetChildOrCreate(x, y, z).AbsoluteSet(x, y, z, color);
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override void Clear()
    {
      for (int i = 0; i < this._childs.Length; ++i) this._childs[i] = null;
    }
  }
}
