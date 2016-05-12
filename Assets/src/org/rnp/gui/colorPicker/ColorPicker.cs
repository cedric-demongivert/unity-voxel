using org.rnp.voxel.unity.gui;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace org.rnp.gui.colorPicker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  /// 
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(RectTransform))]
  public class ColorPicker : MonoBehaviour
  {
    [SerializeField]
    private VoxelColor _pickedColor = new VoxelColor();

    [SerializeField]
    private SquareColorPicker _squarePicker;

    [SerializeField]
    private LineColorPicker _linePicker;

    [SerializeField]
    private GUIColorPicker.ColorAttr _lockedAttribute = GUIColorPicker.ColorAttr.Red;

    public GUIColorPicker.ColorAttr LockedAttribute
    {
      get
      {
        return this._lockedAttribute;
      }
      set
      {
        this._lockedAttribute = value;
        this.DispatchLockedAttribute();
      }
    }

    public SquareColorPicker SquarePicker
    {
      get
      {
        return this._squarePicker;
      }
      set
      {
        if (value == this._squarePicker) return;

        SquareColorPicker old = this._squarePicker;
        this._squarePicker = null;

        if(old != null) old.ParentPicker = null;

        this._squarePicker = value;
        value.ParentPicker = this;

        this.Dispatch();
      }
    }

    public LineColorPicker LinePicker
    {
      get
      {
        return this._linePicker;
      }
      set
      {
        if (value == this._linePicker) return;

        LineColorPicker old = this._linePicker;
        this._linePicker = null;

        if (old != null) old.ParentPicker = null;

        this._linePicker = value;
        value.ParentPicker = this;

        this.Dispatch();
      }
    }

    public VoxelColor PickedColor {
      get
      {
        return this._pickedColor;
      }
      set
      {
        this._pickedColor = value;
        this.DispatchColor();
      }
    }

    private void Dispatch()
    {
      this.DispatchColor();
      this.DispatchLockedAttribute();
    }

    private void DispatchLockedAttribute()
    {
      if (this._linePicker != null)
      {
        this._linePicker.LockedAttribute = this.LockedAttribute;
      }

      if (this._squarePicker != null)
      {
        this._squarePicker.LockedAttribute = this.LockedAttribute;
      }
    }

    private void DispatchColor()
    {
      if(this._linePicker != null)
      {
        this._linePicker.PickedColor = this.PickedColor;
      }

      if (this._squarePicker != null)
      {
        this._squarePicker.PickedColor = this.PickedColor;
      }
    }
  }
}
