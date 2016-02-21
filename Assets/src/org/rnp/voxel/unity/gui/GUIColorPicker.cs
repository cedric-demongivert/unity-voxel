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
    public enum ColorMode {
      HSL, RGB
    }

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
    ///   A field that allow you to enter a color attribute value between 0 and 255.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public static float LayoutColorField(string text, float value, String label, out string modified)
    {
      int parsed;
      float result = value;

      string str = ((int)(value * 255f)).ToString();
      if (text != null) str = text;
      
      GUILayout.BeginHorizontal();
        
        GUILayout.Label(label);
        modified = GUILayout.TextField(str, GUILayout.Width(125));

      GUILayout.EndHorizontal();

      if(int.TryParse(modified, out parsed))
      {
        if (parsed > 255) parsed = 255;
        if (parsed < 0) parsed = 0;
        result = parsed / 255f;
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

    public static ColorMode GetModeOf(ColorAttr attr)
    {
      switch (attr)
      {
        case ColorAttr.Red : 
        case ColorAttr.Green :
        case ColorAttr.Blue :
          return ColorMode.RGB;
        case ColorAttr.Luminosity :
        case ColorAttr.Hue :
        case ColorAttr.Saturation :
          return ColorMode.HSL;
        default :
          throw new ColorPickerMissConfigurationException("Unknown mode for " + attr);
      }
    }

    /// <summary>
    ///   Fill a square texture in order to pick a color in it.
    /// </summary>
    /// <param name="square">The texture to fill.</param>
    /// <param name="mode">Mode used for representation (RGB, HSL, etc...)</param>
    /// <param name="baseColor">Picked color</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    public static void ColorSquare(Texture2D square, VoxelColor baseColor, ColorMode mode, ColorAttr locked)
    {
      VoxelColor color = new VoxelColor(baseColor);
      color.A = 1f;
      Vector2 colorLocation = new Vector2();

      for(int i = 0; i < square.width; ++i)
      {
        for(int j = 0; j < square.height; ++j)
        {
          colorLocation.Set(i / (float)square.width, j / (float)square.height);
          GUIColorPicker.GetSquareColor(colorLocation, mode, locked, color);
          
          square.SetPixel(i, j, color);
        }
      }
    }

    /// <summary>
    ///   Get a color for a square texture.
    /// </summary>
    /// <param name="coord">Coord of the color between (0f, 0f) and (1f, 1f)</param>
    /// <param name="mode">Mode used for representation (RGB, HSL, etc...)</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetSquareColor(Vector2 coord, ColorMode mode, ColorAttr locked, VoxelColor outColor)
    {
      switch (mode)
      {
        case ColorMode.RGB:
          GUIColorPicker.GetSquareColorRGB(coord, locked, outColor);
          break;
        case ColorMode.HSL:
          GUIColorPicker.GetSquareColorHSL(coord, locked, outColor);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Unknown mode : " + mode);
      }
    }

    /// <summary>
    ///   Get a color for a square texture (RGB mode).
    /// </summary>
    /// <param name="coord">Coord of the color between (0f, 0f) and (1f, 1f)</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetSquareColorRGB(Vector2 coord, ColorAttr locked, VoxelColor outColor)
    {
      switch (locked)
      {
        case ColorAttr.Red:
          outColor.G = coord.x;
          outColor.B = coord.y;
          break;
        case ColorAttr.Green:
          outColor.R = coord.x;
          outColor.B = coord.y;
          break;
        case ColorAttr.Blue:
          outColor.R = coord.x;
          outColor.G = coord.y;
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + locked);
      }
    }

    /// <summary>
    ///   Get a color for a square texture (HSL mode).
    /// </summary>
    /// <param name="coord">Coord of the color between (0f, 0f) and (1f, 1f)</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetSquareColorHSL(Vector2 coord, ColorAttr locked, VoxelColor outColor)
    {
      switch (locked)
      {
        case ColorAttr.Hue:
          outColor.Saturation = coord.x;
          outColor.Luminosity = coord.y;
          break;
        case ColorAttr.Saturation:
          outColor.Hue = coord.x;
          outColor.Luminosity = coord.y;
          break;
        case ColorAttr.Luminosity:
          outColor.Hue = coord.x;
          outColor.Saturation = coord.y;
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + locked);
      }
    }

    /// <summary>
    ///   Fill a line texture in order to pick a color in it.
    /// </summary>
    /// <param name="line">The texture to fill.</param>
    /// <param name="baseColor">Picked color</param>
    /// <param name="mode">Mode used for representation (RGB, HSL, etc...)</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="horizontal">The texture must be fill from right to left.</param>
    public static void ColorLine(Texture2D line, VoxelColor baseColor, ColorMode mode, ColorAttr locked, bool horizontal = false)
    {
      VoxelColor color = new VoxelColor(baseColor);
      color.A = 1f;

      if (horizontal)
      {
        for (int i = 0; i < line.width; ++i)
        {
          GUIColorPicker.GetLineColor(i / (float)line.width, mode, locked, color);

          for (int j = 0; j < line.height; ++j)
          {
            line.SetPixel(i, j, color);
          }
        }
      }
      else
      {
        for (int j = 0; j < line.height; ++j)
        {
          GUIColorPicker.GetLineColor(j / (float)line.height, mode, locked, color);

          for (int i = 0; i < line.width; ++i)
          {
            line.SetPixel(i, j, color);
          }
        }
      }
    }

    /// <summary>
    ///   Get a color for a line texture.
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="mode">Mode used for representation (RGB, HSL, etc...)</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetLineColor(float coord, ColorMode mode, ColorAttr locked, VoxelColor outColor)
    {
      switch (mode)
      {
        case ColorMode.RGB:
          GUIColorPicker.GetLineColorRGB(coord, locked, outColor);
          break;
        case ColorMode.HSL:
          GUIColorPicker.GetLineColorHSL(coord, locked, outColor);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Unknown mode : " + mode);
      }
    }

    /// <summary>
    ///   Get a color for a line texture (RGB mode).
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetLineColorRGB(float coord, ColorAttr locked, VoxelColor outColor)
    {
      switch (locked)
      {
        case ColorAttr.Red:
          outColor.R = coord;
          break;
        case ColorAttr.Green:
          outColor.G = coord;
          break;
        case ColorAttr.Blue:
          outColor.B = coord;
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + locked);
      }
    }

    /// <summary>
    ///   Get a color for a line texture (HSL mode).
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="locked">Locked attribute (used for color line representation)</param>
    /// <param name="outColor">Result color.</param>
    public static void GetLineColorHSL(float coord, ColorAttr locked, VoxelColor outColor)
    {
      switch (locked)
      {
        case ColorAttr.Hue:
          outColor.SetHSL(coord, 1f, 1f);
          break;
        case ColorAttr.Saturation:
          outColor.Saturation = coord;
          break;
        case ColorAttr.Luminosity:
          outColor.Luminosity = coord;
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + locked);
      }
    }
  }
}
