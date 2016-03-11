using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.builder;
using UnityEngine;

namespace org.rnp.voxel.mesh.octree
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A voxel mesh that store data in an octree.
  /// </summary>
  /// 
  /// <see cref="https://en.wikipedia.org/wiki/Octree"/>
  public sealed class OctreeVoxelMesh : AbstractVoxelMesh, IOctreeVoxelMesh
  {
    /// <summary>
    ///   Octree childs.
    /// </summary>
    private readonly IVoxelMesh[, ,] _childs;

    /// <summary>
    ///   Format of the octree. Store valid octree dimensions.
    /// </summary>
    private readonly OctreeVoxelMeshFormat _format;

    /// <summary>
    ///   Readonly implementation.
    /// </summary>
    private ReadonlyOctreeVoxelMesh _readOnly;

    /// <summary>
    ///   A builder that create octree nodes.
    /// </summary>
    private readonly IOctreeNodeBuilder _builder;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override int Width
    {
      get { return _format.Width; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override int Height
    {
      get { return _format.Height; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override int Depth
    {
      get { return _format.Depth; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Color32 this[int x, int y, int z]
    {
      get { return this.Get(x, y, z); }
      set { this.Set(x, y, z, value); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation Start { get { return VoxelLocation.Zero; } }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation End
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
    public OctreeVoxelMesh()
      : base()
    {
      this._childs = new IVoxelMesh[2, 2, 2];
      this._format = OctreeVoxelMeshFormat.Empty;
      this._builder = new OctreeNodeBuilder();
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="format"></param>
    public OctreeVoxelMesh(OctreeVoxelMeshFormat format)
      : base()
    {
      this._childs = new IVoxelMesh[2, 2, 2];
      this._format = format;
      this._builder = new OctreeNodeBuilder();
    }

    /// <summary>
    ///   A custom voxel octree.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="builder"></param>
    public OctreeVoxelMesh(OctreeVoxelMeshFormat format, IOctreeNodeBuilder builder)
      : base()
    {
      this._childs = new IVoxelMesh[2, 2, 2];
      this._format = format;
      this._builder = builder;
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public OctreeVoxelMesh(IVoxelMesh toCopy)
      : base()
    {
      this._childs = new IVoxelMesh[2, 2, 2];
      this._format = OctreeVoxelMeshFormat.GetFormat(toCopy.Width, toCopy.Height, toCopy.Depth);
      this._builder = new OctreeNodeBuilder();

      this.Copy(toCopy.Start, toCopy.End, VoxelLocation.Zero, toCopy);
    }

    /// <summary>
    ///   Copy an existing octree.
    /// </summary>
    /// <param name="toCopy"></param>
    public OctreeVoxelMesh(OctreeVoxelMesh toCopy)
      : base()
    {
      this._childs = new IVoxelMesh[2, 2, 2];
      this._format = toCopy._format;
      this._builder = (IOctreeNodeBuilder) toCopy._builder.Copy();

      this.Copy(toCopy.Start, toCopy.End, VoxelLocation.Zero, toCopy);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      if (this._childs[x, y, z] == null)
      {
        return null;
      }
      else
      {
        return this._childs[x, y, z].ReadOnly();
      }
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
      int lx = x / this._format.ChildFormat.Width;
      int ly = y / this._format.ChildFormat.Height;
      int lz = z / this._format.ChildFormat.Depth;
      IVoxelMesh child = this.GetChild(lx, ly, lz);

      if (child != null)
      {
        return child[
          x - lx * this._format.ChildFormat.Width,
          y - ly * this._format.ChildFormat.Height,
          z - lz * this._format.ChildFormat.Depth
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
    private IVoxelMesh GetChildOrCreate(int x, int y, int z)
    {
      if (this._childs[x, y, z] == null)
      {
        this._builder.Format = this._format.ChildFormat;
        this._childs[x, y, z] = this._builder.Build();
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
      int lx = (int)(x / this._format.ChildFormat.Width);
      int ly = (int)(y / this._format.ChildFormat.Height);
      int lz = (int)(z / this._format.ChildFormat.Depth);

      if (color.a == 0)
      {
        this.GetChildOrCreate(lx, ly, lz)[
          x - lx * this._format.ChildFormat.Width,
          y - ly * this._format.ChildFormat.Height,
          z - lz * this._format.ChildFormat.Depth
        ] = color;
      }
      else if (this._childs[lx, ly, lz] != null)
      {
        this._childs[lx, ly, lz][
          x - lx * this._format.ChildFormat.Width,
          y - ly * this._format.ChildFormat.Height,
          z - lz * this._format.ChildFormat.Depth
        ] = color;

        if (this._childs[lx, ly, lz].IsEmpty())
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
    public override bool IsFull()
    {
      for (int i = 0; i < 2; ++i)
      {
        for (int j = 0; j < 2; ++j)
        {
          for (int k = 0; k < 2; ++k)
          {
            if (this._childs[i, j, k] == null || !this._childs[i, j, k].IsFull())
            {
              return false;
            }
          }
        }
      }

      return true;
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
            if (this._childs[i, j, k] != null)
            {
              return false;
            }
          }
        }
      }

      return true;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelMesh Copy()
    {
      return new OctreeVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      if (this._readOnly == null)
      {
        this._readOnly = new ReadonlyOctreeVoxelMesh(this);
      }

      return this._readOnly;
    }
  }
}