using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh.map;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.unity.components.meshes
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
    public int Radius
    {
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
    public Color32 Color
    {
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

      point.x += 0.5f;
      point.y += 0.5f;
      point.z += 0.5f;

      return point;
    }

    /// <summary>
    ///   Rebuild the voxel mesh.
    /// </summary>
    public void RefreshMesh()
    {
      int size = this.Radius * 2;

      IVoxelMesh sphere = new MapVoxelMesh();

      for (int x = 0; x < size; ++x)
      {
        for (int y = 0; y < size; ++y)
        {
          for (int z = 0; z < size; ++z)
          {
            Vector3 point = this.GetPoint(x, y, z);

            if (point.magnitude <= this.Radius)
            {
              sphere[x - this.Radius, y - this.Radius, z - this.Radius] = new Color32(
                (byte) (this.Color.r * (y / (float) size)),
                (byte) (this.Color.g * (y / (float) size)),
                (byte) (this.Color.b * (y / (float) size)), 
                this.Color.a
              );
            }
            else
            {
              sphere[x - this.Radius, y - this.Radius, z - this.Radius] = Voxels.Empty;
            }
          }
        }
      }

      this.Mesh = sphere;
    }
  }
}