using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui.exception;

namespace org.rnp.voxel.unity.gui
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Methods for color pickers.
  /// </summary>
  public static class GUIColorPicker
  {
    public enum ColorAttr
    {
      Hue, Saturation, Luminosity, Red, Green, Blue
    }

    /// <summary>
    ///   A field that allow you to enter a color attribute value between 0 and 255.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public static VoxelColor LayoutHexaField(VoxelColor value, String label)
    {
      Color parsed;
      VoxelColor result = value;
      string str = ((Color)value).ToHexStringRGBA();
      string modified = str;

      GUILayout.BeginHorizontal();

      GUILayout.Label(label);
      modified = GUILayout.TextField(str, GUILayout.Width(125));

      if (!modified.Equals(str) && Color.TryParseHexString(modified, out parsed))
      {
        result = parsed;
      }

      GUILayout.EndHorizontal();

      return result;
    }
    
    /// <summary>
    ///   A field that allow you to enter a color attribute value between min and max.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int LayoutColorField(String label, int value, int min, int max)
    {
      int result = 0;
            
      GUILayout.BeginHorizontal();
        
        GUILayout.Label(label);
        String toParse = GUILayout.TextField(value.ToString(), GUILayout.Width(125));

      GUILayout.EndHorizontal();

      if (int.TryParse(toParse, out result))
      {
        if (result > max) result = max;
        if (result < min) result = min;
      }
      else
      {
        result = 0;
      }

      return result;
    }

    /// <summary>
    ///   A field that allow to select a color attribute.
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ColorAttr LayoutAttrChkbox(ColorAttr selected, ColorAttr value)
    {
      if(GUILayout.Toggle(selected == value, new GUIContent()))
      {
        return value;
      }
      else
      {
        return selected;
      }
    }
  }
}
