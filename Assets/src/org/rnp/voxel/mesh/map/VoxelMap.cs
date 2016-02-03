using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh;
using UnityEngine;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An infinite voxel mesh.
  /// </summary>
  public sealed class VoxelMap : AbstractVoxelMesh
  {
    private Dictionary<IVoxelLocation, IVoxelMesh> _parts;
    private Dimensions3D _dimensions;
    private VoxelLocation _min;

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation Start
    {
      get { return new VoxelLocation(_min); }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"></see>
    public override IVoxelLocation End
    {
      get { 
        return new VoxelLocation(
          this._min.X + _dimensions.Width,
          this._min.Y + _dimensions.Height,
          this._min.Z + _dimensions.Depth
        );
      }
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

    public override Color32 this[int x, int y, int z]
    {
      get
      {
        return this.Get(x,y,z);
      }
      set 
      {
        this.Set(x,y,z,value);
      }
    }

    public VoxelMap()
    {
      this._parts = new Dictionary<IVoxelLocation, IVoxelMesh>();
      this._dimensions = new Dimensions3D();
      this._min = new VoxelLocation();
    }

    public Color32 Get(int x, int y, int z)
    {
      return Color.black;
    }

    public void Set(int x, int y, int z, Color32 value)
    {

    }

    public override Boolean IsEmpty()
    {
      return false;
    }

    public override void Clear()
    {

    }

    public override IVoxelMesh Copy()
    {
      throw new NotImplementedException();
    }

    public override IReadonlyVoxelMesh ReadOnly()
    {
      throw new NotImplementedException();
    }
  }
}
