using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using org.rnp.gui.colorPicker;

namespace org.rnp.gui.colorPalette
{
  [CustomEditor(typeof(PainterColorPalette))]
  public class PainterColorPaletteEditor : ColorPaletteEditor
  {
    /// <summary>
    ///   Get the edited menu item.
    /// </summary>
    public PainterColorPalette TargetPalette
    {
      get
      {
        return this.target as PainterColorPalette;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);

      this.TargetPalette.Picker = EditorGUILayout.ObjectField(
        "Color Picker", this.TargetPalette.Picker, typeof(ColorPicker), true
      ) as ColorPicker;
    }
  }
}
