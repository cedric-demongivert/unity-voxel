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
  ///   Wrap entirely an existing mesh.
  /// </summary>
  public class ProxyMesh : AbstractVoxelMesh, ISubMesh
  {
    /// <summary>
    ///   ReadOnly implementation.
    /// </summary>
    private ReadOnlySubMesh _readOnly;

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
      get { return this._parentMesh[x, y, z]; }
      set { this._parentMesh[x, y, z] = value; }
    }

    /// <see cref="org.rnp.voxel.mesh.submesh.ISubMesh"></see>
    public IVoxelLocation Offset
    {
      get { return VoxelLocation.Zero; }
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
        return this._parentMesh.End;
      }
    }

    /// <summary>
    ///   Wrap an existing mesh.
    /// </summary>
    /// <param name="toWrap"></param>
    public ProxyMesh(IVoxelMesh toWrap) : base()
    {
      this._parentMesh = toWrap;
    }

    /// <summary>
    ///   Copy an existing voxel mesh.
    /// </summary>
    /// <param name="toCopy"></param>
    public ProxyMesh(ProxyMesh toCopy)
      : base()
    {
      this._parentMesh = toCopy.ParentMesh;
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
      return new ProxyMesh(this);
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
