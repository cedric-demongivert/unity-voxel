using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace org.rnp.gui.colorPalette
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A color of a palette.
  /// </summary>
  public class ColorPaletteItem : MonoBehaviour
  {
    [SerializeField]
    private RawImage _image = null;

    [SerializeField]
    private ColorPalette _parent = null;

    [SerializeField]
    private Color32 _color = new Color32();

    public RawImage ColorImage
    {
      get
      {
        return this.GetColorImage();
      }
      set
      {
        this.SetColorImage(value);
      }
    }

    /// <summary>
    ///   The color palette that contains this color.
    /// </summary>
    public ColorPalette Parent {
      get
      {
        return this.GetParent();
      }
    }

    /// <summary>
    ///   The color of this element.
    /// </summary>
    public Color32 Color
    {
      get
      {
        return this.GetColor();
      }
      set
      {
        this.SetColor(value);
      }
    }

    /// <summary>
    ///   Get the color of this element.
    /// </summary>
    /// <returns></returns>
    public virtual Color32 GetColor()
    {
      return this._color;
    }

    /// <summary>
    ///   Change the GUI Item that render the color.
    /// </summary>
    /// <param name="value"></param>
    public void SetColorImage(RawImage value)
    {
      if (this._image == value) return;

      this._image = value;
      if(this._image != null)
      {
        this._image.color = this._color;
      }
    }

    /// <summary>
    ///   Return the GUI Item that render the color.
    /// </summary>
    /// <returns></returns>
    public RawImage GetColorImage()
    {
      return this._image;
    }

    /// <summary>
    ///   Change the color of this element.
    /// </summary>
    /// <param name="color"></param>
    public virtual void SetColor(Color32 color)
    {
      if(!this._color.Equals(color))
      {
        Color32 oldColor = this._color;
        this._color = color;

        if (this.Parent != null) this.Parent.ChangeColor(oldColor, color);
        if (this.ColorImage != null) this.ColorImage.color = color;
      }
    }
    
    /// <summary>
    ///   Initialize the color palette item with a parent.
    /// </summary>
    /// <param name="parent"></param>
    public void InitializeColorItem(ColorPalette parent, Color32 color)
    {
      if(this._parent == null)
      {
        this._parent = parent;
        this._color = color;
        if (this.ColorImage != null) this.ColorImage.color = color;
      }
      else
      {
        throw new Exception("This item is allready initialized.");
      }
    }

    /// <summary>
    ///   Return the palette that contains this element.
    /// </summary>
    /// <returns></returns>
    public virtual ColorPalette GetParent()
    {
      return this._parent;
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void OnDestroy()
    {
      if(this._parent != null)
      {
        this._parent.RemoveColor(this._color);
      }
    }
  }
}