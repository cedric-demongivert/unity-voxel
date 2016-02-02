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
  public sealed class VoxelOctree : IWritableVoxelMesh
  {
    private readonly static Color32 EMPTY = new Color32(0,0,0,255);

    private IWritableVoxelMesh[] _childs;
    private Dimensions3D _dimensions;
    private Dimensions3D _midDimensions;
    private IVoxelOctreeNodeBuilder _builder;

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Vector3 Start
    {
      get { return Vector3.zero; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public int Width
    {
      get { return _dimensions.Width; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public int Height
    {
      get { return _dimensions.Height; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public int Depth
    {
      get { return _dimensions.Depth; }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[int x, int y, int z]
    {
      get { return this.Get(x, y, z); }
      set { this.Set(x, y, z, value); }
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public Color32 this[Vector3 location]
    {
      get { return this.Get((int) location.x, (int) location.y, (int) location.z); }
      set { this.Set((int)location.x, (int)location.y, (int)location.z, value); }
    }

    /// <summary>
    ///   An empty voxel octree.
    /// </summary>
    public VoxelOctree()
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
    public VoxelOctree(int width, int height, int depth)
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
    public VoxelOctree(IVoxelMesh toCopy)
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
    public VoxelOctree(VoxelOctree toCopy)
    {
      this._childs = new IWritableVoxelMesh[8];
      this._dimensions = new Dimensions3D(toCopy);
      this._midDimensions = new Dimensions3D(toCopy._midDimensions);
      this._builder = toCopy._builder.Copy();
    }

    /// <summary>
    ///   Return true if a child exist at the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public bool ExistChildFor(int x, int y, int z)
    {
      int lx = (int)(x / this._midDimensions.Width);
      int ly = (int)(y / this._midDimensions.Height);
      int lz = (int)(z / this._midDimensions.Depth);

      int indx = lx + ly * 2 + lz * 4;

      return this._childs[indx] != null;
    }

    /// <summary>
    ///   Return an octree node at a specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public IWritableVoxelMesh GetChildFor(int x, int y, int z)
    {
      int lx = (int)(x / this._midDimensions.Width);
      int ly = (int)(y / this._midDimensions.Height);
      int lz = (int)(z / this._midDimensions.Depth);

      int indx = lx + ly*2 + lz*4;

      if (this._childs[indx] == null)
      {
        int childWidth = this._midDimensions.Width;
        int childHeight = this._midDimensions.Height;
        int childDepth = this._midDimensions.Depth;

        if (lx == 1) childWidth = this.Width - childWidth;
        if (ly == 1) childHeight = this.Height - childHeight;
        if (lz == 1) childDepth = this.Depth - childDepth;

        this._childs[indx] = this._builder.Build(childWidth, childHeight, childDepth);
      }

      return this._childs[indx];
    }

    /// <summary>
    ///   Transform the coordinate to a location relative to a child.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public int ToLocalX(int x)
    {
      if (x >= this._midDimensions.Width) return x - this._midDimensions.Width;
      else return x;
    }

    /// <summary>
    ///   Transform the coordinate to a location relative to a child.
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public int ToLocalY(int y)
    {
      if (y >= this._midDimensions.Height) return y - this._midDimensions.Height;
      else return y;
    }

    /// <summary>
    ///   Transform the coordinate to a location relative to a child.
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public int ToLocalZ(int z)
    {
      if (z >= this._midDimensions.Width) return z - this._midDimensions.Depth;
      else return z;
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
      if (this.ExistChildFor(x, y, z))
      {
        return this.GetChildFor(x, y, z)[this.ToLocalX(x), this.ToLocalY(y), this.ToLocalZ(z)];
      }
      else
      {
        return VoxelOctree.EMPTY;
      }
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
      this.GetChildFor(x, y, z)[this.ToLocalX(x), this.ToLocalY(y), this.ToLocalZ(z)] = color;
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public void Clear()
    {
      for (int i = 0; i < this._childs.Length; ++i) this._childs[i] = null;
    }
  }
}
