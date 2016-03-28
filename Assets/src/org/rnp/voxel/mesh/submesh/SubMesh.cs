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
  public class SubMesh : AbstractVoxelMesh
  {
    /// <summary>
    ///   Voxel dimension of the mesh.
    /// </summary>
    private Dimensions3D _dimensions;

    /// <summary>
    ///   Offset from parent starting point.
    /// </summary>
    private VoxelLocation _offset;

    /// <summary>
    ///   Parent mesh.
    /// </summary>
    private IVoxelMesh _parentMesh;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override long LastUpdateTime
    {
      get
      {
        if (base.LastUpdateTime > this._parentMesh.LastUpdateTime)
        {
          return base.LastUpdateTime;
        }
        else
        {
          return this._parentMesh.LastUpdateTime;
        }
      }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
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

    public VoxelLocation Offset
    {
      get { return this._offset; }
    }

    public IVoxelMesh ParentMesh
    {
      get { return this._parentMesh; }
    }

    /// <summary>
    ///   Keep a submesh of an existing mesh.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="start"></param>
    /// <param name="dimensions"></param>
    public SubMesh(IVoxelMesh parent, VoxelLocation start, Dimensions3D dimensions) : base()
    {
      this._dimensions = dimensions;
      this._parentMesh = parent;
      this._offset = start;
    }

    /// <summary>
    ///   Create a mesh that is a part of another mesh.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="dimensions"></param>
    public SubMesh(IVoxelMesh parent, Dimensions3D dimensions) : base()
    {
      this._dimensions = dimensions;
      this._parentMesh = parent;
      this._offset = parent.Start;
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public SubMesh(SubMesh toCopy)
      : base()
    {
      this._dimensions = toCopy.Dimensions;
      this._parentMesh = toCopy.ParentMesh;
      this._offset = toCopy.Offset;
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 value)
    {
      this.Set(new VoxelLocation(x, y, z), value);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 value)
    {
      if (this.Contains(location))
      {
        this._parentMesh.Set(location.Add(this._offset), value);
      }
      else
      {
        throw new IndexOutOfRangeException();
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      return this.Get(new VoxelLocation(x, y, z));
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      if (this.Contains(location))
      {
        return this._parentMesh.Get(location.Add(this._offset));
      }
      else
      {
        throw new IndexOutOfRangeException();
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override void Clear()
    {
      for (int x = 0; x < this.Dimensions.Width; ++x)
      {
        for (int y = 0; y < this.Dimensions.Height; ++y)
        {
          for (int z = 0; z < this.Dimensions.Depth; ++z)
          {
            this[x, y, z] = Voxels.Empty;
          }
        }
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      for (int x = 0; x < this.Dimensions.Width; ++x)
      {
        for (int y = 0; y < this.Dimensions.Height; ++y)
        {
          for (int z = 0; z < this.Dimensions.Depth; ++z)
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
      for (int x = 0; x < this.Dimensions.Width; ++x)
      {
        for (int y = 0; y < this.Dimensions.Height; ++y)
        {
          for (int z = 0; z < this.Dimensions.Depth; ++z)
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
      return new ReadonlyVoxelMesh(this);
    }
  }
}
