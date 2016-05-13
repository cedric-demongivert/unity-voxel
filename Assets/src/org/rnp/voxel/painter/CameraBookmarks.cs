using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.gui;

namespace org.rnp.voxel.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Gui for camera bookmarks.
  /// </summary>
  ///[ExecuteInEditMode]
  public class CameraBookmarks : MonoBehaviour
  {
    public PainterCamera Camera;

    /// <summary>
    ///   Instantiate things.
    /// </summary>
    public void Awake()
    {

    }

    public void Front()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = Vector3.zero;
    }

    public void Back()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(180, 0, 0);
    }

    public void Top()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(0, 90, 0);
    }
    
    public void Bottom()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(0, -90, 0);
    }

    public void Left()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(-90, 0, 0);
    }
       
    public void Right()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(90, 0, 0);
    }

    public void Isometric()
    {
      this.Camera.LookedPoint = Vector3.zero;
      this.Camera.Distance = 50;
      this.Camera.Rotation = new Vector3(45, 45, 0);
    }

    /// <summary>
    ///   Capture clicks.
    /// </summary>
    public void Update()
    {

    }
  }
}
