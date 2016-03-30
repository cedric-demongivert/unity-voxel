using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Painter camera component.
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class PainterCamera : MonoBehaviour
  {
    [SerializeField]
    protected Vector3 _lookedPoint;
    
    [SerializeField]
    protected float _distance;

    [SerializeField]
    protected Vector2 _rotation;

    protected bool _reversed;
    
    /// <summary>
    ///   Point wich was looked by the camera.
    /// </summary>
    public Vector3 LookedPoint
    {
      get
      {
        return this._lookedPoint;
      }
      set
      {
        this._lookedPoint = value;
      }
    }

    /// <summary>
    ///   Distance of the camera to the looked point.
    /// </summary>
    public float Distance
    {
      get
      {
        return this._distance;
      }
      set
      {
        this._distance = value;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update()
    {
      this.Refresh();
    }

    /// <summary>
    ///   Rotation of the camera
    /// </summary>
    public Vector2 Rotation
    {
      get
      {
        return this._rotation;
      }
      set
      {
        this._rotation = value;
        this._rotation.x %= 360f;
        this._rotation.y %= 360f;
      }
    }
    
    /// <summary>
    ///   Get used camera.
    /// </summary>
    public Camera UsedCamera
    {
      get
      {
        return this.GetComponent<Camera>();
      }
    }

    /// <summary>
    ///   Transform of the camera.
    /// </summary>
    public Transform UsedCameraTransform
    {
      get
      {
        return this.gameObject.transform;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.Refresh();
    }

    /// <summary>
    ///   Update location.
    /// </summary>
    /// <param name="newRotation"></param>
    private void Refresh()
    {
      float y = (-this.Rotation.y + 90f) * Mathf.Deg2Rad;
      float x = (this.Rotation.x + 90f) * Mathf.Deg2Rad;
      float abs = (Math.Abs(this.Rotation.y) % 360f);

      this._reversed = abs > 90f && abs < 270f;

      this.UsedCameraTransform.position = new Vector3(
        this.Distance * Mathf.Sin(y) * Mathf.Cos(x),
        this.Distance * Mathf.Cos(y),
        this.Distance * Mathf.Sin(y) * Mathf.Sin(x)
      );

      this.UsedCameraTransform.position += this.LookedPoint;

      if (this._reversed)
      {
        this.UsedCameraTransform.LookAt(this.LookedPoint, Vector3.down);
      }
      else
      {
        this.UsedCameraTransform.LookAt(this.LookedPoint, Vector3.up);
      }
    }
  }
}
