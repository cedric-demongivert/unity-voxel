using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A Generic tool of the painter
  /// </summary>
  public abstract class PainterTool : MonoBehaviour
  {
    [SerializeField]
    private Painter _parent;

    public Painter Parent
    {
      get
      {
        return this._parent;
      }
      set
      {
        if(this._parent != value)
        {
          Painter oldParent = this._parent;
          this._parent = null;

          if(oldParent != null)
          {
            oldParent.UnregisterTool(this);
          }

          this._parent = value;

          if(this._parent != null)
          {
            this._parent.RegisterTool(this);
          }
        }
      }
    }

    public abstract void OnToolStart();

    public abstract void OnToolUpdate();

    public abstract void OnToolEnd();
  }
}
