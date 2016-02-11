using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.unity.components.meshes;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for paint.
  /// </summary>
  [ExecuteInEditMode]
  public sealed class Painter : MonoBehaviour 
  {
    /// <summary>
    ///   Mesh to paint.
    /// </summary>
    public VoxelMesh Mesh;

    private Ray lastRay = new Ray();

    public void OnDrawGizmos()
    {
      Gizmos.DrawRay(lastRay.origin, lastRay.direction);
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Start() 
    {
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update() 
    {
      if(Input.GetMouseButtonDown(0))
      {
        /*Debug.Log("WAZAAAA \\(::)/");
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        this.lastRay = myRay;
        if(VoxelPhysics.RayCast(myRay, this.Mesh))
        {
          Debug.Log("YOUUUUUUUUPIIIIIII !!!!");
        }
        else
        {
          Debug.Log("Lol...");
        }*/
      }
    }
  }
}