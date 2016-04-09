using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A base translator implementation.
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Translator : MonoBehaviour, IVoxelMeshCommitListener
  {
    /// <summary>
    ///   The computed mesh.
    /// </summary>
    private VoxelMesh _meshToTranslate;
        
    /// <summary>
    ///   The computed part of the mesh.
    /// </summary>
    public VoxelMesh MeshToTranslate
    {
      get {
        return _meshToTranslate;
      }
    }
    
    /// <summary>
    ///   If true, this translator needs to be destroyed on next commit.
    /// </summary>
    private bool _destroyOnCommit = false;

    /// <summary>
    ///   If true, this translator needs to be destroyed on next commit.
    /// </summary>
    public bool DestroyOnCommit
    {
      get
      {
        return _destroyOnCommit;
      }
    }

    /// <summary>
    ///   True if the translator has already been initialized.
    /// </summary>
    private bool _initialized = false;

    /// <summary>
    ///   Initialize the translator. 
    ///   
    ///   A translator can be initialized only one time.
    /// </summary>
    /// <param name="meshToTranslate"></param>
    public void Initialize(VoxelMesh meshToTranslate)
    {
      if(_initialized)
      {
        throw new InvalidOperationException("Translator has already been initialized.");
      }
      else
      {
        _initialized = true;

        this._meshToTranslate = meshToTranslate;
        this._meshToTranslate.RegisterCommitListener(this);
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void OnDestroy()
    {
      if(this._meshToTranslate != null)
      {
        this._meshToTranslate.UnregisterCommitListener(this);
      }
    }
    
    /// <summary>
    ///   Translation process.
    /// </summary>
    protected abstract void DoTranslation();
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnRegister(VoxelMesh mesh)
    {
      if(mesh != this._meshToTranslate)
      {
        mesh.UnregisterCommitListener(this);
        throw new InvalidOperationException("A Translator can be registered to only one voxel mesh.");
      }
      else
      {
        this.DoTranslation();
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitBegin(VoxelMesh mesh)
    {
      if (mesh == this._meshToTranslate)
      {
        this.DoTranslation();
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitEnd(VoxelMesh mesh)
    {

    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnUnregister(VoxelMesh mesh)
    {
      if(mesh == this._meshToTranslate)
      {
        this._destroyOnCommit = true;
        this._meshToTranslate = null;
      }
    }
  }
}
