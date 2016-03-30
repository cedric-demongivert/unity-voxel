using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace org.rnp.gui.menubar
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An item that do something on click.
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(Selectable))]
  public class ActionItem : MenuItem, IPointerUpHandler
  {
    [Serializable]
    public class ActionEvent : UnityEvent
    { }

    #region Fields
    /// <summary>
    ///   Event listener.
    /// </summary>
    public ActionEvent OnAction;

    private Selectable _selectable;
    
    /// <summary>
    ///   GUI Component to display MenuItem label.
    /// </summary>
    [SerializeField]
    private Text _guiLabel;

    [SerializeField]
    /// <summary>
    ///   GUI Component to display MenuItem icon.
    /// </summary>
    private Image _guiIcon;
    #endregion

    #region Getters & Setters
    /// <summary>
    ///   Disable or enable the menu item.
    /// </summary>
    public bool Disabled
    {
      get
      {
        return !this._selectable.interactable;
      }
      set
      {
        this._selectable.interactable = !value;
      }
    }

    /// <summary>
    ///   GUI Component to display MenuItem label.
    /// </summary>
    public Text GUILabel
    {
      get
      {
        return this._guiLabel;
      }
      set
      {
        this._guiLabel = value;
        this.RefreshGameObjectName();
      }
    }

    /// <summary>
    ///   GUI Component to display MenuItem icon.
    /// </summary>
    public Image GUIIcon
    {
      get
      {
        return this._guiIcon;
      }
      set
      {
        this._guiIcon = value;
        this.RefreshGUIIconState();
      }
    }

    /// <summary>
    ///   Change the MenuItem title.
    /// </summary>
    public String Title
    {
      get
      {
        if (this.GUILabel != null)
        {
          return this.GUILabel.text;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (this.GUILabel != null)
        {
          this.GUILabel.text = value;
        }

        this.RefreshGameObjectName();
      }
    }

    /// <summary>
    ///   Change the menu item Icon.
    /// </summary>
    public Sprite Icon
    {
      get
      {
        if (this.GUIIcon != null)
        {
          return this.GUIIcon.sprite;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (this.GUIIcon != null)
        {
          this.GUIIcon.sprite = value;
        }

        this.RefreshGUIIconState();
      }
    }
    #endregion

    /// <summary>
    ///   Do registered action.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
      this.OnAction.Invoke();
    }

    /// <summary>
    ///   Change the GameObject name according to this component.
    /// </summary>
    private void RefreshGameObjectName()
    {
      this.gameObject.name = "[Action Item] " + this.Title;
    }

    /// <summary>
    ///   Disable or enable the GUIIcon GameObject.
    /// </summary>
    private void RefreshGUIIconState()
    {
      if (this._guiIcon != null)
      {
        this._guiIcon.gameObject.active = (this.Icon != null);
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      if (this.Parent != null)
      {
        this.Parent.AddMenuItem(this);
      }
      
      this.RefreshGameObjectName();
      this._selectable = this.gameObject.GetComponent<Selectable>();
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void OnDestroy()
    {
      this.Parent = null;
    }
  }
}
