using org.rnp.gui.colorPalette;
using org.rnp.gui.colorPicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace org.rnp.voxel.painter
{
  [CustomEditor(typeof(Painter))]
  public class PainterEditor : Editor
  {
    /// <summary>
    ///   The edited Painter.
    /// </summary>
    public Painter TargetPainter
    {
      get
      {
        return this.target as Painter;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);

      this.TargetPainter.Palette = EditorGUILayout.ObjectField(
        "Color Palette", this.TargetPainter.Palette, typeof(PainterColorPalette), true
      ) as PainterColorPalette;

      this.TargetPainter.Picker = EditorGUILayout.ObjectField(
        "Color Picker", this.TargetPainter.Picker, typeof(ColorPicker), true
      ) as ColorPicker;

      this.TargetPainter.ActiveTool = EditorGUILayout.IntField(
        "Default Active Tool", this.TargetPainter.ActiveTool  
      );

      EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

      this.RegisterNewTools();
      this.ManageRegisteredTools();

      EditorGUILayout.EndVertical();
    }

    /// <summary>
    ///   GUI for managing current menu items.
    /// </summary>
    private void ManageRegisteredTools()
    {
      EditorGUILayout.LabelField("Registered Tools");
      int i = 0;

      if(this.TargetPainter.ToolsCount <= 0)
      {
        EditorGUILayout.LabelField("No tools registered yet");
      }

      foreach(PainterTool registered in this.TargetPainter.Tools)
      {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i < this.TargetPainter.ToolsCount - 1)
          {
            this.TargetPainter.MoveTool(i, i + 1);
          }
        }

        if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i > 0)
          {
            this.TargetPainter.MoveTool(i, i - 1);
          }
        }

        if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80)))
        {
          this.TargetPainter.UnregisterTool(registered);
        }
        else
        {
          EditorGUILayout.LabelField(i + " : " + registered.GetType().Name);
        }

        EditorGUILayout.EndHorizontal();

        i += 1;
      } 
    }

    /// <summary>
    ///   GUI for registering new menu items.
    /// </summary>
    private void RegisterNewTools()
    {
      EditorGUILayout.LabelField("Register new tools");

      PainterTool toRegister = EditorGUILayout.ObjectField(
        GUIContent.none, null, typeof(PainterTool), true
      ) as PainterTool;

      if (toRegister != null)
      {
        this.TargetPainter.RegisterTool(toRegister);
      }
    }
  }
}
