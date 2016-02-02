using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple, procedural, spheric voxel mesh
  /// </summary>
  [ExecuteInEditMode]
  public class SphereVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Sphere radius in voxels.
    /// </summary>
    [SerializeField]
    protected int _Radius;

    /// <summary>
    ///   Sphere color.
    /// </summary>
    [SerializeField]
    protected Color32 _Color;

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

    /// <summary>
    ///   Return a point in space.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    protected Vector3 GetPoint(int x, int y, int z)
    {
      return (new Vector3(
        (float)(x - this.Radius) + 0.5f,
        (float)(y - this.Radius) + 0.5f,
        (float)(z - this.Radius) + 0.5f
      ));
    }

    /// <summary>
    ///   Rebuild the voxel mesh.
    /// </summary>
    public void RefreshMesh()
    {
      int size = this.Radius * 2;
      Color32 empty = new Color32(0, 0, 0, 255);

      VoxelArray sphere = new VoxelArray(size, size, size);

      for(int x = 0; x < size; ++x) 
      {
        for(int y = 0; y < size; ++y) 
        {
          for(int z = 0; z < size; ++z) 
          {
            Vector3 point = this.GetPoint(x, y, z);

            if (point.magnitude <= this.Radius)
            {
              sphere[x, y, z] = this.Color;
            }
            else 
            {
              sphere[x, y, z] = empty;
            }
          }
        }
      }

      this.Mesh = sphere;
    }
  }
}
