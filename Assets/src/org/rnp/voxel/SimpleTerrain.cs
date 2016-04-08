using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A basic perlin noise voxel terrain.
  /// </summary>
  [ExecuteInEditMode]
  public class SimpleTerrain : VoxelMeshContainer
  {
    public VoxelMesh Terrain = new MapVoxelMesh(new Dimensions3D(16, 16, 16));

    /// <see cref="org.rnp.voxel.VoxelMeshContainer"/>
    public override VoxelMesh Mesh
    {
      get
      {
        return this.Terrain;
      }
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected override void Awake()
    {
      this.Generate();
    }

    /// <summary>
    ///   Generate the terrain.
    /// </summary>
    private void Generate()
    {
      this.Terrain.Clear();

      for(int i = 0; i < 100; ++i)
      {
        for(int k = 0; k < 100; ++k)
        {
          int height = (int)(Mathf.PerlinNoise(i/100f, k/100f) * 30);

          for(int j = 0; j <= height; ++j)
          {
            Terrain[i, j, k] = new Color32(110,110,110,0);
          }
        }
      }

      this.Terrain.Commit();
    }
  }
}
