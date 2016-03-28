using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
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
    private IVoxelMesh[, ,] _childs;

    /// <summary>
    ///   Format of the octree. Store valid octree dimensions.
    /// </summary>
    private OctreeVoxelMeshFormat _format;
    
    /// <summary>
    ///   Start location of that octree.
    /// </summary>
    private VoxelLocation _start;

    /// <summary>
    ///   A builder that create octree nodes.
    /// </summary>
    private readonly IOctreeNodeBuilder _builder;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Dimensions3D Dimensions
    {
      get {
        return this._format.Dimensions;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public Dimensions3D ChildDimensions
    {
      get
      {
        return this._format.ChildFormat.Dimensions;
      }
    }
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation Start { 
      get {
        return VoxelLocation.Zero; 
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
      this._format = OctreeVoxelMeshFormat.GetFormat(toCopy.Dimensions);
      this._builder = new OctreeNodeBuilder();

      VoxelMeshes.Copy(toCopy, this);
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
      
      VoxelMeshes.Copy(toCopy, this);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public VoxelLocation ToChildCoordinates(VoxelLocation location)
    {
      return location.Div(this._format.ChildFormat.Dimensions);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public VoxelLocation ToLocaleCoordinates(VoxelLocation location)
    {
      return location.Mod(this._format.ChildFormat.Dimensions);
    }

    /// <see cref="org.rnp.voxel.mesh.octree.IOctreeVoxelMesh"></see>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      if (this._childs[x, y, z] != null)
      {
        return this._childs[x, y, z].ReadOnly();
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    ///   Same as GetChildAt but return an updatable instance.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private IVoxelMesh GetChildAt(VoxelLocation location)
    {
      VoxelLocation childLocation = this.ToChildCoordinates(location);
      return this._childs[childLocation.X, childLocation.Y, childLocation.Z];
    }

    /// <summary>
    ///   Create a subnode (if not exist) and return it.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private IVoxelMesh CreateChildAt(VoxelLocation location)
    {
      VoxelLocation childLocation = this.ToChildCoordinates(location);

      if (this._childs[childLocation.X, childLocation.Y, childLocation.Z] == null)
      {
        this._builder.Format = this._format.ChildFormat;
        this._childs[childLocation.X, childLocation.Y, childLocation.Z] = this._builder.Build();
      }

      return this._childs[childLocation.X, childLocation.Y, childLocation.Z];
    }

    /// <summary>
    ///   Destroy a child node if exist.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void DestroyChildAt(VoxelLocation location)
    {
      VoxelLocation childLocation = this.ToChildCoordinates(location);

      this._childs[childLocation.X, childLocation.Y, childLocation.Z] = null;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Color32 Get(int x, int y, int z)
    {
      return this.Get(new VoxelLocation(x, y, z));
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override Color32 Get(VoxelLocation location) {
      IVoxelMesh child = this.GetChildAt(location);
      if (child != null)
      {
        return child[this.ToLocaleCoordinates(location)];
      }
      else
      {
        return Voxels.Empty;
      }
    }
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override void Set(int x, int y, int z, Color32 color)
    {
      this.Set(new VoxelLocation(x, y, z), color);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override void Set(VoxelLocation location, Color32 color)
    {
      VoxelLocation localeCoordinates = this.ToLocaleCoordinates(location);

      if (Voxels.IsNotEmpty(color))
      {
        this.CreateChildAt(location)[localeCoordinates] = color;
        this.Touch();
      }
      else
      {
        IVoxelMesh child = this.GetChildAt(location);
        if (child != null)
        {
          child[localeCoordinates] = color;
          if (child.IsEmpty())
          {
            this.DestroyChildAt(location);
          }
          this.Touch();
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
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
      return new ReadonlyOctreeVoxelMesh(this);
    }
  }
}