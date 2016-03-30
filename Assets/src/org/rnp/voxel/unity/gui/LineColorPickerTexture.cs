using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui.exception;
using UnityEngine;

namespace org.rnp.voxel.unity.gui
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A line texture for a color picker, it allow you to pick a color value
  /// in a line.
  /// </summary>
  public class LineColorPickerTexture
  {
    private Texture2D _texture;

    /// <summary>
    ///   The locked attribute (selected by this texture)
    /// </summary>
    private GUIColorPicker.ColorAttr _lockedAttr;

    /// <summary>
    ///   Paint method.
    /// </summary>
    private bool _isVertical = true;

    /// <summary>
    ///   The locked attribute value.
    /// </summary>
    private VoxelColor _selectedColor;

    /// <summary>
    ///   Genered texture.
    /// </summary>
    public Texture2D Texture
    {
      get
      {
        return this._texture;
      }
    }

    public int width
    {
      get
      {
        return this._texture.width;
      }
    }

    public int height
    {
      get
      {
        return this._texture.height;
      }
    }

    /// <summary>
    ///   The locked attribute (selected by this texture)
    /// </summary>
    public GUIColorPicker.ColorAttr LockedAttr
    {
      get
      {
        return this._lockedAttr;
      }
      set
      {
        this._lockedAttr = value;
        this.Refresh();
      }
    }

    /// <summary>
    ///   Paint method.
    /// </summary>
    public bool IsVertical
    {
      get
      {
        return this._isVertical;
      }
      set
      {
        this._isVertical = value;
        this.Refresh();
      }
    }

    /// <summary>
    ///   The locked attribute value.
    /// </summary>
    public VoxelColor SelectedColor
    {
      get
      {
        return this._selectedColor.Copy();
      }
      set
      {
        this._selectedColor.Set(value);
        this.Refresh();
      }
    }

    /// <summary>
    ///   Create a new square color texture.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public LineColorPickerTexture(int width, int height)
    {
      this._texture = new Texture2D(width, height);
      this._lockedAttr = GUIColorPicker.ColorAttr.Hue;
      this._selectedColor = new VoxelColor();
      this.Refresh();
    }

    /// <summary>
    ///   Reset the entire configuration of the object.
    /// </summary>
    /// <param name="lockedAttribute"></param>
    /// <param name="lockedAttributeValue"></param>
    public void Configure(GUIColorPicker.ColorAttr lockedAttribute, VoxelColor selectedColor)
    {
      this._lockedAttr = lockedAttribute;
      this._selectedColor.Set(selectedColor);
      this.Refresh();
    }

    /// <summary>
    ///   Repaint the texture.
    /// </summary>
    public void Refresh()
    {
      VoxelColor color = new VoxelColor();

      for (int i = 0; i < this.width; ++i)
      {
        for (int j = 0; j < this.height; ++j)
        {
          this.GetColorAt(i, j, color);
          this._texture.SetPixel(i, j, color);
        }
      }

      this._texture.Apply();
    }

    /// <summary>
    ///   Get a color from a line texture.
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <result></result>
    public VoxelColor GetColorAt(float coord)
    {
      VoxelColor result = new VoxelColor();
      this.GetColorAt(coord, result);
      return result;
    }

    /// <summary>
    ///   Return the selected pixel.
    /// </summary>
    /// <returns></returns>
    public int GetSelectedPixel()
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
        case GUIColorPicker.ColorAttr.Green:
        case GUIColorPicker.ColorAttr.Blue:
          return this.GetSelectedPixelRGB();
        case GUIColorPicker.ColorAttr.Hue:
        case GUIColorPicker.ColorAttr.Saturation:
        case GUIColorPicker.ColorAttr.Luminosity:
          return this.GetSelectedPixelHSL();
        default:
          throw new ColorPickerMissConfigurationException("Unhandled locked attribute : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get selected pixel in RGB mode.
    /// </summary>
    /// <returns></returns>
    private int GetSelectedPixelRGB()
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
          return this.height - (int)(this._selectedColor.R * this.height);
        case GUIColorPicker.ColorAttr.Green:
          return this.height - (int)(this._selectedColor.G * this.height);
        case GUIColorPicker.ColorAttr.Blue:
          return this.height - (int)(this._selectedColor.B * this.height);
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get selected pixel in HSL mode.
    /// </summary>
    /// <returns></returns>
    private int GetSelectedPixelHSL()
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Hue:
          return this.height - (int)(this._selectedColor.Hue * this.height);
        case GUIColorPicker.ColorAttr.Saturation:
          return this.height - (int)(this._selectedColor.Saturation * this.height);
        case GUIColorPicker.ColorAttr.Luminosity:
          return this.height - (int)(this._selectedColor.Luminosity * this.height);
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get a color from a line texture.
    /// </summary>
    /// <param name="x">Coord of the color between 0 and width (exclusive)</param>
    /// <param name="y">Coord of the color between 0 and height (exclusive)</param>
    /// <result></result>
    public VoxelColor GetColorAt(int x, int y)
    {
      VoxelColor result = new VoxelColor();
      this.GetColorAt(x, y, result);
      return result;
    }

    /// <summary>
    ///   Get a color from a line texture.
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="outColor">Result color</param>
    public void GetColorAt(float coord, VoxelColor outColor)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
        case GUIColorPicker.ColorAttr.Green:
        case GUIColorPicker.ColorAttr.Blue:
          this.GetColorRGB(coord, outColor);
          break;
        case GUIColorPicker.ColorAttr.Hue:
        case GUIColorPicker.ColorAttr.Saturation:
        case GUIColorPicker.ColorAttr.Luminosity:
          this.GetColorHSL(coord, outColor);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Unhandled locked attribute : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get a color from a line texture.
    /// </summary>
    /// <param name="x">Coord of the color between 0 and width (exclusive)</param>
    /// <param name="y">Coord of the color between 0 and height (exclusive)</param>
    /// <param name="outColor">Result color</param>
    public void GetColorAt(int x, int y, VoxelColor outColor)
    {
      if (this._isVertical)
      {
        this.GetColorAt(y / (float)this.height, outColor);
      }
      else
      {
        this.GetColorAt(x / (float)this.width, outColor);
      }
    }

    /// <summary>
    ///   Get a color for a line texture (RGB mode).
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="outColor">Result color</param>
    private void GetColorRGB(float coord, VoxelColor outColor)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
          outColor.R = coord;
          outColor.G = this._selectedColor.G;
          outColor.B = this._selectedColor.B;
          break;
        case GUIColorPicker.ColorAttr.Green:
          outColor.R = this._selectedColor.R;
          outColor.G = coord;
          outColor.B = this._selectedColor.B;
          break;
        case GUIColorPicker.ColorAttr.Blue:
          outColor.R = this._selectedColor.R;
          outColor.G = this._selectedColor.G;
          outColor.B = coord;
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get a color for a line texture (HSL mode).
    /// </summary>
    /// <param name="coord">Coord of the color between 0f and 1f</param>
    /// <param name="outColor">Result color.</param>
    private void GetColorHSL(float coord, VoxelColor outColor)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Hue:
          outColor.SetHSL(
            coord,
            this._selectedColor.Saturation,
            this._selectedColor.Luminosity
          );
          break;
        case GUIColorPicker.ColorAttr.Saturation:
          outColor.SetHSL(
            this._selectedColor.Hue,
            coord,
            this._selectedColor.Luminosity
          );
          break;
        case GUIColorPicker.ColorAttr.Luminosity:
          outColor.SetHSL(
            this._selectedColor.Hue,
            this._selectedColor.Saturation,
            coord
          );
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }

  }
}
