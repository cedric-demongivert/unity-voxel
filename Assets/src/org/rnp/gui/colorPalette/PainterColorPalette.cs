using org.rnp.gui.colorPicker;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.gui.colorPalette
{
  public class PainterColorPalette : ColorPalette {
    public ColorPicker Picker;
    
    public void PushColor(Color32 color)
    {
      if(this.Picker != null)
      {
        Picker.PickedColor = color;
      }
    }

    public void CaptureColor()
    {
      if (this.Picker != null)
      {
        this.AddColor(this.Picker.PickedColor);
      }
    }

    public void AnalyseVoxel(VoxelMesh voxel)
    {
      VoxelLocation start = voxel.Start;
      VoxelLocation end = voxel.Start.Add(voxel.Dimensions);

      for(int i = start.X; i < end.X; ++i)
      {
        for (int j = start.Y; j < end.Y; ++j)
        {
          for (int k = start.Z; k < end.Z; ++k)
          {
            if(!Voxels.IsEmpty(voxel[i,j,k]))
            {
              Color32 voxelColor = voxel[i, j, k];

              //bug
              voxelColor.a = 255;
              this.AddColor(voxelColor);
            }
          }
        }
      }
    }
  }
}
