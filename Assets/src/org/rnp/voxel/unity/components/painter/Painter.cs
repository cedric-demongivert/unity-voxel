using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.unity.components.translators;
using org.rnp.voxel.unity.components.colliders;
using org.rnp.voxel.utils;
using Assets.src.org.rnp.voxel.utils;
using UnityEditor;

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
    public Cursor cursor;

    public void DitPlopl()
    {
      Debug.Log("plopl !");
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
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
        this.PaintedMesh.Mesh.Fill(cursor.Location, cursor.Dimensions, Voxels.Empty);

        this.PaintedTranslator.Translate();
        this.PaintedTranslator.Publish();

        this.PaintedCollider.RefreshCollider();
      }

      if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
      {
        // bug
        Color pickedColor = this.Picker.SelectedColor;

        if ((int)(pickedColor.a * 255f) >= 254f)
        {
          pickedColor.a = 0;
        }
        else
        {
          pickedColor.a = 255;
        }

        this.PaintedMesh.Mesh.Fill(cursor.Location, cursor.Dimensions, pickedColor);

        this.PaintedTranslator.Translate();
        this.PaintedTranslator.Publish();

        this.PaintedCollider.RefreshCollider();
      }
    }


    public void SaveMesh()
    {
            //Ask for file
            var path = EditorUtility.SaveFilePanel(
                    "Save Mesh",
                    "",
                    "Mesh" + ".vxl",
                    "vxl");


            //Save Mesh to file
            VoxelFile.Save( PaintedMesh.Mesh, path);
    }

    public void OpenMesh()
    {
            var file = EditorUtility.OpenFilePanel("", "", "vxl");

            IVoxelMesh vm= VoxelFile.Load(file);
            if(vm != null)
                PaintedMesh.Mesh = vm;

            this.PaintedTranslator.Translate();
            this.PaintedTranslator.Publish();

            this.PaintedCollider.RefreshCollider();

        }
  }
}