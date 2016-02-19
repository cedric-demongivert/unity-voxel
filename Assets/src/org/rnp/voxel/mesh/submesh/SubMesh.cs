using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.submesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple implementation.
  /// </summary>
  public class SubMesh : AbstractVoxelMesh, ISubMesh
  {
    /// <summary>
    ///   ReadOnly implementation.
    /// </summary>
    private ReadOnlySubMesh _readOnly;

    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private IDimensions3D _dimensions;

    /// <summary>
    ///   Start point.
    /// </summary>
    private VoxelLocation _start;

    /// <summary>
    ///   Parent mesh.
    /// </summary>
    private IVoxelMesh _parentMesh;
    
    /// <see cref="org.rnp.voxel.mesh.submesh.ISubMesh"></see>
    public IVoxelMesh ParentMesh
    {
      get { return this._parentMesh; }
    }

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
      get { 
        if(this.Contains(x,y,z))
        {
          return this._parentMesh[this._start.X + x, this._start.Y + y, this._start.Z + z];
        }
        else
        {
          throw new IndexOutOfRangeException();
        }
      }
      set {
        if (this.Contains(x, y, z))
        {
          this._parentMesh[this._start.X + x, this._start.Y + y, this._start.Z + z] = value;
        }
        else
        {
          throw new IndexOutOfRangeException();
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.submesh.ISubMesh"></see>
    public VoxelLocation Offset
    {
      get { return new VoxelLocation(this._start); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation Start
    {
      get { return VoxelLocation.Zero; }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override VoxelLocation End
    {
      get
      {
        return new VoxelLocation(this.Width, this.Height, this.Depth);
      }
    }

    /// <summary>
    ///   Keep a submesh of an existing mesh.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="start"></param>
    /// <param name="dimensions"></param>
    public SubMesh(IVoxelMesh parent, VoxelLocation start, IDimensions3D dimensions) : base()
    {
      this._dimensions = dimensions.Copy();
      this._parentMesh = parent;
      this._start = new VoxelLocation(start);
    }

    /// <summary>
    ///   Create a custom voxel mesh.
    /// </summary>
    /// 
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public SubMesh(IVoxelMesh parent, IDimensions3D dimensions) : base()
    {
      this._dimensions = dimensions.Copy();
      this._parentMesh = parent;
      this._start = VoxelLocation.Zero;
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public SubMesh(ISubMesh toCopy)
      : base()
    {
      this._dimensions = new Dimensions3D(toCopy.Width, toCopy.Height, toCopy.Depth);
      this._parentMesh = toCopy.ParentMesh;
      this._start = new VoxelLocation(toCopy.Offset);
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
            this[x, y, z] = Voxels.Empty;
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            if (this.IsEmpty(x, y, z))
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
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
        {
          for (int z = 0; z < this.Depth; ++z)
          {
            if (!this.IsEmpty(x, y, z))
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
      return new SubMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      if (this._readOnly == null)
      {
        this._readOnly = new ReadOnlySubMesh(this);
      }
      return this._readOnly;
    }
  }
}
