using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEngine;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A Chunck of a Chuncked Voxel Mesh, a chunck hold a world location and
  /// can select voxels that is out of its bounds.
  /// </summary>
  public class ChunckVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Chunck location in map
    /// </summary>
    private VoxelLocation _location;

    /// <summary>
    ///   Parent mesh.
    /// </summary>
    private ChunckedVoxelMesh _parentMesh;

    /// <summary>
    ///   Voxel mesh used for storing data.
    /// </summary>
    private VoxelMesh _container;
    
    /// <see cref="org.rnp.voxel.utils.IDimensions3D"></see>
    public override Dimensions3D Dimensions
    {
      get
      {
        return this._parentMesh.ChunckDimensions;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelLocation Start
    {
      get
      {
        return VoxelLocation.Zero;
      }
    }

    public VoxelLocation ChunckLocation
    {
      get { return this._location; }
    }

    public ChunckedVoxelMesh ParentMesh
    {
      get { return this._parentMesh; }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsReadonly
    {
      get { return false; }
    }
    
    /// <summary>
    ///   Create a chunck.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="start"></param>
    /// <param name="dimensions"></param>
    public ChunckVoxelMesh(ChunckedVoxelMesh parent, VoxelLocation location, VoxelMesh container) : base()
    {
      this._parentMesh = parent;
      this._location = location;
      this._container = container;
    }
    
    /// <summary>
    ///   Copy an existing chunck.
    /// </summary>
    /// <param name="toCopy"></param>
    public ChunckVoxelMesh(ChunckVoxelMesh toCopy)
      : base()
    {
      this._parentMesh = toCopy.ParentMesh;
      this._location = toCopy.ChunckLocation;
      this._container = toCopy._container.Copy();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(int x, int y, int z, Color32 value)
    {
      if (this.Contains(x, y, z))
      {
        this._container.Set(x, y, z, value);
        this.MarkDirty();
      }
      else
      {
        this._parentMesh[this._parentMesh.ToWorldLocation(this._location, x, y, z)] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Set(VoxelLocation location, Color32 value)
    {
      if (this.Contains(location))
      {
        this._container.Set(location, value);
        this.MarkDirty();
      }
      else
      {
        this._parentMesh[this._parentMesh.ToWorldLocation(this._location, location)] = value;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(int x, int y, int z)
    {
      if (this.Contains(x,y,z))
      {
        return this._container.Get(new VoxelLocation(x, y, z));
      }
      else
      {
        return this._parentMesh[this._parentMesh.ToWorldLocation(this._location, x, y, z)];
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override Color32 Get(VoxelLocation location)
    {
      if (this.Contains(location))
      {
        return this._container.Get(location);
      }
      else
      {
        return this._parentMesh[this._parentMesh.ToWorldLocation(this._location, location)];
      }
    }
    
    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override void Clear()
    {
      this._container.Clear();
      this.MarkDirty();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsFull()
    {
      return this._container.IsFull();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsEmpty()
    {
      return this._container.IsEmpty();
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsEmpty(int x, int y, int z)
    {
      return Voxels.IsEmpty(this[x, y, z]);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override bool IsEmpty(VoxelLocation location)
    {
      return Voxels.IsEmpty(this[location]);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Copy()
    {
      return new ChunckVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"/>
    public override VoxelMesh Readonly()
    {
      return new ReadonlyVoxelMesh(this);
    }
  }
}
