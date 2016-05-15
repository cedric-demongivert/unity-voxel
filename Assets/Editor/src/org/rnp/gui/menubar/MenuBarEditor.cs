using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace org.rnp.gui.menubar
{
  [CustomEditor(typeof(MenuBar))]
  public class MenuBarEditor : Editor
  {
    /// <summary>
    ///   The edited MenuBar.
    /// </summary>
    public MenuBar TargetMenuBar
    {
      get
      {
        return this.target as MenuBar;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);

      this.RegisterNewItems();
      this.ManageRegisteredItems();

      EditorGUILayout.EndVertical();
    }

    /// <summary>
    ///   GUI for managing current menu items.
    /// </summary>
    private void ManageRegisteredItems()
    {
      EditorGUILayout.LabelField("Registered Menu Items");

      for (int i = 0; i < this.TargetMenuBar.Count; ++i)
      {
        MenuItem registered = this.TargetMenuBar[i];

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i < this.TargetMenuBar.Count - 1)
          {
            this.TargetMenuBar.MoveMenuItem(i, i + 1);
          }
        }

        if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i > 0)
          {
            this.TargetMenuBar.MoveMenuItem(i, i - 1);
          }
        }

        if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80)))
        {
          this.TargetMenuBar.RemoveMenuItem(registered);
        }
        else
        {
          EditorGUILayout.LabelField(registered.gameObject.name);
        }

        EditorGUILayout.EndHorizontal();
      }
    }

    /// <summary>
    ///   GUI for registering new menu items.
    /// </summary>
    private void RegisterNewItems()
    {
      EditorGUILayout.LabelField("Register an existing Menu Item");

      MenuItem toRegister = EditorGUILayout.ObjectField(
        GUIContent.none, null, typeof(MenuItem), true
      ) as MenuItem;

      if (toRegister != null)
      {
        this.TargetMenuBar.AddMenuItem(toRegister);
      }
    }
  }
}
