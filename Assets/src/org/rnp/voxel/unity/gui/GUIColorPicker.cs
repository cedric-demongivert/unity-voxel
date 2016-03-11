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
    ///   A field that allow you to enter a color attribute.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static String LayoutColorField(String label, String value)
    {
      GUILayout.BeginHorizontal();
        
        GUILayout.Label(label);
        String result = GUILayout.TextField(value, GUILayout.Width(125));

      GUILayout.EndHorizontal();

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

    /// <summary>
    ///   Convert a color object into an Hex string.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static String ToHex(VoxelColor color)
    {
      return ((Color)color).ToHexStringRGBA();
    }

    /// <summary>
    ///   Convert a hex string to a color.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool FromHex(String value, VoxelColor result)
    {
      Color parsed;

      if (Color.TryParseHexString(value, out parsed))
      {
        result.Set(parsed);
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}
