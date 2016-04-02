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
    private VoxelMesh _globalMesh;
    
    /// <summary>
    ///   The computed part of the mesh.
    /// </summary>
    private VoxelMesh _localMesh;
    
    /// <summary>
    ///   World location of the computed part.
    /// </summary>
    private VoxelLocation _worldLocation;
    
    /// <summary>
    ///   The computed mesh.
    /// </summary>
    public VoxelMesh GlobalMesh {
      get {
        return _globalMesh;
      }
    }

    /// <summary>
    ///   The computed part of the mesh.
    /// </summary>
    public VoxelMesh LocalMesh
    {
      get {
        return _localMesh;
      }
    }

    /// <summary>
    ///   World location of the computed part.
    /// </summary>
    public VoxelLocation WorldLocation
    {
      get {
        return _worldLocation;
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
    /// <param name="globalMesh"></param>
    public void Initialize(VoxelMesh globalMesh)
    {
      this.Initialize(globalMesh, globalMesh, VoxelLocation.Zero);
    }

    /// <summary>
    ///   Initialize the translator. 
    ///   
    ///   A translator can be initialized only one time.
    /// </summary>
    /// <param name="globalMesh"></param>
    /// <param name="localMesh"></param>
    public void Initialize(VoxelMesh globalMesh, VoxelMesh localMesh)
    {
      this.Initialize(globalMesh, localMesh, VoxelLocation.Zero);
    }

    /// <summary>
    ///   Initialize the translator. 
    ///   
    ///   A translator can be initialized only one time.
    /// </summary>
    /// <param name="globalMesh"></param>
    /// <param name="localMesh"></param>
    /// <param name="worldLocation"></param>
    public void Initialize(VoxelMesh globalMesh, VoxelMesh localMesh, VoxelLocation worldLocation)
    {
      if(_initialized)
      {
        throw new InvalidOperationException("Translator has already been initialized.");
      }
      else
      {
        _initialized = true;

        this._localMesh = localMesh;
        this._globalMesh = globalMesh;
        this._worldLocation = worldLocation;
        
        this._localMesh.RegisterCommitListener(this);
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void OnDestroy()
    {
      if(this._localMesh != null)
      {
        this._localMesh.UnregisterCommitListener(this);
      }
    }
    
    /// <summary>
    ///   Translation process.
    /// </summary>
    protected abstract void DoTranslation();
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnRegister(VoxelMesh mesh)
    {
      if(mesh != this._localMesh)
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
      if (mesh == this._localMesh)
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
      if(mesh == this._localMesh)
      {
        this._destroyOnCommit = true;
        this._localMesh = null;
        this._globalMesh = null;
        this._worldLocation = null;
      }
    }
  }
}
