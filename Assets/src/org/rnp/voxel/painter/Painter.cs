using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using UnityEditor;
using org.rnp.gui.colorPicker;
using System;
using org.rnp.gui.colorPalette;

namespace org.rnp.voxel.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for paint.
  /// </summary>
  [ExecuteInEditMode]
  public sealed class Painter : VoxelMeshContainer 
  {
    public VoxelMesh PaintedMesh;

    public PainterColorPalette Palette;

    public ColorPicker Picker;
    public Cursor cursor;

    public override VoxelMesh Mesh
    {
      get
      {
        return this.PaintedMesh;
      }
    }

    public void DitPlopl()
    {
      Debug.Log("plopl !");
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.PaintedMesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
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
        VoxelMeshes.Fill(this.PaintedMesh, cursor.Location, cursor.Dimensions, Voxels.Empty);
        this.PaintedMesh.Commit();
      }

      if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
      {
        // bug
        Color pickedColor = this.Picker.PickedColor;
        this.Palette.AddColor(pickedColor);
        
        if ((int)(pickedColor.a * 255f) >= 254f)
        {
          pickedColor.a = 0;
        }
        else
        {
          pickedColor.a = 1;
        }

        VoxelMeshes.Fill(this.PaintedMesh, cursor.Location, cursor.Dimensions, pickedColor);
        this.PaintedMesh.Commit();
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
      VoxelFile.Save(PaintedMesh, path);
    }

    public void OpenMesh()
    {
      var file = EditorUtility.OpenFilePanel("", "", "vxl");

      VoxelMesh vm= VoxelFile.Load(file);
      if(vm != null)
          this.PaintedMesh = vm;

      this.PaintedMesh.Commit();

      this.Palette.Clear();

      if (vm != null)
        this.Palette.AnalyseVoxel(vm);
    }
  }
}