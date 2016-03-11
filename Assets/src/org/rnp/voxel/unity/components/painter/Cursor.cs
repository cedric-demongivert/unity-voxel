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
    private VoxelLocation _location;
    private Dimensions3D _dimensions;

    private Transform _objectTransform;

    public ColorPicker ColorPicker;
    public MeshRenderer CursorRenderer;

    public VoxelLocation Location
    {
      get
      {
        return _location;
      }
      set
      {
        this._location.Set(value);
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
        _dimensions.Set(value);
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

    /// <summary>
    ///   Check for inputs.
    /// </summary>
    private void HandleInputs()
    {
      // Scaling Down
      if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        if (Input.GetKeyDown(KeyCode.DownArrow) && this._dimensions.Depth > 1)
        {
          this._dimensions.Depth -= 1;
          this._location.Z += 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && this._dimensions.Depth > 1)
        {
          this._dimensions.Depth -= 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && this._dimensions.Width > 1)
        {
          this._dimensions.Width -= 1;
          this._location.X += 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && this._dimensions.Width > 1)
        {
          this._dimensions.Width -= 1;
        }

        if (Input.GetKeyDown(KeyCode.PageDown) && this._dimensions.Height > 1)
        {
          this._dimensions.Height -= 1;
        }

        if (Input.GetKeyDown(KeyCode.PageUp) && this._dimensions.Height > 1)
        {
          this._dimensions.Height -= 1;
          this._location.Y += 1;
        }
      }
      // Scaling Up
      else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
      {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          this._dimensions.Depth += 1;
          this._location.Z -= 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          this._dimensions.Depth += 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          this._dimensions.Width += 1;
          this._location.X -= 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          this._dimensions.Width += 1;
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
          this._dimensions.Height += 1;
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
          this._dimensions.Height += 1;
          this._location.Y -= 1;
        }
      }
      else
      {
        // Standard displacement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          this._location.Z -= 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          this._location.Z += 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          this._location.X -= 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          this._location.X += 1;
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
          this._location.Y += 1;
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
          this._location.Y -= 1;
        }
      }
    }

    /// <summary>
    ///   Update the scene.
    /// </summary>
    private void RefreshCursor()
    {
      this._objectTransform.position = new Vector3(
         this._location.X,
         this._location.Y,
         this._location.Z
      );
      
      this._objectTransform.localScale = new Vector3(
         this._dimensions.Width,
         this._dimensions.Height,
         this._dimensions.Depth
      );

      if (this.ColorPicker != null && this.CursorRenderer != null)
      {
        this.CursorRenderer.sharedMaterial.color = this.ColorPicker.SelectedColor;
      }
    }
  }
}
