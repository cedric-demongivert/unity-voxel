﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh;
using System.Collections;

namespace org.rnp.voxel.translator.cubic
{
  /// <summary>
  ///   Translate a voxel mesh which is made of many chuncks.
  /// </summary>
  [Translate("Cubes", typeof(ChunckedVoxelMesh))]
  [ExecuteInEditMode]
  public class CubicChunckedVoxelMeshTranslator : ComposedTranslator
  {
    /// <summary>
    ///   Translated chuncks.
    /// </summary>
    private Dictionary<VoxelLocation, Translator> _chuncks;

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    protected void Awake()
    {
      this._chuncks = new Dictionary<VoxelLocation, Translator>();
    }

    private ChunckedVoxelMesh _map
    {
      get
      {
        return this.MeshToTranslate as ChunckedVoxelMesh;
      }
    }

    /// <see cref="org.rnp.voxel.translator.ComposedTranslator"></see>
    public override IEnumerator GetEnumerator()
    {
      foreach (Translator translator in this._chuncks.Values)
      {
        yield return translator;
      }
    }

    /// <summary>
    ///   Translation process.
    /// </summary>
    protected override void DoTranslation()
    {
      this.CheckForDelete();
      this.CheckForNew();
    }

    /// <summary>
    ///   Check for new chuncks.
    /// </summary>
    private void CheckForNew()
    {
      foreach (VoxelLocation location in this._map.ChunckLocations)
      {
        if (!_chuncks.ContainsKey(location))
        {
          VoxelLocation worldLocation = location.Mul(this._map.ChunckDimensions);
                    
          Translator genered = Translators.Instance().Generate(
            "Cubes", this._map.GetChunck(location)
          );

          genered.transform.SetParent(this.transform);
          genered.transform.localPosition = worldLocation;

          this._chuncks[location] = genered;
        }
      }
    }

    /// <summary>
    ///   Check for chuncks that need to be destroyed.
    /// </summary>
    private void CheckForDelete()
    { 
      foreach(VoxelLocation location in new HashSet<VoxelLocation>(this._chuncks.Keys))
      {
        Translator translator = this._chuncks[location];

        if(translator.DestroyOnCommit)
        {
          this._chuncks.Remove(location);
          Destroy(translator.gameObject);
        }
      }
    }
  }
}