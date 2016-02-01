using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh.octree;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple, procedural, spheric voxel mesh
  /// </summary>
  [ExecuteInEditMode]
  public sealed class SphereVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Sphere radius in voxels.
    /// </summary>
    [SerializeField]
    private int _Radius;

    /// <summary>
    ///   Sphere color.
    /// </summary>
    [SerializeField]
    private Color32 _Color;

    /// <summary>
    ///   Sphere radius in voxels.
    /// </summary>
    public int Radius {
      get
      {
        return _Radius;
      }
      set
      {
        this._Radius = value;
        this.RefreshMesh();
      }
    }

    /// <summary>
    ///   Sphere color.
    /// </summary>
    public Color32 Color {
      get
      {
        return _Color;
      }
      set
      {
        this._Color = value;
        this.RefreshMesh();
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public override void Awake()
    {
      this.RefreshMesh();
    }

    private Vector3 GetPoint(int x, int y, int z)
    {
      Vector3 point = new Vector3(
        x - this.Radius,
        y - this.Radius,
        z - this.Radius
      );

      if (point.x < 0) point.x += 0.5f;
      else point.x += 0.5f;

      if (point.y < 0) point.y += 0.5f;
      else point.y += 0.5f;

      if (point.z < 0) point.z += 0.5f;
      else point.z += 0.5f;

      return point;
    }

    /// <summary>
    ///   Rebuild the voxel mesh.
    /// </summary>
    public void RefreshMesh()
    {
      int size = this.Radius * 2;
      Color32 empty = new Color32(0, 0, 0, 255);

      IWritableVoxelMesh sphere = new VoxelArray(size, size, size);
      sphere.Start.X -= this.Radius;
      sphere.Start.Y -= this.Radius;
      sphere.Start.Z -= this.Radius;

      for(int x = 0; x < size; ++x) 
      {
        for(int y = 0; y < size; ++y) 
        {
          for(int z = 0; z < size; ++z) 
          {
            Vector3 point = this.GetPoint(x, y, z);

            if (point.magnitude <= this.Radius)
            {
              sphere.AbsoluteSet(point, this.Color);
            }
            else 
            {
              sphere.AbsoluteSet(point, empty);
            }
          }
        }
      }

      this.Mesh = sphere;
    }
  }
}
