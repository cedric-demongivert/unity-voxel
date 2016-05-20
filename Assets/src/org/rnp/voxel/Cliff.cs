using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Procedural Cliff
  /// </summary>
  [ExecuteInEditMode]
  public class Cliff : VoxelMeshContainer
  {
    private VoxelMesh _cliffMesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));

    private int Height = 5;
    private int HeightVariance = 3;

    private int Width = 5;
    private int Depth = 7;

    public override VoxelMesh Mesh
    {
      get
      {
        return _cliffMesh;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.Generate();
    }
    
    /// <summary>
    ///   Launch a bush generation
    /// </summary>
    public void Generate()
    {
      this._cliffMesh.Clear();
            
      


      this._cliffMesh.Commit();
    }
  }
}
