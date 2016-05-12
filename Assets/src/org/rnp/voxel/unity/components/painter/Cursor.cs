using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A voxel cursor that can be displaced and scaled in order to put or remove
  /// voxels from a mesh.
  /// </summary>
  [ExecuteInEditMode]
  public class Cursor : MonoBehaviour
  {
    private VoxelLocation _location = new VoxelLocation();
    private Dimensions3D _dimensions = new Dimensions3D();

    private Transform _objectTransform;

    public ColorPicker ColorPicker;
    public MeshRenderer CursorRenderer;

    public Camera View;

    public VoxelLocation Location
    {
      get
      {
        return _location;
      }
      set
      {
        this._location = value;
      }
    }

    public Dimensions3D Dimensions
    {
      get
      {
        return _dimensions;
      }
      set
      {
        _dimensions = value;
      }
    }

    public void Awake()
    {
      this._objectTransform = this.gameObject.transform;

      this._location = new VoxelLocation();
      this._dimensions = new Dimensions3D(1, 1, 1);
    }

    public void Update()
    {
      this.HandleInputs();
      this.RefreshCursor();
    }

    public void Forward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      this._location += Vector3.Cross(right, up);
    }

    public void Backward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      this._location -= Vector3.Cross(right, up);
    }

    public void Left()
    {
      this._location -= Voxels.KeepMax(this.View.transform.right);
    }

    public void Right()
    {
      this._location += Voxels.KeepMax(this.View.transform.right);
    }

    public void Top()
    {
      this._location += Voxels.KeepMax(this.View.transform.up);
    }

    public void Bottom()
    {
      this._location -= Voxels.KeepMax(this.View.transform.up);
    }

    public void ExtendForward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      Vector3 extension = Vector3.Cross(right, up);
      this.Extend(extension);
    }

    public void ExtendBackward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      Vector3 extension = -Vector3.Cross(right, up);
      this.Extend(extension);
    }

    public void ExtendLeft()
    {
      Vector3 extension = Voxels.KeepMax(-this.View.transform.right);
      this.Extend(extension);
    }

    public void ExtendRight()
    {
      Vector3 extension = Voxels.KeepMax(this.View.transform.right);
      this.Extend(extension);
    }

    public void ExtendTop()
    {
      Vector3 extension = Voxels.KeepMax(this.View.transform.up);
      this.Extend(extension);
    }

    public void ExtendBottom()
    {
      Vector3 extension = Voxels.KeepMax(-this.View.transform.up);
      this.Extend(extension);
    }

    private void Extend(Vector3 extension)
    {
      if (Voxels.HasNegative(extension))
      {
        this._dimensions.Sub(extension);
        this._location += extension;
      }
      else
      {
        this._dimensions.Add(extension);
      }
    }

    public void ReduceForward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      Vector3 reduction = -Vector3.Cross(right, up);
      this.Reduce(reduction);
    }

    public void ReduceBackward()
    {
      Vector3 right = Voxels.KeepMax(this.View.transform.right);
      Vector3 up = Voxels.KeepMax(this.View.transform.up);
      Vector3 reduction = Vector3.Cross(right, up);
      this.Reduce(reduction);
    }

    public void ReduceLeft()
    {
      Vector3 reduction = Voxels.KeepMax(this.View.transform.right);
      this.Reduce(reduction);
    }

    public void ReduceRight()
    {
      Vector3 reduction = -Voxels.KeepMax(this.View.transform.right);
      this.Reduce(reduction);
    }

    public void ReduceTop()
    {
      Vector3 reduction = -Voxels.KeepMax(this.View.transform.up);
      this.Reduce(reduction);
    }

    public void ReduceBottom()
    {
      Vector3 reduction = Voxels.KeepMax(this.View.transform.up);
      this.Reduce(reduction);
    }

    private void Reduce(Vector3 reduction)
    {
      if (Voxels.HasPositive(reduction))
      {
        this._dimensions.Sub(reduction);

        if (this._dimensions.HasNull())
        {
          this._dimensions.Add(reduction);
        }
        else
        {
          this._location += reduction;
        }
      }
      else
      {
        this._dimensions.Add(reduction);

        if (this._dimensions.HasNull())
        {
          this._dimensions.Sub(reduction);
        }
      }
    }

    /// <summary>
    ///   Check for inputs.
    /// </summary>
    private void HandleInputs()
    {
      // Scaling Down
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          this.ReduceTop();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          this.ReduceBottom();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          this.ReduceRight();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          this.ReduceLeft();
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
          this.ReduceForward();
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
          this.ReduceBackward();
        }
      }
      // Scaling Up
      else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
      {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          this.ExtendTop();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          this.ExtendBottom();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          this.ExtendRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          this.ExtendLeft();
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
          this.ExtendForward();
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
          this.ExtendBackward();
        }
      }
      else
      {
        // Standard displacement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          this.Top();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          this.Bottom();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          this.Right();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          this.Left();
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
          this.Forward();
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
          this.Backward();
        }
      }
    }

    /// <summary>
    ///   Update the scene.
    /// </summary>
    private void RefreshCursor()
    {
      this._objectTransform.position = new Vector3(
         this._location.X - 0.01f,
         this._location.Y - 0.01f,
         this._location.Z - 0.01f
      );
      
      this._objectTransform.localScale = new Vector3(
         this._dimensions.Width + 0.02f,
         this._dimensions.Height + 0.02f,
         this._dimensions.Depth + 0.02f
      );

      if (this.ColorPicker != null && this.CursorRenderer != null)
      {
        this.CursorRenderer.sharedMaterial.color = this.ColorPicker.SelectedColor;
      }
    }
  }
}
