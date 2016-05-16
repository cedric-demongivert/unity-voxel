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
  public class ReplacePainterTool : PainterTool
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
          // bug
          Color replaceColor = this.Parent.Picker.PickedColor;
          this.Parent.Palette.AddColor(replaceColor);

          if ((int)(replaceColor.a * 255f) >= 254f)
          {
            replaceColor.a = 0;
          }
          else
          {
            replaceColor.a = 1;
          }

          VoxelColor oldColor = this.Parent.PaintedMesh[result.HittedVoxel];

          this.ReplaceColor(oldColor, replaceColor);

          this.Parent.PaintedMesh.Commit();
        }
      }
    }

    /// <summary>
    ///  Optimizable
    /// </summary>
    /// <param name="oldColor"></param>
    /// <param name="replaceColor"></param>
    private void ReplaceColor(Color32 oldColor, Color32 replaceColor)
    {
      VoxelLocation start = this.Parent.PaintedMesh.Start;
      VoxelLocation end = this.Parent.PaintedMesh.Start.Add(this.Parent.PaintedMesh.Dimensions);
      
      for (int i = start.X; i < end.X; ++i)
      {
        for (int j = start.Y; j < end.Y; ++j)
        {
          for (int k = start.Z; k < end.Z; ++k)
          {
            if (this.Parent.PaintedMesh[i, j, k].Equals(oldColor))
            {
              this.Parent.PaintedMesh[i, j, k] = replaceColor;
            }
          }
        }
      }
    }
  }
}
