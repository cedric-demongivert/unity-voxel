using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace org.rnp.gui.menubar
{
  [CustomEditor(typeof(SubMenuItem))]
  public class SubMenuEditor : MenuItemEditor
  {
    /// <summary>
    ///   The edited MenuBar.
    /// </summary>
    public SubMenuItem TargetMenuItem
    {
      get
      {
        return this.target as SubMenuItem;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();
      
      this.EditGUIParts();
      this.EditMenuItem();

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

      for (int i = 0; i < this.TargetMenuItem.Count; ++i)
      {
        MenuItem registered = this.TargetMenuItem[i];

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i < this.TargetMenuItem.Count - 1)
          {
            this.TargetMenuItem.MoveMenuItem(i, i + 1);
          }
        }

        if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(25)))
        {
          if (i > 0)
          {
            this.TargetMenuItem.MoveMenuItem(i, i - 1);
          }
        }

        if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80)))
        {
          this.TargetMenuItem.RemoveMenuItem(registered);
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
        null, typeof(MenuItem), true
      ) as MenuItem;

      if (toRegister != null)
      {
        this.TargetMenuItem.AddMenuItem(toRegister);
      }
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
          "Icon", this.TargetMenuItem.Icon, typeof(Texture)
        ) as Texture;
      }
      else
      {
        EditorGUILayout.LabelField("Specify a Icon Element in order to set a Menu Item icon.");
      }

      this.TargetMenuItem.Disabled = EditorGUILayout.Toggle("Disabled", this.TargetMenuItem.Disabled);
      this.TargetMenuItem.Opened = EditorGUILayout.Toggle("Opened", this.TargetMenuItem.Opened);

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
        "Icon Element", this.TargetMenuItem.GUIIcon, typeof(RawImage)
      ) as RawImage;
    }
  }
}
