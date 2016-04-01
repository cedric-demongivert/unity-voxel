using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using UnityEngine;
using org.rnp.voxel.mesh.exceptions;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///     A sphere of voxels.
  /// </summary> 
  [CreateAssetMenu(menuName="Voxel/Sphere")]
  public class SphereVoxelMesh : VoxelMesh
  {
    [SerializeField]
    private int _radius;

    [SerializeField]
    private Color32 _color;

    public int Radius
    {
      get { return this._radius; }
      set
      {
        if(this._radius != value)
        {
          this._radius = value;
          this.MarkDirty();
        }
      }
    }

    public Color32 Color
    {
      get { return this._color; }
      set
      {
        if(!this._color.Equals(value))
        {
          this._color = value;
          this.MarkDirty();
        }
      }
    }

    /// <summary>
    ///   A new empty sphere voxel mesh.
    /// </summary>
    public SphereVoxelMesh()
    {
      this._radius = 0;
      this._color = Voxels.Empty;
    }

    /// <summary>
    ///   A  sphere voxel mesh.
    /// </summary>
    public SphereVoxelMesh(int radius, Color32 color)
    {
      this._radius = radius;
      this._color = color;
    }

    /// <summary>
    ///   Copy a sphere voxel mesh.
    /// </summary>
    public SphereVoxelMesh(SphereVoxelMesh toCopy)
    {
      this._radius = toCopy._radius;
      this._color = toCopy._color;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override Dimensions3D Dimensions
    {
      get
      {
        return new Dimensions3D(this._radius * 2);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelLocation Start
    {
      get
      {
        return new VoxelLocation(-this._radius);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsReadonly
    {
      get
      {
        return false;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelMesh Readonly()
    {
      return new ReadonlyVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override VoxelMesh Copy()
    {
      return new SphereVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override void Set(int x, int y, int z, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this, "A sphere voxel mesh can't be directly modified.");
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override void Set(VoxelLocation location, Color32 value)
    {
      throw new UnmodifiableVoxelMeshException(this, "A sphere voxel mesh can't be directly modified.");
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override Color32 Get(int x, int y, int z)
    {
      if(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f).sqrMagnitude <= _radius * _radius)
      {
        return this._color;
      }
      else
      {
        return Voxels.Empty;
      }
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override void Clear()
    {
      this._radius = 0;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override bool IsEmpty()
    {
      return this._radius == 0 || Voxels.IsEmpty(this._color);
    }

    /// <summary>
    ///   A sphere block can't be full.
    /// </summary>
    /// <returns></returns>
    public override bool IsFull()
    {
      return false;
    }

    /// <see cref="org.rnp.voxel.mesh.VoxelMesh"></see>
    public override Color32 Get(VoxelLocation location)
    {
      if (new Vector3(location.X + 0.5f, location.Y + 0.5f, location.Z + 0.5f).sqrMagnitude <= _radius * _radius)
      {
        return this._color;
      }
      else
      {
        return Voxels.Empty;
      }
    }
  }
}
