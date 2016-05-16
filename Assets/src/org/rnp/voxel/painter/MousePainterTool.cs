using org.rnp.voxel.mesh;
using org.rnp.voxel.physics;
using org.rnp.voxel.translator;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.painter
{
  public class MousePainterTool : PainterTool
  {
    public VoxelFilter filter;

    public override void OnToolEnd()
    {
    }

    public override void OnToolStart()
    {
    }

    public override void OnToolUpdate()
    {
      if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        VoxelHit result; 

        if (VoxelPhysics.IsRayCollideVoxelMesh(ray, filter, out result))
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

          if (Input.GetMouseButtonDown(0))
          {
            this.Parent.PaintedMesh[result.HittedFaceVoxel] = pickedColor;
          }
          else
          {
            this.Parent.PaintedMesh[result.HittedVoxel] = Voxels.Empty;
          }

          this.Parent.PaintedMesh.Commit();
        }
      }
    }
  }
}
