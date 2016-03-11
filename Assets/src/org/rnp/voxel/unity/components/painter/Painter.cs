using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.unity.components.translators;
using org.rnp.voxel.unity.components.colliders;


namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for paint.
  /// </summary>
  [ExecuteInEditMode]
  public sealed class Painter : MonoBehaviour 
  {
    public PrototypeTranslator PaintedTranslator;
    public OctreeCollider PaintedCollider;
    public VoxelMesh PaintedMesh;

    public ColorPicker Picker;

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
        // bug
        Color pickedColor = this.Picker.SelectedColor;

        if ((int) (pickedColor.a * 255f) >= 254f)
        {
          pickedColor.a = 0;
        }
        else
        {
          pickedColor.a = 255;
        }

        if(VoxelPhysics.Raycast(myRay, out hitInfo))
        {
          if (hitInfo.IsVoxelMesh)
          {
            this.PaintedMesh.Mesh[hitInfo.HittedOutVoxel] = pickedColor;
          }
          else
          {
            this.PaintedMesh.Mesh[hitInfo.HitResult.point] = pickedColor;
          }

          
          this.PaintedTranslator.Translate();
          this.PaintedTranslator.Publish();

          this.PaintedCollider.RefreshCollider();
        }
      }
    }
  }
}