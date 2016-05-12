using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui.exception;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace org.rnp.gui.colorPicker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A line texture for a color picker, it allow you to pick a color value
  /// in a line.
  /// </summary>
  public class LineColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    private Texture2D _texture;

    /// <summary>
    ///   The locked attribute (selected by this texture)
    /// </summary>
    private GUIColorPicker.ColorAttr _lockedAttr = GUIColorPicker.ColorAttr.Red;

    /// <summary>
    ///   Paint method.
    /// </summary>
    [SerializeField]
    private bool _isVertical = true;

    private bool _track;

    [SerializeField]
    private RawImage _image;
    
    [SerializeField]
    private ColorPicker _parentPicker;

    [SerializeField]
    private RectTransform _cursor;

    /// <summary>
    ///   The locked attribute value.
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
        value.LinePicker = this;

        this.SynchronizeLockedAttrWithParent(false);
        this.SynchronizePickedColorWithParent(false);
      }
    }

    public int Width
    {
      get
      {
        if (this._image != null)
        {
          return (int)this._image.rectTransform.sizeDelta.x;
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
          return (int)this._image.rectTransform.sizeDelta.y;
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
        if (this._image != null) this._image.texture = this._texture;
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

        if (this.IsVertical)
        {
          this._texture = new Texture2D(1, 255);
        }
        else
        {
          this._texture = new Texture2D(255, 1);
        }

        this.RefreshTexture();
      }
    }

    private void RefreshCursor()
    {
      if (this._cursor != null)
      {
        if (this.IsVertical)
        {
          this._cursor.localPosition = new Vector2(
            this._cursor.localPosition.x,
            this.GetSelectedPixel()
          );
        }
        else
        {
          this._cursor.localPosition = new Vector2(
            this.GetSelectedPixel(),
            this._cursor.localPosition.y
          );
        }
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
      if(this.IsVertical)
      {
        this._texture = new Texture2D(1, 255);
      }
      else
      {
        this._texture = new Texture2D(255, 1);
      }

      this.SynchronizeLockedAttrWithParent(false);
      this.SynchronizePickedColorWithParent(false);
      this.RefreshTexture();
      this._track = false;
      if (this._image != null) this._image.texture = this._texture;
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
      VoxelColor color = new VoxelColor();
      
      for (int i = 0; i < 255; ++i)
      {
        this.GetColorAt(i / 255f, color);

        if(this.IsVertical)
        {
          this._texture.SetPixel(0, i, color);
        }
        else
        {

          this._texture.SetPixel(i, 0, color);
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
          return (int)(this._pickedColor.R * this.Height);
        case GUIColorPicker.ColorAttr.Green:
          return (int)(this._pickedColor.G * this.Height);
        case GUIColorPicker.ColorAttr.Blue:
          return (int)(this._pickedColor.B * this.Height);
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
          return (int)(this._pickedColor.Hue * this.Height);
        case GUIColorPicker.ColorAttr.Saturation:
          return (int)(this._pickedColor.Saturation * this.Height);
        case GUIColorPicker.ColorAttr.Luminosity:
          return (int)(this._pickedColor.Luminosity * this.Height);
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
        this.GetColorAt(y / (float)this.Height, outColor);
      }
      else
      {
        this.GetColorAt(x / (float)this.Width, outColor);
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
          outColor.G = this._pickedColor.G;
          outColor.B = this._pickedColor.B;
          break;
        case GUIColorPicker.ColorAttr.Green:
          outColor.R = this._pickedColor.R;
          outColor.G = coord;
          outColor.B = this._pickedColor.B;
          break;
        case GUIColorPicker.ColorAttr.Blue:
          outColor.R = this._pickedColor.R;
          outColor.G = this._pickedColor.G;
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
            this._pickedColor.Saturation,
            this._pickedColor.Luminosity
          );
          break;
        case GUIColorPicker.ColorAttr.Saturation:
          outColor.SetHSL(
            this._pickedColor.Hue,
            coord,
            this._pickedColor.Luminosity
          );
          break;
        case GUIColorPicker.ColorAttr.Luminosity:
          outColor.SetHSL(
            this._pickedColor.Hue,
            this._pickedColor.Saturation,
            coord
          );
          break;
        default:
          throw new ColorPickerMissConfigurationException("Invalid locked value for HSL mode : " + this._lockedAttr);
      }
    }

  }
}
