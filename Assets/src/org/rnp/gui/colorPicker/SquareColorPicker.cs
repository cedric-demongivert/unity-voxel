using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui.exception;
using UnityEngine.EventSystems;

namespace org.rnp.gui.colorPicker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A square texture for a color picker, you can use it in order to pick
  ///   2D values for a color.
  /// </summary>
  public class SquareColorPicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
  {
    /// <summary>
    ///   Image for drawing the color square
    /// </summary>
    [SerializeField]
    private RawImage _image;

    private Texture2D _texture;

    private bool _track;

    [SerializeField]
    private ColorPicker _parentPicker;

    [SerializeField]
    private RectTransform _cursor;

    /// <summary>
    ///   The locked attribute (selected with a BarColorPickerTexture)
    /// </summary>
    [SerializeField]
    private GUIColorPicker.ColorAttr _lockedAttr = GUIColorPicker.ColorAttr.Hue;

    /// <summary>
    ///   Selected color
    /// </summary>
    [SerializeField]
    private VoxelColor _pickedColor = new VoxelColor();

    public RectTransform Cursor
    {
      get
      {
        return this._cursor;
      }
      set
      {
        this._cursor = value;
        this.RefreshCursor();
      }
    }

    /// <summary>
    ///   Parent Color Picker
    /// </summary>
    public ColorPicker ParentPicker
    {
      get
      {
        return this._parentPicker;
      }
      set
      {
        if (this._parentPicker == value) return;

        ColorPicker old = this._parentPicker;
        this._parentPicker = null;

        if (old != null) old.LinePicker = null;

        this._parentPicker = value;
        value.SquarePicker = this;

        this.SynchronizeLockedAttrWithParent(false);
        this.SynchronizePickedColorWithParent(false);
      }
    }

    public int Width
    {
      get
      {
        if(this._image != null)
        {
          return (int) this._image.rectTransform.sizeDelta.x;
        }
        else
        {
          return 0;
        }
      }
    }

    public int Height
    {
      get
      {
        if (this._image != null)
        {
          return (int) this._image.rectTransform.sizeDelta.y;
        }
        else
        {
          return 0;
        }
      }
    }

    /// <summary>
    ///   Genered image.
    /// </summary>
    public RawImage Image
    {
      get
      {
        return this._image;
      }
      set
      {
        this._image = value;
        if(this._image != null) this._image.texture = this._texture;
        this.RefreshTexture();
      }
    }
    
    /// <summary>
    ///   The locked attribute (selected with a BarColorPickerTexture)
    /// </summary>
    public GUIColorPicker.ColorAttr LockedAttribute
    {
      get
      {
        return this._lockedAttr;
      }
      set
      {
        this._lockedAttr = value;
        this.SynchronizeLockedAttrWithParent(true);

        this.RefreshTexture();
      }
    }


    /// <summary>
    ///   Selected color
    /// </summary>
    public VoxelColor PickedColor
    {
      get
      {
        return _pickedColor.Copy();
      }
      set
      {
        _pickedColor.Set(value);
        this.RefreshCursor();
        this.SynchronizePickedColorWithParent(true);
        this.RefreshTexture();
      }
    }

    private void RefreshCursor()
    {
      if(this._cursor != null)
      {
        this._cursor.localPosition = this.GetSelectedPixel();
      }
    }

    private void SynchronizeLockedAttrWithParent(bool overrideParent)
    {
      if (this._parentPicker != null)
      {
        if (this._parentPicker.LockedAttribute != this._lockedAttr)
        {
          if (overrideParent)
          {
            this._parentPicker.LockedAttribute = this._lockedAttr;
          }
          else
          {
            this._lockedAttr = this._parentPicker.LockedAttribute;
          }
        }
      }
    }

    private void SynchronizePickedColorWithParent(bool overrideParent)
    {
      if (this._parentPicker != null)
      {
        if (this._parentPicker.PickedColor != this.PickedColor)
        {
          if (overrideParent)
          {
            this._parentPicker.PickedColor = this.PickedColor;
          }
          else
          {
            this.PickedColor = this._parentPicker.PickedColor;
          }
        }
      }
    }

    
    public void Awake()
    {
      this._texture = new Texture2D(255, 255);
      this.SynchronizeLockedAttrWithParent(false);
      this.SynchronizePickedColorWithParent(false);
      this.RefreshTexture();
      this._track = false;
      if (this._image != null) this._image.texture = this._texture;
    }

    /// <summary>
    ///   Reset the entire configuration of the object.
    /// </summary>
    /// <param name="lockedAttribute"></param>
    /// <param name="lockedAttributeValue"></param>
    public void Configure(GUIColorPicker.ColorAttr lockedAttribute, VoxelColor selectedColor)
    {
      this._lockedAttr = lockedAttribute;
      this._pickedColor.Set(selectedColor);
      this.RefreshTexture();
    }

    /// <summary>
    ///   Repaint the texture.
    /// </summary>
    public void RefreshTexture()
    {
      if (this._texture == null) return;

      VoxelColor color = new VoxelColor();

      for (int i = 0; i < 255; ++i)
      {
        for (int j = 0; j < 255; ++j)
        {
          this.GetColorAt(i/255f, j/255f, color);
          this._texture.SetPixel(i, j, color);
        }
      }

      this._texture.Apply();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (this._image != null)
      {
        this._image.rectTransform.SetAsLastSibling();
        if (RectTransformUtility.RectangleContainsScreenPoint(this._image.rectTransform, eventData.position, eventData.pressEventCamera))
        {
          this._track = true;
          this.UpdateCursorFromEventData(eventData);
        }
      }
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (this._track)
      {
        this._image.rectTransform.SetAsLastSibling();
        this.UpdateCursorFromEventData(eventData);
      }
    }

    private void UpdateCursorFromEventData(PointerEventData eventData)
    {
      Vector2 trackPosition;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._image.rectTransform, eventData.position, eventData.pressEventCamera, out trackPosition);

      if (trackPosition.x > this.Width) trackPosition.x = this.Width;
      if (trackPosition.x < 0) trackPosition.x = 0;

      if (trackPosition.y > this.Height) trackPosition.y = this.Height;
      if (trackPosition.y < 0) trackPosition.y = 0;

      this.PickedColor = this.GetColorAt((int)trackPosition.x, (int)trackPosition.y);
    } 

    public void OnPointerUp(PointerEventData eventData)
    {
      this._track = false;
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
            (int)(this._pickedColor.G * this.Width),
            (int)(this._pickedColor.B * this.Height)
          );
        case GUIColorPicker.ColorAttr.Green:
          return new Vector2(
            (int)(this._pickedColor.R * this.Width),
            (int)(this._pickedColor.B * this.Height)
          );
        case GUIColorPicker.ColorAttr.Blue:
          return new Vector2(
            (int)(this._pickedColor.R * this.Width),
            (int)(this._pickedColor.G * this.Height)
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
            (int)(this._pickedColor.Saturation * this.Width),
            (int)(this._pickedColor.Luminosity * this.Height)
          );
        case GUIColorPicker.ColorAttr.Saturation:
          return new Vector2(
            (int)(this._pickedColor.Hue * this.Width),
            (int)(this._pickedColor.Luminosity * this.Height)
          );
        case GUIColorPicker.ColorAttr.Luminosity:
          return new Vector2(
            (int)(this._pickedColor.Hue * this.Width),
            (int)(this._pickedColor.Saturation * this.Height)
          );
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
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
      this.GetColorAt(x / (float)this.Width, y / (float)this.Height, result);
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
          result.Set(this._pickedColor.R, x, y);
          break;
        case GUIColorPicker.ColorAttr.Green:
          result.Set(x, this._pickedColor.G, y);
          break;
        case GUIColorPicker.ColorAttr.Blue:
          result.Set(x, y, this._pickedColor.B);
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
          result.SetHSL(this._pickedColor.Hue, x, y);
          break;
        case GUIColorPicker.ColorAttr.Saturation:
          result.SetHSL(x, this._pickedColor.Saturation, y);
          break;
        case GUIColorPicker.ColorAttr.Luminosity:
          result.SetHSL(x, y, this._pickedColor.Luminosity);
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }
  }
}
