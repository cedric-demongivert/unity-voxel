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
  /// <summary>
  ///   A square texture for a color picker, you can use it in order to pick
  ///   2D values for a color.
  /// </summary>
  public class SquareColorPickerTexture
  {
    private Texture2D _texture;

    /// <summary>
    ///   The locked attribute (selected with a BarColorPickerTexture)
    /// </summary>
    private GUIColorPicker.ColorAttr _lockedAttr;

    /// <summary>
    ///   Selected color
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

    public int width {
      get {
        return this._texture.width;
      }
    }

    public int height {
      get {
        return this._texture.height;
      }
    }

    /// <summary>
    ///   The locked attribute (selected with a BarColorPickerTexture)
    /// </summary>
    public GUIColorPicker.ColorAttr LockedAttribute
    {
      get
      {
        return _lockedAttr;
      }
      set
      {
        _lockedAttr = value;
        this.Refresh();
      }
    }

    /// <summary>
    ///   Selected color
    /// </summary>
    public VoxelColor SelectedColor
    {
      get
      {
        return _selectedColor.Copy();
      }
      set
      {
        _selectedColor.Set(value);
        this.Refresh();
      }
    }


    /// <summary>
    ///   Return the selected pixel.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetSelectedPixel()
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
    private Vector2 GetSelectedPixelRGB()
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
          return new Vector2(
            (int)(this._selectedColor.G * this.width),
            this.height - (int)(this._selectedColor.B * this.height)
          );
        case GUIColorPicker.ColorAttr.Green:
          return new Vector2(
            (int)(this._selectedColor.R * this.width),
            this.height - (int)(this._selectedColor.B * this.height)
          );
        case GUIColorPicker.ColorAttr.Blue:
          return new Vector2(
            (int)(this._selectedColor.R * this.width),
            this.height - (int)(this._selectedColor.G * this.height)
          );
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get selected pixel in HSL mode.
    /// </summary>
    /// <returns></returns>
    private Vector2 GetSelectedPixelHSL()
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Hue:
          return new Vector2(
            (int)(this._selectedColor.Saturation * this.width),
            this.height - (int)(this._selectedColor.Luminosity * this.height)
          );
        case GUIColorPicker.ColorAttr.Saturation:
          return new Vector2(
            (int)(this._selectedColor.Hue * this.width),
            this.height - (int)(this._selectedColor.Luminosity * this.height)
          );
        case GUIColorPicker.ColorAttr.Luminosity:
          return new Vector2(
            (int)(this._selectedColor.Hue * this.width),
            this.height - (int)(this._selectedColor.Saturation * this.height)
          );
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Create a new square color texture.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public SquareColorPickerTexture(int width, int height)
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
    ///   Pick a color from the square picker.
    /// </summary>
    /// <param name="x">Coord of the color between 0 and width (exclusive, absciss)</param>
    /// <param name="y">Coord of the color between 0 and height (exclusive, ordinates)</param>
    /// <returns></returns>
    public VoxelColor GetColorAt(int x, int y)
    {
      VoxelColor result = new VoxelColor();
      this.GetColorAt(x, y, result);
      return result;
    }

    /// <summary>
    ///   Pick a color from the square picker.
    /// </summary>
    /// <param name="x">Coord of the color between 0f and 1f (absciss)</param>
    /// <param name="y">Coord of the color between 0f and 1f (ordinates)</param>
    /// <returns></returns>
    public VoxelColor GetColorAt(float x, float y)
    {
      VoxelColor result = new VoxelColor();
      this.GetColorAt(x, y, result);
      return result;
    }

    /// <summary>
    ///   Pick a color from the square picker.
    /// </summary>
    /// <param name="x">Coord of the color between 0 and width (exclusive, absciss)</param>
    /// <param name="y">Coord of the color between 0 and height (exclusive, ordinates)</param>
    /// <param name="result">Result, if instanciation is not required</param>
    public void GetColorAt(int x, int y, VoxelColor result)
    {
      this.GetColorAt(x / (float)this.width, y / (float)this.height, result);
    }


    /// <summary>
    ///   Pick a color from the square picker.
    /// </summary>
    /// <param name="x">Coord of the color between 0f and 1f (absciss)</param>
    /// <param name="y">Coord of the color between 0f and 1f (ordinates)</param>
    /// <param name="result">Result, if instanciation is not required</param>
    public void GetColorAt(float x, float y, VoxelColor result)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
        case GUIColorPicker.ColorAttr.Green:
        case GUIColorPicker.ColorAttr.Blue:
          this.GetColorRGB(x, y, result);
          break;
        case GUIColorPicker.ColorAttr.Hue:
        case GUIColorPicker.ColorAttr.Saturation:
        case GUIColorPicker.ColorAttr.Luminosity:
          this.GetColorHSL(x, y, result);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Unhandled locked attribute : " + this._lockedAttr);
      }
    }
    
    /// <summary>
    ///   Get a color for a square texture (RGB mode).
    /// </summary>
    /// <param name="x">Coord of the color between 0f and 1f (absciss)</param>
    /// <param name="y">Coord of the color between 0f and 1f (ordinates)</param>
    /// <param name="result">Result, if instanciation is not required</param>
    private void GetColorRGB(float x, float y, VoxelColor result)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Red:
          result.Set(this._selectedColor.R, x, y);
          break;
        case GUIColorPicker.ColorAttr.Green:
          result.Set(x, this._selectedColor.G, y);
          break;
        case GUIColorPicker.ColorAttr.Blue:
          result.Set(x, y, this._selectedColor.B);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for RGB mode : " + this._lockedAttr);
      }
    }

    /// <summary>
    ///   Get a color for a square texture (HSL mode).
    /// </summary>
    /// <param name="x">Coord of the color between 0f and 1f (absciss)</param>
    /// <param name="y">Coord of the color between 0f and 1f (ordinates)</param>
    /// <param name="result">Result, if instanciation is not required</param>
    private void GetColorHSL(float x, float y, VoxelColor result)
    {
      switch (this._lockedAttr)
      {
        case GUIColorPicker.ColorAttr.Hue:
          result.SetHSL(this._selectedColor.Hue, x, y);
          break;
        case GUIColorPicker.ColorAttr.Saturation:
          result.SetHSL(x, this._selectedColor.Saturation, y);
          break;
        case GUIColorPicker.ColorAttr.Luminosity:
          result.SetHSL(x, y, this._selectedColor.Luminosity);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }

  }
}
