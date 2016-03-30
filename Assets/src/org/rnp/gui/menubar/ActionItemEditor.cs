using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace org.rnp.gui.menubar
{
  [CustomEditor(typeof(ActionItem))]
  public class ActionItemEditor : MenuItemEditor
  {

    /// <summary>
    ///   Get the edited menu item.
    /// </summary>
    public ActionItem TargetMenuItem
    {
      get
      {
        return this.target as ActionItem;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.BeginVertical();

      this.EditGUIParts();
      this.EditMenuItem();
      this.EditEvent();

      EditorGUILayout.EndVertical();
      this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    ///   GUI for managing actions.
    /// </summary>
    private void EditEvent()
    {
      EditorGUILayout.LabelField("Action", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnAction"), true);
    }

    /// <summary>
    ///   GUI for managing the Menu Item.
    /// </summary>
    private void EditMenuItem()
    {
      EditorGUILayout.LabelField("Menu Item", EditorStyles.boldLabel);

      if (this.TargetMenuItem.GUILabel != null)
      {
        this.TargetMenuItem.Title = EditorGUILayout.TextField("Title", this.TargetMenuItem.Title);
      }
      else
      {
        EditorGUILayout.LabelField("Specify a Label Element in order to set a Menu Item title.");
      }

      if (this.TargetMenuItem.GUIIcon != null)
      {
        this.TargetMenuItem.Icon = EditorGUILayout.ObjectField(
          "Icon", this.TargetMenuItem.Icon, typeof(Sprite)
        ) as Sprite;
      }
      else
      {
        EditorGUILayout.LabelField("Specify a Icon Element in order to set a Menu Item icon.");
      }

      this.TargetMenuItem.Disabled = EditorGUILayout.Toggle("Disabled", this.TargetMenuItem.Disabled);
      
      this.TargetMenuItem.Parent = EditorGUILayout.ObjectField(
        "Parent", this.TargetMenuItem.Parent, typeof(ParentMenuItem), true
      ) as ParentMenuItem;
    }

    /// <summary>
    ///   GUI for managing GUI Parts.
    /// </summary>
    private void EditGUIParts()
    {
      EditorGUILayout.LabelField("GUI Parts", EditorStyles.boldLabel);

      this.TargetMenuItem.GUILabel = EditorGUILayout.ObjectField(
        "Label Element", this.TargetMenuItem.GUILabel, typeof(Text)
      ) as Text;

      this.TargetMenuItem.GUIIcon = EditorGUILayout.ObjectField(
        "Icon Element", this.TargetMenuItem.GUIIcon, typeof(Image)
      ) as Image;
    }
  }
}
