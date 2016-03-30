using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Gui for camera bookmarks.
  /// </summary>
  ///[ExecuteInEditMode]
  public class CameraBookmarks : MonoBehaviour
  {
    #region Fields
    private bool _minimized = false;

    #region GUI Window Configuration
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUI.Window.html"/>
    public int WindowId = 26;
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUI.Window.html"/>
    public String WindowName = "Camera";
    public PainterCamera Camera;
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/GUISkin.html"/>
    [SerializeField]
    private GUISkin _skin = null;
    private Rect _draggableArea = new Rect();

    /// <summary>
    ///   Change the picker skin.
    /// </summary>
    public GUISkin Skin
    {
      get
      {
        return this._skin;
      }
      set
      {
        this._skin = value;
        this.Layout();
      }
    }

    public bool Minimized
    {
      get
      {
        return this._minimized;
      }
      set
      {
        if (this._minimized != value)
        {
          this._minimized = value;
          this.Layout();
        }
      }
    }
    #endregion

    #region Layout
    private Rect _windowBounds = new Rect();
    private Rect _fieldsBounds = new Rect();
    private Rect _minimizeToggleBounds = new Rect();
    #endregion

    #endregion
    
    #region GUI
    public void OnGUI()
    {
      GUI.skin = this._skin;

      this._windowBounds = GUI.Window(
        this.WindowId,
        this._windowBounds,
        this.BookmarksWindow,
        new GUIContent()
      );

      GUI.skin = null;
    }

    protected void BookmarksWindow(int windowId)
    {
      this.WindowId = windowId;

      GUI.DragWindow(this._draggableArea);

      // Title
      if (this._skin == null || this._skin.FindStyle("Window Title") == null)
      {
        GUI.Label(this._draggableArea, this.WindowName);
      }
      else
      {
        GUI.Label(this._draggableArea, this.WindowName, this._skin.FindStyle("Window Title"));
      }

      this.Minimized = GUI.Toggle(this._minimizeToggleBounds, this._minimized, GUIContent.none);

      if (this._minimized) return;
      
      // Fields
      GUILayout.BeginArea(this._fieldsBounds);
        GUILayout.BeginVertical();

        if (GUILayout.Button("Front"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = Vector3.zero;
        }

        if (GUILayout.Button("Back"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(180, 0, 0);
        }

        if (GUILayout.Button("Top"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(0, 90, 0);
        }

        if (GUILayout.Button("Bottom"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(0, -90, 0);
        }

        if (GUILayout.Button("Left"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(-90, 0, 0);
        }

        if(GUILayout.Button("Right"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(90, 0, 0);
        }

        if (GUILayout.Button("Isometric"))
        {
          this.Camera.LookedPoint = Vector3.zero;
          this.Camera.Distance = 50;
          this.Camera.Rotation = new Vector3(45, 45, 0);
        }
         
        GUILayout.EndVertical();
      GUILayout.EndArea();
    }

    /// <summary>
    ///   Calculate width and location of each window elements.
    /// </summary>
    public void Layout()
    {
      GUISkin skin = this._skin;

      if (skin == null)
      {
        return;
      }

      RectOffset windowPadding = skin.window.padding;
      RectOffset windowBorder = skin.window.border;

      this._fieldsBounds.x = windowPadding.left + windowBorder.left; ;
      this._fieldsBounds.y = windowPadding.top + windowBorder.top;
      this._fieldsBounds.width = 150;
      this._fieldsBounds.height = 180;

      if (this._minimized)
      {
        this._windowBounds.width = this._fieldsBounds.xMax + windowPadding.right + windowBorder.right;
        this._windowBounds.height = windowBorder.top + windowPadding.bottom + windowBorder.bottom;
      }
      else
      {
        this._windowBounds.width = this._fieldsBounds.xMax + windowPadding.right + windowBorder.right;
        this._windowBounds.height = this._fieldsBounds.yMax + windowPadding.bottom + windowBorder.bottom;
      }

      this._draggableArea.x = windowBorder.top;
      this._draggableArea.y = 0;
      this._draggableArea.width = this._windowBounds.width - windowBorder.top;
      this._draggableArea.height = windowBorder.top;

      this._minimizeToggleBounds.x = 0;
      this._minimizeToggleBounds.y = 0;
      this._minimizeToggleBounds.width = windowBorder.top;
      this._minimizeToggleBounds.height = windowBorder.top;
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Instantiate things.
    /// </summary>
    public void Awake()
    {
      this.Layout();
    }

    /// <summary>
    ///   Capture clicks.
    /// </summary>
    public void Update()
    {

    }
    #endregion
  }
}
