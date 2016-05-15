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
  [CustomEditor(typeof(ColorPalette))]
  public class ColorPaletteEditor : Editor
  {
    /// <summary>
    ///   Get the edited menu item.
    /// </summary>
    private ColorPalette TargetPalette
    {
      get
      {
        return this.target as ColorPalette;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.EditGUIParts();
      this.EditColors();

      EditorGUILayout.EndVertical();
    }

    private void EditColors()
    {
      EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);

      if(this.TargetPalette.IsEmpty())
      {
        EditorGUILayout.LabelField("The palette is empty");
      }
      else
      {
        foreach (Color32 color in this.TargetPalette.Colors)
        {
          Color32 newColor = EditorGUILayout.ColorField(color);

          if (!newColor.Equals(color))
          {
            this.TargetPalette.ChangeColor(color, newColor);
          }
        }
      }
    }

    /// <summary>
    ///   GUI for managing GUI Parts.
    /// </summary>
    private void EditGUIParts()
    {
      EditorGUILayout.LabelField("GUI Parts", EditorStyles.boldLabel);
      
      this.TargetPalette.ColorItem = EditorGUILayout.ObjectField(
        "Color Item", this.TargetPalette.ColorItem, typeof(GameObject), true
      ) as GameObject;
      
      this.TargetPalette.Palette = EditorGUILayout.ObjectField(
        "Palette Container", this.TargetPalette.Palette, typeof(GameObject), true
      ) as GameObject;
    }
  }
}
