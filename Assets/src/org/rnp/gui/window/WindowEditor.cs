using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace org.rnp.gui.window
{
  [CustomEditor(typeof(Window))]
  public class WindowEditor : Editor
  {
    /// <summary>
    ///   The edited Window.
    /// </summary>
    public Window TargetWindow
    {
      get
      {
        return this.target as Window;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.EditGuiParts();
      this.EditWindow();
      this.EditOptions();
     
      EditorGUILayout.EndVertical();
    }

    private void EditGuiParts()
    {
      EditorGUILayout.LabelField("GUI Parts", EditorStyles.boldLabel);

      this.TargetWindow.TitleContainer = EditorGUILayout.ObjectField("Title Container", this.TargetWindow.TitleContainer, typeof(RectTransform), true) as RectTransform;
      this.TargetWindow.WindowTitleGUI = EditorGUILayout.ObjectField("Title Label", this.TargetWindow.WindowTitleGUI, typeof(Text), true) as Text;
      this.TargetWindow.BodyContainer = EditorGUILayout.ObjectField("Body Container", this.TargetWindow.BodyContainer, typeof(RectTransform), true) as RectTransform;
      this.TargetWindow.MinimizerContainer = EditorGUILayout.ObjectField("Minimizer Container", this.TargetWindow.MinimizerContainer, typeof(RectTransform), true) as RectTransform;
    }

    private void EditOptions()
    {
      EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);

      this.TargetWindow.IsDraggable = EditorGUILayout.Toggle("Is Draggable", this.TargetWindow.IsDraggable);
      this.TargetWindow.IsMinimizable = EditorGUILayout.Toggle("Is Minimizable", this.TargetWindow.IsMinimizable);
    }

    private void EditWindow()
    {
      EditorGUILayout.LabelField("Window", EditorStyles.boldLabel);

      this.TargetWindow.WindowTitle = EditorGUILayout.TextField("Title", this.TargetWindow.WindowTitle);

      if (this.TargetWindow.IsDraggable)
      {
        this.TargetWindow.DraggableArea = EditorGUILayout.ObjectField("Draggable Area", this.TargetWindow.DraggableArea, typeof(RectTransform), true) as RectTransform;
      }

      if (this.TargetWindow.IsMinimizable)
      {
        this.TargetWindow.Minimized = EditorGUILayout.Toggle("Minimized", this.TargetWindow.Minimized);
      }

      this.TargetWindow.ScreenPosition = EditorGUILayout.Vector2Field("Position", this.TargetWindow.ScreenPosition);
      this.TargetWindow.Size = EditorGUILayout.Vector2Field("Size", this.TargetWindow.Size);
      this.TargetWindow.BottomOffset = EditorGUILayout.IntField("Bottom Offset", this.TargetWindow.BottomOffset);
    }
  }
}
