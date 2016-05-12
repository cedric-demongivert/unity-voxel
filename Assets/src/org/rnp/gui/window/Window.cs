using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace org.rnp.gui.window
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  /// 
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof(RectTransform))]
  public class Window : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    #region Fields
    /// <summary>
    ///   GUI Location Manager
    /// </summary>
    private RectTransform _transform;
    private RectTransform _canvasTransform;

    private bool _dragging;

    private Vector2 _pointerOffset;

    [SerializeField]
    private bool _minimized;

    [SerializeField]
    private Vector2 _size;

    [SerializeField]
    private RectTransform _draggableArea;

    [SerializeField]
    private Text _windowTitleGUI;

    [SerializeField]
    private RectTransform _titleContainer;
        
    [SerializeField]
    private RectTransform _bodyContainer;

    [SerializeField]
    private RectTransform _minimizerContainer;

    public bool IsDraggable;

    public int BottomOffset;

    [SerializeField]
    private bool _isMinimizable;
    #endregion

    #region Getters & Setters
    public bool IsMinimizable
    {
      get
      {
        return this._isMinimizable;
      }
      set
      {
        this._isMinimizable = value;
        this.UpdateSize();
        this.UpdateMinimizer();
      }
    }

    public Text WindowTitleGUI
    {
      get
      {
        return this._windowTitleGUI;
      }
      set
      {
        this._windowTitleGUI = value;
      }
    }

    public RectTransform MinimizerContainer
    {
      get
      {
        return this._minimizerContainer;
      }
      set
      {
        this._minimizerContainer = value;
        this.UpdateMinimizer();
      }
    }

    public RectTransform TitleContainer
    {
      get
      {
        return this._titleContainer;
      }
      set
      {
        this._titleContainer = value;
        this.UpdateSize();
      }
    }

    public RectTransform BodyContainer
    {
      get
      {
        return this._bodyContainer;
      }
      set
      {
        this._bodyContainer = value;
        this.UpdateSize();
      }
    }

    public bool Minimized
    {
      get
      {
        return this._minimized && this.IsMinimizable;
      }
      set
      {
        if(this.IsMinimizable)
        {
          this._minimized = value;
        }
        else
        {
          this._minimized = false;
        }
        this.UpdateSize();
      }
    }
    
    /// <summary>
    ///   Change the area that trigger the drag of the window.
    /// </summary>
    public RectTransform DraggableArea
    {
      get
      {
        if(this._draggableArea == null)
        {
          return this._transform;
        }
        else
        {
          return this._draggableArea;
        }
      }
      set
      {
        this._draggableArea = value;
      }
    }

    /// <summary>
    ///   Change the MenuItem title.
    /// </summary>
    public String WindowTitle
    {
      get
      {
        if (this._windowTitleGUI != null)
        {
          return this._windowTitleGUI.text;
        }
        else
        {
          return null;
        }
      }
      set
      {
        if (this._windowTitleGUI != null)
        {
          this._windowTitleGUI.text = value;
        }

        this.RefreshGameObjectName();
      }
    }

    /// <summary>
    ///   Get the position of this window on the screen.
    /// </summary>
    public Vector2 ScreenPosition
    {
      get
      {
        return this._transform.position;
      }
      set
      {
        this._transform.position = value;
      }
    }

    /// <summary>
    ///   Get the size of this window.
    /// </summary>
    public Vector2 Size
    {
      get
      {
        return this._size;
      }
      set
      {
        this._size = value;
        this.UpdateSize();
      }
    }
    #endregion

    #region Methods
    private void UpdateMinimizer()
    {
      if(this._minimizerContainer != null)
      {
        this._minimizerContainer.gameObject.SetActive(this.IsMinimizable);
      }
    }

    /// <summary>
    ///   Change the GameObject name according to this component.
    /// </summary>
    private void RefreshGameObjectName()
    {
      this.gameObject.name = "[Window] " + this.WindowTitle;
    }
    
    public void Toggle()
    {
      this.Minimized = !this.Minimized;
    }

    public void Minimize()
    {
      this.Minimized = true;
    }

    public void Minimize(bool value)
    {
      this.Minimized = value;
    }
    
    public void Maximize()
    {
      this.Minimized = false;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void Awake()
    {
      this._transform = this.gameObject.GetComponent<RectTransform>();

      Canvas canvas = GetComponentInParent<Canvas>();

      if (canvas != null)
      {
        _canvasTransform = canvas.transform as RectTransform;
      }

      this._dragging = false;

      this.RefreshGameObjectName();
      this.UpdateSize();
      this.UpdateMinimizer();
    }

    private void UpdateSize()
    {
      if(this.Minimized)
      {
        this._transform.sizeDelta = new Vector2(
          this._size.x,
          (this.TitleContainer == null) ? 25 + BottomOffset : this.TitleContainer.sizeDelta.y + BottomOffset
        );
      }
      else
      {
        this._transform.sizeDelta = this._size;
      }

      if (this.BodyContainer != null)
      {
        this.BodyContainer.gameObject.SetActive(!this.Minimized);
      }
    }

    /// <see cref="http://docs.unity3d.com/Documentation/ScriptReference/EventSystems.IPointerDownHandler.html"/>
    public void OnPointerDown(PointerEventData data)
    {
      if(!this._dragging)
      {
        this._dragging = RectTransformUtility.RectangleContainsScreenPoint(this.DraggableArea, data.position, data.pressEventCamera);
      }

      this._transform.SetAsLastSibling();
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this._transform, data.position, data.pressEventCamera, out _pointerOffset);
    }

    /// <see cref="http://docs.unity3d.com/Documentation/ScriptReference/EventSystems.IDragHandler.html"/>
    public void OnDrag(PointerEventData data)
    {
      if (this._transform == null || !this._dragging)
        return;

      Vector2 pointerPostion = ClampToWindow(data);

      Vector2 localPointerPosition;
      
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
          _canvasTransform, pointerPostion, data.pressEventCamera, out localPointerPosition
      ))
      {
        _transform.localPosition = localPointerPosition - _pointerOffset;
      }
    }

    private Vector2 ClampToWindow(PointerEventData data)
    {
      Vector2 rawPointerPosition = data.position;

      Vector3[] canvasCorners = new Vector3[4];
      _canvasTransform.GetWorldCorners(canvasCorners);

      float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
      float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

      Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
      return newPointerPosition;
    }

    /// <see cref="http://docs.unity3d.com/Documentation/ScriptReference/EventSystems.IPointerUpHandler.html"/>
    public void OnPointerUp(PointerEventData eventData)
    {
      this._dragging = false;
    }
    #endregion

  }
}
