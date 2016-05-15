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
  [CustomEditor(typeof(ColorPaletteItem))]
  public class ColorPaletteItemEditor : Editor
  {
    /// <summary>
    ///   Get the edited menu item.
    /// </summary>
    private ColorPaletteItem TargetItem
    {
      get
      {
        return this.target as ColorPaletteItem;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.EditGUIParts();
      this.EditItem();

      EditorGUILayout.EndVertical();
    }

    private void EditItem()
    {
      EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);

      this.TargetItem.Color = EditorGUILayout.ColorField("Color", this.TargetItem.Color);
    }

    /// <summary>
    ///   GUI for managing GUI Parts.
    /// </summary>
    private void EditGUIParts()
    {
      EditorGUILayout.LabelField("GUI Parts", EditorStyles.boldLabel);
      
      this.TargetItem.ColorImage = EditorGUILayout.ObjectField(
        "Color Renderer", this.TargetItem.ColorImage, typeof(RawImage), true
      ) as RawImage;
    }
  }
}
