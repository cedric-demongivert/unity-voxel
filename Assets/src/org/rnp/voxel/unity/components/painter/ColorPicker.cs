using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Gui for selecting colors.
  /// </summary>
  ///[ExecuteInEditMode]
  public class ColorPicker : MonoBehaviour
  {
    #region Fields
    /// <summary>
    ///   User selected color.
    /// </summary>
    private VoxelColor _selectedColor = new VoxelColor();
    private VoxelColor _lastColor = new VoxelColor();
    
    [SerializeField]
    private GUIColorPicker.ColorAttr _lockedAttribute = GUIColorPicker.ColorAttr.Red;

    private SquareColorPickerTexture _colorSquare;
    private LineColorPickerTexture _colorLine;

    #region GUI Window Configuration
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUI.Window.html"/>
    public int WindowId = 25;
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUI.Window.html"/>
    public String WindowName = "Color Picker";
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUISkin.html"/>
    [SerializeField]
    private GUISkin _skin = null;

    /// <summary>
    ///   Change the picker skin.
    /// </summary>
    public GUISkin Skin
    {
      get
      {
        return this._skin;
      }
      set
      {
        this._skin = value;
        this.Layout();
      }
    }

    public GUIColorPicker.ColorAttr LockedAttribute
    {
      get
      {
        return this._lockedAttribute;
      }
      set
      {
        if (this._lockedAttribute != value)
        {
          this._lockedAttribute = value;
          this.Refresh();
        }
      }
    }
    #endregion

    #region Layout
    private Rect _colorLineBounds = new Rect();
    private Rect _colorSquareBounds = new Rect();
    private Rect _windowBounds = new Rect();
    private Rect _fieldsBounds = new Rect();
    private Rect _titleBounds = new Rect();
    private Rect _draggableArea = new Rect();
    private int _toggleSize;
    #endregion

    #region Values
    private int _red = 0;
    private int _green = 0;
    private int _blue = 0;
    private int _hue = 0;
    private int _saturation = 0;
    private int _luminosity = 0;
    private int _alpha = 0;
    #endregion
    #endregion

    #region Getters & Setters
    public Color SelectedColor
    {
      get
      {
        return this._selectedColor;
      }
      set
      {
        this._selectedColor.Set(value);
        this.Refresh();
      }
    }
    #endregion

    #region GUI
    public void OnGUI()
    {
      GUI.skin = this._skin;

      this._windowBounds = GUI.Window(
        this.WindowId,
        this._windowBounds,
        this.ColorPickerWindow,
        new GUIContent()
      );

      GUI.skin = null;
    }

    protected void ColorPickerWindow(int windowId)
    {
      this.WindowId = windowId;
      this._lastColor.Set(this._selectedColor);

      GUI.DragWindow(this._draggableArea);

      // Title
      if (this._skin == null || this._skin.FindStyle("Window Title") == null)
      {
        GUI.Label(this._draggableArea, this.WindowName);
      }
      else
      {
        GUI.Label(this._draggableArea, this.WindowName, this._skin.FindStyle("Window Title"));
      }

      // Selection Area
      GUI.DrawTexture(this._colorSquareBounds, this._colorSquare.Texture);
      GUI.DrawTexture(this._colorLineBounds, this._colorLine.Texture);

      // Fields
      GUILayout.BeginArea(this._fieldsBounds);
        GUILayout.BeginVertical();

          // RGB
          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Red);
            this._red = GUIColorPicker.LayoutColorField("R", this._red, 0, 255);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Green);
            this._green = GUIColorPicker.LayoutColorField("G", this._green, 0, 255);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Blue);
            this._blue = GUIColorPicker.LayoutColorField("B", this._blue, 0, 255);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // HSL
          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Hue);
            this._hue = GUIColorPicker.LayoutColorField("H", this._hue, 0, 360);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Saturation);
            this._saturation = GUIColorPicker.LayoutColorField("S", this._saturation, 0, 100);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Luminosity);
            this._luminosity = GUIColorPicker.LayoutColorField("L", this._luminosity, 0, 100);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // Alpha
          GUILayout.BeginHorizontal();
            GUILayout.Space(this._toggleSize);
            this._alpha = GUIColorPicker.LayoutColorField("A", this._alpha, 0, 255);
          GUILayout.EndHorizontal();

          // #
          GUILayout.BeginHorizontal();
          GUILayout.Space(this._toggleSize);
          this._lastColor.Set(this._selectedColor);
          GUIColorPicker.LayoutHexaField(this._selectedColor, "#");
          GUILayout.EndHorizontal();

        GUILayout.EndVertical();
      GUILayout.EndArea();

      if(!this._selectedColor.Equals(this._lastColor))
      {
        this.Refresh();
      }
      else
      {
        this.Commit();
      }
    }

    /// <summary>
    ///   Calculate width and location of each window elements.
    /// </summary>
    public void Layout()
    {
      GUISkin skin = this._skin;

      if (skin == null)
      {
        this._toggleSize = 20;
        return;
      }

      RectOffset windowPadding = skin.window.padding;
      RectOffset windowBorder = skin.window.border;

      this._colorSquareBounds.x = windowPadding.left + windowBorder.left;
      this._colorSquareBounds.y = windowPadding.top + windowBorder.top;
      this._colorSquareBounds.width = this._colorSquare.width;
      this._colorSquareBounds.height = this._colorSquare.height;

      this._colorLineBounds.x = this._colorSquareBounds.xMax + 25;
      this._colorLineBounds.y = this._colorSquareBounds.y;
      this._colorLineBounds.width = this._colorLine.width;
      this._colorLineBounds.height = this._colorLine.height;

      this._fieldsBounds.x = this._colorLineBounds.xMax + 25;
      this._fieldsBounds.y = this._colorLineBounds.y;
      this._fieldsBounds.width = 200;
      this._fieldsBounds.height = this._colorSquare.height;

      this._windowBounds.width = this._fieldsBounds.xMax + windowPadding.right + windowBorder.right;
      this._windowBounds.height = this._fieldsBounds.yMax + windowPadding.bottom + windowBorder.bottom;

      this._draggableArea.x = this._draggableArea.y = 0;
      this._draggableArea.width = this._windowBounds.width;
      this._draggableArea.height = windowBorder.top;

      this._toggleSize = skin.toggle.border.left + skin.toggle.margin.left + skin.toggle.padding.left;
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Instantiate things.
    /// </summary>
    public void Awake()
    {
      this._colorSquare = new SquareColorPickerTexture(255, 255);
      this._colorLine = new LineColorPickerTexture(20, 255);

      this.Layout();
      this.Refresh();
    }

    /// <summary>
    ///   Recompute fields values.
    /// </summary>
    private void ResetFields()
    {
      this._alpha = (int)(this._selectedColor.A * 255);
      this._blue = (int)(this._selectedColor.B * 255);
      this._green = (int)(this._selectedColor.G * 255);
      this._red = (int)(this._selectedColor.R * 255);
      this._luminosity = (int)(this._selectedColor.Luminosity * 100);
      this._saturation = (int)(this._selectedColor.Saturation * 100);
      this._hue = (int)(this._selectedColor.Hue * 360);
    }

    /// <summary>
    ///   Commit change to the selected color (if has changed)
    /// </summary>
    private void Commit()
    {
      bool hasChanged = false;

      if (this.HasChangedRGB())
      {
        this._selectedColor.Set(this._red/255f, this._green/255f, this._blue/255f);
        hasChanged = true;
      }
      else if(this.HasChangedHSL())
      {
        this._selectedColor.SetHSL(this._hue/360f, this._saturation/100f, this._luminosity/100f);
        hasChanged = true;
      }

      if(this.HasChangedAlpha())
      {
        this._selectedColor.A = this._alpha/255f;
        hasChanged = true;
      }

      if(hasChanged) {
        this.Refresh();
      }
    }

    /// <summary>
    ///   Check if the selected (RGB) color has changed.
    /// </summary>
    /// <returns></returns>
    private bool HasChangedRGB()
    {
      return this._blue != (int)(this._selectedColor.B * 255) ||
             this._green != (int)(this._selectedColor.G * 255) ||
             this._red != (int)(this._selectedColor.R * 255);
    }

    /// <summary>
    ///   Check if the selected (HSL) color has changed.
    /// </summary>
    /// <returns></returns>
    private bool HasChangedHSL()
    {
      return this._luminosity != (int)(this._selectedColor.Luminosity * 100) ||
             this._saturation != (int)(this._selectedColor.Saturation * 100) ||
             this._hue != (int)(this._selectedColor.Hue * 360);
    }

    /// <summary>
    ///   Check if the selected (Alpha) color has changed.
    /// </summary>
    /// <returns></returns>
    private bool HasChangedAlpha()
    {
      return this._alpha != (int)(this._selectedColor.A * 255);
    }

    /// <summary>
    ///   Refresh gui state.
    /// </summary>
    public void Refresh()
    {
      this._colorLine.Configure(this._lockedAttribute, this._selectedColor);
      this._colorSquare.Configure(this._lockedAttribute, this._selectedColor);

      this.ResetFields();
    }

    /// <summary>
    ///   Capture clicks.
    /// </summary>
    public void Update()
    {
      if (Input.GetMouseButton(0))
      {
        Vector2 location = new Vector2(
          Input.mousePosition.x,
          Screen.height - Input.mousePosition.y
        );

        this.WindowHandleClick(location);
      }
    }

    /// <summary>
    ///   On window click
    /// </summary>
    /// <param name="location"></param>
    private void WindowHandleClick(Vector2 location)
    {
      if(this._windowBounds.Contains(location))
      {
        location.x -= this._windowBounds.x;
        location.y -= this._windowBounds.y;

        this.ColorLineHandleClick(location);
        this.ColorSquareHandleClick(location);
      }
    }

    /// <summary>
    ///   On color line click
    /// </summary>
    /// <param name="location"></param>
    private void ColorLineHandleClick(Vector2 location)
    {
      if (this._colorLineBounds.Contains(location))
      {
        location.x -= this._colorSquareBounds.x;
        location.y -= this._colorLineBounds.y; 
        this._colorSquare.GetColorAt(
           (int) location.x,
           this._colorSquare.height - (int)location.y,
           this._selectedColor
         );
        this.Refresh();
       }
    }

    /// <summary>
    ///   On square click.
    /// </summary>
    /// <param name="location"></param>
    private void ColorSquareHandleClick(Vector2 location)
    {
      if (this._colorSquareBounds.Contains(location))
      {
        location.x -= this._colorSquareBounds.x;
        location.y -= this._colorSquareBounds.y;
        this._colorSquare.GetColorAt(
          (int)location.x,
          this._colorSquare.height - (int)location.y,
          this._selectedColor
        );
        this.Refresh();
      }
    }
    #endregion
  }
}
