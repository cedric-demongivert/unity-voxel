﻿using System;
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
    
    [SerializeField]
    private GUIColorPicker.ColorAttr _lockedAttribute = GUIColorPicker.ColorAttr.Red;

    private SquareColorPickerTexture _colorSquare;
    private LineColorPickerTexture _colorLine;

    private ColorPickerView _view;

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
          this.RefreshPickers();
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
        this.RefreshPickers();
        this._view.PullModel(value);
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
            this._view.Red = GUIColorPicker.LayoutColorField("R", this._view.Red);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Green);
            this._view.Green = GUIColorPicker.LayoutColorField("G", this._view.Green);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Blue);
            this._view.Blue = GUIColorPicker.LayoutColorField("B", this._view.Blue);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // HSL
          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Hue);
            this._view.Hue = GUIColorPicker.LayoutColorField("H", this._view.Hue);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Saturation);
            this._view.Saturation = GUIColorPicker.LayoutColorField("S", this._view.Saturation);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            this.LockedAttribute = GUIColorPicker.LayoutAttrChkbox(this._lockedAttribute, GUIColorPicker.ColorAttr.Luminosity);
            this._view.Luminosity = GUIColorPicker.LayoutColorField("L", this._view.Luminosity);
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Height(10));
          GUILayout.EndHorizontal();

          // Alpha
          GUILayout.BeginHorizontal();
            GUILayout.Space(this._toggleSize);
            this._view.Alpha = GUIColorPicker.LayoutColorField("A", this._view.Alpha);
          GUILayout.EndHorizontal();

          // #
          GUILayout.BeginHorizontal();
            GUILayout.Space(this._toggleSize);
            this._view.Hex = GUIColorPicker.LayoutColorField("#", this._view.Hex);
          GUILayout.EndHorizontal();

        GUILayout.EndVertical();
      GUILayout.EndArea();

      if(this._view.IsDirty)
      {
        this._view.CommitView(this._selectedColor);
        this.RefreshPickers();
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
      this._view = new ColorPickerView();

      this.Layout();
      this.RefreshPickers();
      this._view.PullModel(this._selectedColor);
    }

    /// <summary>
    ///   Refresh gui state.
    /// </summary>
    private void RefreshPickers()
    {
      this._colorLine.Configure(this._lockedAttribute, this._selectedColor);
      this._colorSquare.Configure(this._lockedAttribute, this._selectedColor);
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

        this._colorLine.GetColorAt(
           (int) location.x,
           this._colorSquare.height - (int)location.y,
           this._selectedColor
         );
        this.RefreshPickers();
        this._view.PullModel(this._selectedColor);
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
        this.RefreshPickers();
        this._view.PullModel(this._selectedColor);
      }
    }
    #endregion
  }
}
