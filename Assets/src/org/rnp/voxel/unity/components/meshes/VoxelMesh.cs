using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.unity.components.meshes
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for storing mesh data.
  /// </summary>
  [ExecuteInEditMode]
  public class VoxelMesh : MonoBehaviour 
  {
    /// <summary>
    ///   Stored voxel mesh.
    /// </summary>
    public IVoxelMesh Mesh;

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public virtual void Awake()
    {
      // nothing
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public virtual void Start() 
    {
      // nothing
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public virtual void Update() 
    { 
      // nothing
    }
  }
}