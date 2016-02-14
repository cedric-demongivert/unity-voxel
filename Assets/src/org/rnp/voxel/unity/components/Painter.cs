using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.unity.components.translators;

namespace org.rnp.voxel.unity.components
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for paint.
  /// </summary>
  [ExecuteInEditMode]
  public sealed class Painter : MonoBehaviour 
  {
    public PrototypeTranslator ToRefresh;

    public Color32 Color;

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
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastVoxelHit hitInfo = null;

        if(VoxelPhysics.Raycast(myRay, out hitInfo))
        {
          hitInfo.HittedMesh.Mesh[hitInfo.HittedInnerVoxel] = this.Color;
          this.ToRefresh.Translate();
          this.ToRefresh.Publish();

          hitInfo.HittedCollider.RefreshCollider();
        }
      }
    }
  }
}