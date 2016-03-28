using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.translators
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Configure mesh renderers for each mesh filter genered by a voxel filter.
  /// </summary>
  [RequireComponent(typeof(VoxelFilter))]
  [RequireComponent(typeof(MeshRenderer))]
  [ExecuteInEditMode]
  public class VoxelRenderer : MonoBehaviour
  {
    /// <summary>
    ///   Assocated filter.
    /// </summary>
    private VoxelFilter _filter;
    
    /// <summary>
    ///   Get the associated filter.
    /// </summary>
    public VoxelFilter Filter 
    {
      get {
        return _filter;
      }
    }

    private MeshRenderer _parentRenderer;

    /// <summary>
    ///   Last effective update time.
    /// </summary>
    private long _lastUpdateTime;

    /// <summary>
    ///   Get last effective update time.
    /// </summary>
    public long LastUpdateTime
    {
      get
      {
        return this._lastUpdateTime;
      }
    }

    /// <summary>
    ///   Check if the renderer needs to update.
    /// </summary>
    public bool IsDirty
    {
      get
      {
        return this._lastUpdateTime < this._filter.LastUpdateTime;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update()
    {
      this.Refresh();
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake() 
    {
      this._filter = GetComponent<VoxelFilter>();
      this._parentRenderer = GetComponent<MeshRenderer>();

      this._parentRenderer.sharedMaterials = this._parentRenderer.sharedMaterials;
      this.Reset();
    }

    /// <summary>
    ///   Resynchronize renderers.
    /// </summary>
    public void Refresh()
    {
      if (this.IsDirty)
      {
        this.Refresh(this._filter.RootTranslator);
        this.Touch(); 
      }
    }

    /// <summary>
    ///   Refresh a translator.
    /// </summary>
    /// <param name="translator"></param>
    protected void Refresh(Translator translator)
    {
      if (translator == null) return;

      this.Synchronize(translator);

      if (translator is ComposedTranslator)
      {
        ComposedTranslator parent = translator as ComposedTranslator;

        foreach (Translator child in parent)
        {
          this.Refresh(child);
        }
      }
    }

    /// <summary>
    ///   Synchronize a meshRenderer.
    /// </summary>
    /// <param name="translator"></param>
    protected void Synchronize(Translator translator)
    {
      if (translator.GetComponent<MeshFilter>() == null) return;

      MeshRenderer childRenderer = translator.GetComponent<MeshRenderer>();

      if (childRenderer == null)
      {
        childRenderer = translator.gameObject.AddComponent<MeshRenderer>();
      }

      MaterialPropertyBlock properties = new MaterialPropertyBlock();
      _parentRenderer.GetPropertyBlock(properties);
      childRenderer.SetPropertyBlock(properties);
      childRenderer.sharedMaterials = _parentRenderer.sharedMaterials;
    }
    
    /// <summary>
    ///   Mark updated.
    /// </summary>
    protected void Touch()
    {
      this._lastUpdateTime = (long)(Time.time * 1000);
    }

    /// <summary>
    ///   Reset the voxel renderer.
    /// </summary>
    public void Reset()
    {
      this._lastUpdateTime = -1;
    }
  }
}
