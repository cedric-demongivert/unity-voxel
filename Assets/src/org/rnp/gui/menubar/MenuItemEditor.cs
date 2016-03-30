using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace org.rnp.gui.menubar
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Base class for menu item editors.
  /// </summary>
  public class MenuItemEditor : Editor
  {
    /// <see cref="http://docs.unity3d.com/ScriptReference/ScriptableObject.html"/>
    public void OnEnable()
    {
      EditorApplication.hierarchyWindowChanged += HierarchyChanged;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/ScriptableObject.html"/>
    public void OnDisable()
    {
      EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
    }

    /// <summary>
    ///   Return, if exist, the first MenuItem that hold a node.
    /// </summary>
    /// <param name="baseNode"></param>
    /// <returns></returns>
    protected MenuItem FindParentInHierarchy(GameObject baseNode)
    {
      Transform parent = baseNode.transform.parent;

      if (parent != null)
      {
        GameObject parentObject = parent.gameObject;
        MenuItem parentItem = parentObject.GetComponent<MenuItem>();

        if (parentItem != null)
        {
          return parentItem;
        }
        else
        {
          return this.FindParentInHierarchy(parentObject);
        }
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    ///   Launched when the Unity Entity location change in the Inspector hierarchy.
    /// </summary>
    protected void HierarchyChanged()
    {
      MenuItem target = this.target as MenuItem;
      MenuItem containerItem = this.FindParentInHierarchy(target.gameObject);

      // Has a valid parent
      if (containerItem != null && containerItem is ParentMenuItem)
      {
        ParentMenuItem parentItem = containerItem as ParentMenuItem;

        if (target.transform.parent.gameObject == parentItem.ItemsContainer)
        {
          if (target.Parent != parentItem)
          {
            parentItem.AddMenuItem(target.transform.GetSiblingIndex(), target);
          }
          else
          {
            parentItem.MoveMenuItem(target, target.transform.GetSiblingIndex());
          }
        }
        else
        {
          parentItem.AddMenuItem(target);
        }
      }
      // Invalid case
      else if (target.Parent != null)
      {
        target.Parent = null;
        target.transform.SetParent(null);
      }
    }
  }
}
