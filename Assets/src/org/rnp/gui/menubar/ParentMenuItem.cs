using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.gui.menubar
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A menu item that old menu items...
  /// </summary>
  public abstract class ParentMenuItem : MenuItem
  {
    /// <summary>
    ///   Sub menu-items.
    /// </summary>
    [SerializeField]
    private List<MenuItem> _items = new List<MenuItem>();

    /// <summary>
    ///   Game object that old sub-items.
    /// </summary>
    public GameObject ItemsContainer
    {
      get
      {
        Transform transform = this.transform.FindChild("Items");

        if (transform == null)
        {
          GameObject itemsContainer = new GameObject("Items");
          itemsContainer.transform.SetParent(this.transform);
          return itemsContainer;
        }
        else
        {
          return transform.gameObject;
        }
      }
    }

    /// <see cref="org.rnp.gui.menubar.IMenuItemParent"></see>
    public MenuItem this[int numero]
    {
      get
      {
        return this._items[numero];
      }
      set
      {
        this.RemoveMenuItem(numero);
        this.AddMenuItem(numero, value);
      }
    }

    /// <see cref="org.rnp.gui.menubar.IMenuItemParent"></see>
    public int Count
    {
      get
      {
        return this._items.Count;
      }
    }

    /// <summary>
    ///   Add a child at the end.
    /// </summary>
    /// <param name="item"></param>
    public virtual void AddMenuItem(MenuItem item)
    {
      if (!this._items.Contains(item))
      {
        this._items.Add(item);
        item.Parent = this;
      }
    }

    /// <summary>
    ///   Insert a child at a specific location.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="item"></param>
    public virtual void AddMenuItem(int position, MenuItem item)
    {
      if (!this._items.Contains(item))
      {
        this._items.Insert(position, item);
        item.Parent = this;
        item.transform.SetSiblingIndex(position);
      }
    }

    /// <summary>
    ///   Rearange child order.
    /// </summary>
    /// <param name="initial"></param>
    /// <param name="final"></param>
    public virtual void MoveMenuItem(int initial, int final)
    {
      MenuItem item = this._items[initial];
      this._items.RemoveAt(initial);
      this._items.Insert(final, item);
      item.transform.SetSiblingIndex(final);
    }

    /// <summary>
    ///   Rearange child order.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="final"></param>
    public virtual void MoveMenuItem(MenuItem item, int final)
    {
      if (this._items.Contains(item))
      {
        this._items.Remove(item);
        this._items.Insert(final, item);
        item.transform.SetSiblingIndex(final);
      }
    }

    /// <summary>
    ///   Remove a child.
    /// </summary>
    /// <param name="item"></param>
    public virtual void RemoveMenuItem(MenuItem item)
    {
      if (this._items.Contains(item))
      {
        this._items.Remove(item);
        item.Parent = null;
      }
    }

    /// <summary>
    ///   Commit a new child order to the view.
    /// </summary>
    public void RefreshChildOrder()
    {
      for (int index = 0; index < this._items.Count; ++index)
      {
        this._items[index].transform.SetSiblingIndex(index);
      }
    }

    /// <summary>
    ///   Remove a menu item.
    /// </summary>
    /// <param name="numero"></param>
    public virtual void RemoveMenuItem(int numero)
    {
      MenuItem item = this._items[numero];

      this._items.RemoveAt(numero);
      item.Parent = null;
    }

    /// <summary>
    ///   Close the menu item container
    /// </summary>
    public virtual void Close()
    {
      if(this.Parent != null)
      {
        this.Parent.Close();
      }
    }

    /// <summary>
    ///   Open the menu item container
    /// </summary>
    public virtual void Open()
    {
      if (this.Parent != null)
      {
        this.Parent.Open();
      }
    }

  }
}
