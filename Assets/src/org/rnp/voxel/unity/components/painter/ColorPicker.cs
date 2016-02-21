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
  [ExecuteInEditMode]
  public class ColorPicker : MonoBehaviour
  {
    #region Fields
    /// <summary>
    ///   User selected color.
    /// </summary>
    private VoxelColor _selectedColor = new VoxelColor();
    private VoxelColor _lastColor = new VoxelColor();

    public GUIColorPicker.ColorMode Mode = GUIColorPicker.ColorMode.RGB;
    private GUIColorPicker.ColorMode _oldMode = GUIColorPicker.ColorMode.RGB;

    public GUIColorPicker.ColorAttr Locked = GUIColorPicker.ColorAttr.Red;
    private GUIColorPicker.ColorAttr _oldLocked = GUIColorPicker.ColorAttr.Red;

    private Texture2D _colorSquare;
    private Texture2D _colorLine;

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

    #region Strings
    private String _rStr = null;
    private String _gStr = null;
    private String _bStr = null;
    private String _aStr = null;
    private String _hStr = null;
    private String _sStr = null;
    private String _lStr = null;
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
        this._selectedColor = value;
        this.RefreshTextures();
        this.ResetFields();
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
      GUI.DrawTexture(this._colorSquareBounds, this._colorSquare);
      GUI.DrawTexture(this._colorLineBounds, this._colorLine);

      // Fields
      GUILayout.BeginArea(this._fieldsBounds);
        GUILayout.BeginVertical();

          // RGB
          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Red);
            this._selectedColor.R = GUIColorPicker.LayoutColorField(this._rStr, this._selectedColor.R, "R", out this._rStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Green);
            this._selectedColor.G = GUIColorPicker.LayoutColorField(this._gStr, this._selectedColor.G, "G", out this._gStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Blue);
            this._selectedColor.B = GUIColorPicker.LayoutColorField(this._bStr, this._selectedColor.B, "B", out this._bStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // HSL
          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Hue);
            this._selectedColor.Hue = GUIColorPicker.LayoutColorField(this._hStr, this._selectedColor.Hue, "H", out this._hStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Saturation);
            this._selectedColor.Saturation = GUIColorPicker.LayoutColorField(this._sStr, this._selectedColor.Saturation, "S", out this._sStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.Locked = GUIColorPicker.LayoutAttrChkbox(this.Locked, GUIColorPicker.ColorAttr.Luminosity);
            this._selectedColor.Luminosity = GUIColorPicker.LayoutColorField(this._lStr, this._selectedColor.Luminosity, "L", out this._lStr);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // Alpha
          GUILayout.BeginHorizontal();
            GUILayout.Space(this._toggleSize);
            this._selectedColor.A = GUIColorPicker.LayoutColorField(this._aStr, this._selectedColor.A, "A", out this._aStr);
          GUILayout.EndHorizontal();

          // #
          GUILayout.BeginHorizontal();
          GUILayout.Space(this._toggleSize);
          this._selectedColor = GUIColorPicker.LayoutHexaField(this._selectedColor, "#");
          GUILayout.EndHorizontal();

        GUILayout.EndVertical();
      GUILayout.EndArea();

      if(!this._selectedColor.Equals(this._lastColor))
      {
        this.RefreshTextures();
      }

      this.Mode = GUIColorPicker.GetModeOf(this.Locked);
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
    public void Awake()
    {
      this._colorSquare = new Texture2D(255, 255);
      this._colorLine = new Texture2D(20, 255);

      this.RefreshTextures();

      this._oldMode = this.Mode;
      this._oldLocked = this.Locked;

      this.Layout();
    }

    public void ResetFields()
    {
      this._aStr = this._bStr = this._gStr = this._rStr = this._hStr = this._lStr = this._sStr = null;
    }

    public void RefreshTextures()
    {
      GUIColorPicker.ColorLine(
        this._colorLine,
        this._selectedColor,
        this.Mode, 
        this.Locked
      );

      GUIColorPicker.ColorSquare(
        this._colorSquare,
        this._selectedColor,
        this.Mode,
        this.Locked
      );

      this._colorSquare.Apply();
      this._colorLine.Apply();
    }

    public void Update()
    {
      if (this._oldMode != this.Mode || this._oldLocked != this.Locked)
      {
        this.RefreshTextures();

        this._oldMode = this.Mode;
        this._oldLocked = this.Locked;
      }

      if (Input.GetMouseButton(0))
      {
        Vector2 location = new Vector2(
          Input.mousePosition.x,
          Screen.height - Input.mousePosition.y
        );

        this.WindowHandleClick(location);
      }
    }

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

    private void ColorLineHandleClick(Vector2 location)
    {
      if (this._colorLineBounds.Contains(location))
      {
        location.y -= this._colorLineBounds.y;
        Color tmp = this._colorLine.GetPixel(0, this._colorLine.height - (int)location.y);
        tmp.a = this.SelectedColor.a;
        this.SelectedColor = tmp;
      }
    }

    private void ColorSquareHandleClick(Vector2 location)
    {
      if (this._colorSquareBounds.Contains(location))
      {
        location.x -= this._colorSquareBounds.x;
        location.y -= this._colorSquareBounds.y;
        Color tmp = this._colorSquare.GetPixel((int)location.x, this._colorSquare.height - (int)location.y);
        tmp.a = this.SelectedColor.a;
        this.SelectedColor = tmp;
      }
    }
    #endregion
  }
}
