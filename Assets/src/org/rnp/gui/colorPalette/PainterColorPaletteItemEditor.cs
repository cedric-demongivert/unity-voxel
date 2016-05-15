using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace org.rnp.gui.colorPalette
{
  [CustomEditor(typeof(PainterColorPaletteItem))]
  public class PainterColorPaletteItemEditor : ColorPaletteItemEditor
  {
    /// <summary>
    ///   Get the edited menu item.
    /// </summary>
    public ColorPaletteItemEditor TargetItem
    {
      get
      {
        return this.target as ColorPaletteItemEditor;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();


    }
  }
}
