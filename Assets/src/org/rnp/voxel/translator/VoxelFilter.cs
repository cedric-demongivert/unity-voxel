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
  ///   Display a voxel mesh.
  /// </summary>
  [ExecuteInEditMode]
  public class VoxelFilter : MonoBehaviour, IVoxelMeshCommitListener
  {
    /// <summary>
    ///   Listener for voxel filter change.
    /// </summary>
    private HashSet<IVoxelFilterCommitListener> _listeners = new HashSet<IVoxelFilterCommitListener>();

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
          if(this._mesh != null)
          {
            this._mesh.UnregisterCommitListener(this);
            this._mesh = null;
          }
         
          this._mesh = value;
          this.Reset();
        }
      }
    }

    /// <summary>
    ///   Register a new listener.
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterVoxelFilterCommitListener(IVoxelFilterCommitListener listener)
    {
      if(!this._listeners.Contains(listener))
      {
        this._listeners.Add(listener);
        listener.OnRegister(this);
        listener.OnCommit(this);
      }
    }

    /// <summary>
    ///   Register a new listener.
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterVoxelFilterCommitListener(IVoxelFilterCommitListener listener)
    {
      if (this._listeners.Contains(listener))
      {
        this._listeners.Remove(listener);
        listener.OnUnregister(this);
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void Awake()
    {
      this.Reset();
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected virtual void OnDestroy()
    {
      foreach(IVoxelFilterCommitListener listener in new HashSet<IVoxelFilterCommitListener>(this._listeners))
      {
        this.UnregisterVoxelFilterCommitListener(listener);
      }

      if (this._mesh != null)
      {
        this._mesh.UnregisterCommitListener(this);
      }
    }

    /// <summary>
    ///   Reset the translator.
    /// </summary>
    public void Reset()
    {
      if(this._mesh != null && this._style != null)
      {
        this._mesh.UnregisterCommitListener(this);
        this._mesh.RegisterCommitListener(this);
      }
    }

    /// <summary>
    ///   Clear sub objects.
    /// </summary>
    private void ClearChilds()
    {
      while (this.transform.childCount > 0)
      {
        DestroyImmediate(this.transform.GetChild(0).gameObject);
      }

      this._translator = null;
    }
    
    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnRegister(VoxelMesh mesh)
    {
      this.ClearChilds();
      
      if (this._mesh != null && this._style != null)
      {
        this._translator = Translators.Instance().Generate(
          this._style,
          this._mesh
        );

        this._translator.transform.SetParent(this.transform);
      }

      foreach (IVoxelFilterCommitListener listener in this._listeners)
      {
        listener.OnCommit(this);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitBegin(VoxelMesh mesh)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnCommitEnd(VoxelMesh mesh)
    {
      foreach (IVoxelFilterCommitListener listener in this._listeners)
      {
        listener.OnCommit(this);
      }
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMeshCommitListener"/>
    public void OnUnregister(VoxelMesh mesh)
    {
      this.ClearChilds();
    }
  }
}
