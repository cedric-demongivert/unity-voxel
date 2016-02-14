using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh.absolute
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple implementation.
  /// </summary>
  public class AbsoluteVoxelMesh : AbstractVoxelMesh, IAbsoluteVoxelMesh
  {
    /// <summary>
    ///   ReadOnly implementation.
    /// </summary>
    private ReadOnlyAbsoluteMesh _readOnly;
    
    /// <summary>
    ///   Start point.
    /// </summary>
    private IVoxelLocation _start;

    /// <summary>
    ///   Parent mesh.
    /// </summary>
    private IVoxelMesh _parentMesh;
    
    /// <see cref="org.rnp.voxel.mesh.absolute.IAbsoluteVoxelMesh"></see>
    public IVoxelMesh RelativeMesh
    {
      get { return this._parentMesh; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Width
    {
      get { return this._parentMesh.Width; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Height
    {
      get { return this._parentMesh.Height; }
    }

    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override int Depth
    {
      get { return this._parentMesh.Depth; }
    } 

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"></see>
    public override Color32 this[int x, int y, int z]
    {
      get {
        return this._parentMesh[x - this._start.X, y - this._start.Y, z - this._start.Z];
      }
      set {
        this._parentMesh[x - this._start.X, y - this._start.Y, z - this._start.Z] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation Start
    {
      get { return new VoxelLocation(this._start); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation End
    {
      get
      {
        return new VoxelLocation(this.Width, this.Height, this.Depth).Add(this._start);
      }
    }

    /// <summary>
    ///   Locate a voxel mesh.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="start"></param>
    public AbsoluteVoxelMesh(IVoxelMesh parent, IVoxelLocation start) : base()
    {
      this._parentMesh = parent;
      this._start = new VoxelLocation(start);
    }

    /// <summary>
    ///   Locate a voxel mesh.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public AbsoluteVoxelMesh(IVoxelMesh parent, int x, int y, int z)
      : base()
    {
      this._parentMesh = parent;
      this._start = new VoxelLocation(x, y, z);
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public AbsoluteVoxelMesh(IAbsoluteVoxelMesh toCopy)
      : base()
    {
      this._parentMesh = toCopy.RelativeMesh;
      this._start = new VoxelLocation(toCopy.Start);
    }

    /// <see cref="org.rnp.voxel.mesh.IWritableVoxelMesh"/>
    public override void Clear()
    {
      this._parentMesh.Clear();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsFull()
    {
      return this._parentMesh.IsFull();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override bool IsEmpty()
    {
      return this._parentMesh.IsEmpty();
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new AbsoluteVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      if (this._readOnly == null)
      {
        this._readOnly = new ReadOnlyAbsoluteMesh(this);
      }
      return this._readOnly;
    }
  }
}
