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
  ///   Random Mesh for tests
  /// </summary>
  [ExecuteInEditMode]
  public class RandomMesh : VoxelMeshContainer
  {
    private VoxelMesh _mesh = new MapVoxelMesh(new Dimensions3D(20, 20, 20));
    public Dimensions3D size = new Dimensions3D(40, 40, 40);
    public VoxelLocation start = new VoxelLocation(-20);

    private float _time = 0;
    public float Tick = 0.01f;

    /// <see cref="org.rnp.voxel.VoxelMeshContainer"/>
    public override VoxelMesh Mesh
    {
      get
      {
        return this._mesh;
      }
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected override void Awake()
    {  }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public override void Update()
    {
      this._time += Time.deltaTime;

      while(this._time > this.Tick)
      {
        this.Generate();
        this._time -= this.Tick;
      }
    }

    /// <summary>
    ///   Generate the terrain.
    /// </summary>
    private void Generate()
    {
      Dimensions3D toAddSize = new Dimensions3D(
        UnityEngine.Random.value * 15,
        UnityEngine.Random.value * 15,
        UnityEngine.Random.value * 15
      );

      VoxelLocation genered = this.start.Add(this.size.Sub(toAddSize).Mul(
        UnityEngine.Random.value,
        UnityEngine.Random.value,
        UnityEngine.Random.value
      ));

      VoxelMeshes.Fill(this._mesh, genered, toAddSize, new Color32(100, 100, 100, 0));

      //Debug.Log("Genered a cube of " + toAddSize + " at " + genered);

      this._mesh.Commit();
    }
  }
}
