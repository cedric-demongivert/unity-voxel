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
  ///   A base translator implementation.
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Translator : MonoBehaviour
  {
    /// <summary>
    ///   The computed mesh.
    /// </summary>
    private IVoxelMesh _meshToTranslate;
    
    /// <summary>
    ///   The computed part of the mesh.
    /// </summary>
    private IVoxelMesh _partToTranslate;
    
    /// <summary>
    ///   World location of the computed part.
    /// </summary>
    private VoxelLocation _partWorldLocation;

    /// <summary>
    ///   Last update time value.
    /// </summary>
    private long _lastUpdateTime;

    /// <summary>
    ///   Last effective update.
    /// </summary>
    public long LastUpdateTime
    {
      get
      {
        return this._lastUpdateTime;
      }
    }

    /// <summary>
    ///   The computed mesh.
    /// </summary>
    public IVoxelMesh MeshToTranslate {
      get {
        return _meshToTranslate;
      }
      set {
        if (this._meshToTranslate != value)
        {
          _meshToTranslate = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   The computed part of the mesh.
    /// </summary>
    public IVoxelMesh PartToTranslate
    {
      get {
        return _partToTranslate;
      }
      set {
        if (this._partToTranslate != value)
        {
          _partToTranslate = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   World location of the computed part.
    /// </summary>
    public VoxelLocation PartWorldLocation
    {
      get {
        return _partWorldLocation;
      }
      set {
        if (this._partWorldLocation == null || !this._partWorldLocation.Equals(value))
        {
          _partWorldLocation = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   True if this translator require an update.
    /// </summary>
    public bool IsDirty
    {
      get
      {
        return this.PartToTranslate.LastUpdateTime != this._lastUpdateTime;
      }
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void Awake()
    {
      this._lastUpdateTime = -1;
    }

    /// <summary>
    ///   Recalculate the final renderable mesh but don't send modifications
    /// to the graphic card.
    /// </summary>
    public void Translate()
    {
      if (this.IsDirty)
      {
        this.DoTranslation();
        this._lastUpdateTime = this.PartToTranslate.LastUpdateTime;
      }
    }

    /// <summary>
    ///   Reset the translator.
    /// </summary>
    public virtual void Reset()
    {
      this._lastUpdateTime = -1;
    }

    /// <summary>
    ///   Translation process.
    /// </summary>
    protected abstract void DoTranslation();
  }
}
