using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Keyboard & mouse control for the painter camera.
  /// </summary>
  [RequireComponent(typeof(PainterCamera))]
  public class PainterCameraControl : MonoBehaviour
  {
    /// <summary>
    ///   Sensibility of the zoom, higher the sensibility, faster the zoom.
    /// </summary>
    public float ZoomSensibility = 15f;

    /// <summary>
    ///   Sensibility of the rotation tool.
    /// </summary>
    public float RotationSensibility = 0.5f;

    /// <summary>
    ///   Actual state of the controller.
    ///   A waiting controller do nothing.
    ///   A capturing controller update the camera coordinates.
    /// </summary>
    private enum State
    {
      WAITING, CAPTURING
    }

    /// <summary>
    ///   Capturable controls.
    /// </summary>
    public enum CapturableControl
    {
      ZOOM, ROTATION, LOOKED_POINT
    }

    /// <summary>
    ///   Actual state of the controller.
    /// </summary>
    private State _actualState;

    /// <summary>
    ///   Actual captured controls.
    /// </summary>
    private HashSet<CapturableControl> _capturedControls;

    /// <summary>
    ///   Original camera location, in order to do some calculation
    ///   in capturing mode.
    /// </summary>
    private Vector3 _originalCameraLookedPoint;
    private float _originalCameraDistance;
    private Vector2 _originalCameraRotation;

    /// <summary>
    ///   Modification of the initial camera location.
    /// </summary>
    private float _addedDistance;
    private Vector2 _addedRotation;
    private Vector3 _addedLookedPoint;

    /// <summary>
    ///   Calculation elements.
    /// </summary>
    private Vector2 _rotationOriginalMouseLocation;
    private Vector2 _lookedPointOriginalMouseLocation;

    /// <summary>
    ///   Controlled camera.
    /// </summary>
    public PainterCamera ControlledCamera
    {
      get
      {
        return this.gameObject.GetComponent<PainterCamera>();
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this._actualState = State.WAITING;
      this._capturedControls = new HashSet<CapturableControl>();
      this.StartCapture(CapturableControl.ZOOM);
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update()
    {
      this.UpdateState();
    }

    /// <summary>
    ///   Change the controller state.
    /// </summary>
    /// <param name="newState"></param>
    private void EnterState(State newState)
    {
      this._actualState = newState;
      this.OnEnterState(newState);
    }

    /// <summary>
    ///   Fire state switching events.
    /// </summary>
    /// <param name="newState"></param>
    private void OnEnterState(State newState)
    {
      switch(this._actualState)
      {
        case State.WAITING: this.OnEnterStateWaiting(); break;
        case State.CAPTURING: this.OnEnterStateCapturing(); break;
      }
    }

    /// <summary>
    ///   When the controller enter in a waiting state.
    /// </summary>
    private void OnEnterStateWaiting()
    {

    }

    /// <summary>
    ///   When the controller enter in a capturing state.
    /// </summary>
    private void OnEnterStateCapturing()
    {
      this._originalCameraDistance = this.ControlledCamera.Distance;
      this._originalCameraLookedPoint = this.ControlledCamera.LookedPoint;
      this._originalCameraRotation = this.ControlledCamera.Rotation;

      this._addedDistance = 0f;

      this._addedRotation = Vector2.zero;
    }

    /// <summary>
    ///   Update controller whith its state.
    /// </summary>
    private void UpdateState()
    {
      switch(this._actualState)
      {
        case State.WAITING: this.UpdateWaiting(); break;
        case State.CAPTURING: this.UpdateCapturing(); break;
      }
    }

    /// <summary>
    ///   Waiting update.
    /// </summary>
    private void UpdateWaiting()
    {
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
      {
        this.EnterState(State.CAPTURING);
      }
    }

    /// <summary>
    ///   Capturing update.
    /// </summary>
    private void UpdateCapturing()
    {
      if (!this.IsCaptured(CapturableControl.ROTATION) && Input.GetMouseButton(0))
      {
        this.StartCapture(CapturableControl.ROTATION);
      }

      if (!this.IsCaptured(CapturableControl.LOOKED_POINT) && Input.GetMouseButton(2))
      {
        this.StartCapture(CapturableControl.LOOKED_POINT);
      }

      this.Capture();
      this.UpdateCamera();

      if (this.IsCaptured(CapturableControl.ROTATION) && !Input.GetMouseButton(0))
      {
        this.StopCapture(CapturableControl.ROTATION);
      }

      if (this.IsCaptured(CapturableControl.LOOKED_POINT) && !Input.GetMouseButton(2))
      {
        this.StopCapture(CapturableControl.LOOKED_POINT);
      }

      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
      {
        this.EnterState(State.WAITING);
      }
    }

    public bool IsCaptured(CapturableControl control)
    {
      return this._capturedControls.Contains(control);
    }

    private void UpdateCamera()
    {
      this.ControlledCamera.Distance = this._originalCameraDistance + this._addedDistance;
      this.ControlledCamera.Rotation = this._originalCameraRotation + this._addedRotation;
      this.ControlledCamera.LookedPoint = this._originalCameraLookedPoint + this._addedLookedPoint;
    }

    /// <summary>
    ///   Capture controls.
    /// </summary>
    private void Capture()
    {
      foreach(CapturableControl control in this._capturedControls) 
      {
        switch (control)
        {
          case CapturableControl.ZOOM: this.CaptureZoom(); break;
          case CapturableControl.ROTATION: this.CaptureRotation(); break;
          case CapturableControl.LOOKED_POINT: this.CaptureLookedPoint(); break;
        }
      }
    }

    private void CaptureZoom()
    {
      this._addedDistance += Input.GetAxis("Mouse ScrollWheel") * this.ZoomSensibility;
    }

    private void CaptureRotation()
    {
      this._addedRotation.y = -(Input.mousePosition.y - this._rotationOriginalMouseLocation.y) * this.RotationSensibility;
      this._addedRotation.x = -(Input.mousePosition.x - this._rotationOriginalMouseLocation.x) * this.RotationSensibility;
    }

    private void CaptureLookedPoint()
    {
      this._addedLookedPoint =
        (-(Input.mousePosition.y - this._lookedPointOriginalMouseLocation.y)) * this.ControlledCamera.UsedCameraTransform.up
        + (-(Input.mousePosition.x - this._lookedPointOriginalMouseLocation.x)) * this.ControlledCamera.UsedCameraTransform.right;
    }

    /// <summary>
    ///   When the controller begin a rotation capture.
    /// </summary>
    private void OnEnterRotationCapture()
    {

    }

    /// <summary>
    ///   Begin the capture of a control.
    /// </summary>
    /// <param name="control"></param>
    public void StartCapture(CapturableControl control)
    {
      if(this._capturedControls.Contains(control))
      {
        return;
      }
      else
      {
        this._capturedControls.Add(control);
        this.OnCaptureStart(control);
      }
    }

    private void OnCaptureStart(CapturableControl control)
    {
      switch(control)
      {
        case CapturableControl.ZOOM: this.OnZoomCaptureStart(); break;
        case CapturableControl.ROTATION: this.OnRotationCaptureStart(); break;
        case CapturableControl.LOOKED_POINT: this.OnLookedPointCaptureStart(); break;
      }
    }

    private void OnZoomCaptureStart()
    {
      
    }

    private void OnRotationCaptureStart()
    {
      this._rotationOriginalMouseLocation = Input.mousePosition;
      this._addedRotation = Vector2.zero;
    }

    private void OnLookedPointCaptureStart()
    {
      this._lookedPointOriginalMouseLocation = Input.mousePosition;
      this._addedLookedPoint = Vector3.zero;
    }

    /// <summary>
    ///   Stop the capture of a control.
    /// </summary>
    /// <param name="control"></param>
    public void StopCapture(CapturableControl control)
    {
      if (this._capturedControls.Contains(control))
      {
        this._capturedControls.Remove(control);
        this.OnCaptureStop(control);
      }
      else
      {
        return;
      }
    }

    private void OnCaptureStop(CapturableControl control)
    {
      switch (control)
      {
        case CapturableControl.ZOOM: this.OnZoomCaptureStop(); break;
        case CapturableControl.ROTATION: this.OnRotationCaptureStop(); break;
        case CapturableControl.LOOKED_POINT: this.OnLookedPointCaptureStop(); break;
      }
    }

    private void OnZoomCaptureStop()
    {

    }

    private void OnRotationCaptureStop()
    {
      this.ControlledCamera.Rotation = this._originalCameraRotation + this._addedRotation;
      this._originalCameraRotation += this._addedRotation;
      this._addedRotation = Vector2.zero;
    }

    private void OnLookedPointCaptureStop()
    {
      this.ControlledCamera.LookedPoint = this._originalCameraLookedPoint + this._addedLookedPoint;
      this._originalCameraLookedPoint += this._addedLookedPoint;
      this._addedLookedPoint = Vector2.zero;
    }
  }
}
