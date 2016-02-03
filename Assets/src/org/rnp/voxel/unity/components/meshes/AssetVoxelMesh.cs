using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.meshes
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Load a voxel mesh from assets data files.
  /// </summary>
  [ExecuteInEditMode]
  public class AssetVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Mesh data.
    /// </summary>
    public TextAsset AssetToLoad;

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public override void Awake()
    {
      // loading asset.
    }
  }
}
