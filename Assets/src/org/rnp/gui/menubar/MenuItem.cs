using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace org.rnp.gui.menubar
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A generic menu item.
  /// </summary>
  public abstract class MenuItem : MonoBehaviour
  {
    /// <summary>
    ///   Parent item.
    /// </summary>
    [SerializeField]
    private ParentMenuItem _parent;

    /// <summary>
    ///   Change or return the parent of this item.
    /// </summary>
    public ParentMenuItem Parent
    {
      get
      {
        return this.GetParent();
      }
      set
      {
        this.SetParent(value);
      }
    }

    /// <summary>
    ///   Return the parent of this Item.
    /// </summary>
    /// <returns></returns>
    public ParentMenuItem GetParent()
    {
      return this._parent;
    }

    /// <summary>
    ///   Change the parent of this Item.
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(ParentMenuItem newParent)
    {
      if (this._parent != newParent)
      {
        if (this._parent != null)
        {
          ParentMenuItem oldParent = this._parent;
          this._parent = null;
          oldParent.RemoveMenuItem(this);
        }

        this._parent = newParent;

        if (newParent != null)
        {
          newParent.AddMenuItem(this);
          this.transform.SetParent(newParent.ItemsContainer.transform);
        }
        else
        {
          this.transform.SetParent(null);
        }
      }
    }
  }
}
