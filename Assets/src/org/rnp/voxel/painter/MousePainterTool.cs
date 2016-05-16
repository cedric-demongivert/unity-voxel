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
      if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (VoxelPhysics.IsRayCollideVoxelMesh(ray, filter))
        {
          Debug.Log("YEEESSS !");
        }
        else
        {
          Debug.Log("NOOOOOOOOOOOO !");
        }
      }
    }
  }
}
