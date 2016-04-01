using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Configure mesh renderers for each mesh filter genered by a voxel filter.
  /// </summary>
  [RequireComponent(typeof(VoxelFilter))]
  [RequireComponent(typeof(MeshRenderer))]
  [ExecuteInEditMode]
  public class VoxelRenderer : MonoBehaviour, IVoxelFilterCommitListener
  {
    private MeshRenderer _parentRenderer;
        
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake() 
    {
      this._parentRenderer = GetComponent<MeshRenderer>();
      this._parentRenderer.sharedMaterials = this._parentRenderer.sharedMaterials;

      VoxelFilter filter = GetComponent<VoxelFilter>();
      filter.RegisterVoxelFilterCommitListener(this);
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

    /// <see cref="org.rnp.voxel.translator.IVoxelFilterCommitListener"/>
    public void OnRegister(VoxelFilter filter)
    { }

    /// <see cref="org.rnp.voxel.translator.IVoxelFilterCommitListener"/>
    public void OnCommit(VoxelFilter filter)
    {
      this.Refresh(filter.RootTranslator);
    }

    /// <see cref="org.rnp.voxel.translator.IVoxelFilterCommitListener"/>
    public void OnUnregister(VoxelFilter filter)
    { }
  }
}
