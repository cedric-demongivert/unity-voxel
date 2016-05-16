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
  public class PipettePainterTool : PainterTool
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
      if(Input.GetMouseButtonDown(0) && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        VoxelHit result; 

        if (VoxelPhysics.IsRayCollideVoxelMesh(ray, filter, out result))
        {
          Color32 color = this.Parent.PaintedMesh[result.HittedVoxel];
          color.a = 255;

          this.Parent.Picker.PickedColor = color;
        }
      }
    }
  }
}
