using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.painter
{
  public class KeyBoardPainterTool : PainterTool
  {
    public Cursor cursor;

    public override void OnToolEnd()
    {
      this.cursor.gameObject.SetActive(false);
    }

    public override void OnToolStart()
    {
      this.cursor.gameObject.SetActive(true);
    }

    public override void OnToolUpdate()
    {
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
        VoxelMeshes.Fill(this.Parent.PaintedMesh, cursor.Location, cursor.Dimensions, Voxels.Empty);
        this.Parent.PaintedMesh.Commit();
      }

      if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
      {
        // bug
        Color pickedColor = this.Parent.Picker.PickedColor;
        this.Parent.Palette.AddColor(pickedColor);

        if ((int)(pickedColor.a * 255f) >= 254f)
        {
          pickedColor.a = 0;
        }
        else
        {
          pickedColor.a = 1;
        }

        VoxelMeshes.Fill(this.Parent.PaintedMesh, cursor.Location, cursor.Dimensions, pickedColor);
        this.Parent.PaintedMesh.Commit();
      }
    }
  }
}
