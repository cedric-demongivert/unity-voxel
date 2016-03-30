using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.mesh.map;

namespace org.rnp.voxel.unity.components.meshes
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for storing mesh data.
  /// </summary>
  [ExecuteInEditMode]
  public class EmptyVoxelMesh : VoxelMesh 
  {
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public override void Awake()
    {
      this.Mesh = new MapVoxelMesh();
    }
  }
}