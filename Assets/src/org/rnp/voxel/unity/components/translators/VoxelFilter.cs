using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.unity.components.meshes;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.unity.components.translators
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Display a voxel mesh.
  /// </summary>
  [ExecuteInEditMode]
  public class VoxelFilter : MonoBehaviour
  {
    /// <summary>
    ///   Style to use when the filter needs to translate a mesh.
    /// </summary>
    [SerializeField]
    private string _style;

    /// <summary>
    ///   The mesh to translate.
    /// </summary>
    [SerializeField]
    private VoxelMesh _mesh;

    /// <summary>
    ///   Genered translator.
    /// </summary>
    private Translator _translator;

    /// <summary>
    ///   Get the root translator if exists.
    /// </summary>
    public Translator RootTranslator
    {
      get
      {
        return _translator;
      }
    }

    /// <summary>
    ///   Change the style used in translation.
    /// </summary>
    public string Style
    {
      get
      {
        return this._style;
      }
      set
      {
        if (this._style == null || !this._style.Equals(value))
        {
          this._style = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   Mesh to translate.
    /// </summary>
    public VoxelMesh Mesh
    {
      get
      {
        return this._mesh;
      }
      set
      {
        if (this._mesh != value)
        {
          this._mesh = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   Get the voxel mesh to translate.
    /// </summary>
    protected IVoxelMesh _voxelMesh
    {
      get
      {
        if (this._mesh == null)
        {
          return null;
        }
        else
        {
          return this._mesh.Mesh;
        }
      }
    }

    /// <summary>
    ///   Last translated mesh.
    /// </summary>
    private IVoxelMesh _oldMesh;

    /// <summary>
    ///   Last effective translation.
    /// </summary>
    private long _lastUpdateTime;

    /// <summary>
    ///   Check if this filter needs to recalculate its mesh.
    /// </summary>
    public bool IsDirty
    {
      get
      {
        if (this._voxelMesh == null) return false;
        else
        {
          return this._lastUpdateTime < this._voxelMesh.LastUpdateTime;
        }
      }
    }

    /// <summary>
    ///   
    /// </summary>
    public long LastUpdateTime
    {
      get
      {
        return this._lastUpdateTime;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected void Awake()
    {
      this.Reset();
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update()
    {
      this.Translate();
    }

    /// <summary>
    ///   Do translation.
    /// </summary>
    public void Translate()
    {
      // check for mesh changes.
      if (this._voxelMesh != null && this._oldMesh != null && this._voxelMesh != this._oldMesh)
      {
        this.Reset();
      }

      if (this.IsDirty && this._translator != null)
      {
        this._translator.Translate();
        this._oldMesh = this._voxelMesh;

        this.Touch();
      }
    }

    /// <summary>
    ///   Mark updated.
    /// </summary>
    protected void Touch()
    {
      this._lastUpdateTime = (long)(Time.time * 1000);
    }

    /// <summary>
    ///   Reset the translation structure.
    /// </summary>
    public void Reset()
    {
      this._translator = null;
      while (this.transform.GetChildCount() > 0)
      {
        Transform child = this.transform.GetChild(0);
        child.SetParent(null);
        DestroyImmediate(child.gameObject);
      }

      if (this._voxelMesh != null && this._style != null)
      {
        this._translator = Translators.Instance().Generate(
          this._style,
          this._voxelMesh,
          this._voxelMesh,
          VoxelLocation.Zero
        );
        this._translator.transform.SetParent(this.transform);
      }

      this._lastUpdateTime = -1;
    }
  }
}
