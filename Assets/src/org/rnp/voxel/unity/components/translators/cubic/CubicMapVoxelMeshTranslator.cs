using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.mesh;
using System.Collections;

namespace org.rnp.voxel.unity.components.translators.cubic
{
  /// <summary>
  ///   Translate a map voxel mesh chunck by chunck.
  /// </summary>
  [Translate("Cubes", typeof(IMapVoxelMesh))]
  [ExecuteInEditMode]
  public class CubicMapVoxelMeshTranslator : ComposedTranslator
  {
    /// <summary>
    ///   Translated chuncks.
    /// </summary>
    private Dictionary<VoxelLocation, Translator> _chuncks;
    private Dictionary<VoxelLocation, Translator> _newChuncks;

    protected override void Awake()
    {
      base.Awake();

      this._chuncks = new Dictionary<VoxelLocation, Translator>();
      this._newChuncks = new Dictionary<VoxelLocation, Translator>();
    }

    private IMapVoxelMesh _map
    {
      get
      {
        return this.PartToTranslate as IMapVoxelMesh;
      }
    }

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
      foreach (VoxelLocation location in this._map.Keys())
      {
        if (_chuncks.ContainsKey(location))
        {
          this.UpdateChunck(location);
        }
        else
        {
          this.DiscoverChunck(location);
        }
      }

      foreach(VoxelLocation location in this._chuncks.Keys)
      {
        this.DestroyChunck(location);
      }

      this.Swap();
    }

    /// <summary>
    ///   Swap chunck list for the next turn.
    /// </summary>
    private void Swap()
    {
      this._chuncks.Clear();
      Dictionary<VoxelLocation, Translator> tmp = this._chuncks;
      this._chuncks = this._newChuncks;
      this._newChuncks = tmp;
    }

    /// <summary>
    ///   Destroy a chunck.
    /// </summary>
    /// <param name="location"></param>
    private void DestroyChunck(VoxelLocation location)
    {
      DestroyImmediate(this._chuncks[location].gameObject);
    }

    /// <summary>
    ///   A new chunck was discovered.
    /// </summary>
    /// <param name="location"></param>
    private void DiscoverChunck(VoxelLocation location)
    {
      VoxelLocation worldLocation = location.Mul(this._map.ChildDimensions).Add(this.PartWorldLocation);

      Translator genered = Translators.Instance().Generate(
        "Cubes", this._map.GetChild(location), this.MeshToTranslate, worldLocation
      );

      genered.transform.SetParent(this.transform);

      this._newChuncks[location] = genered;

      genered.Translate();
    }

    /// <summary>
    ///   A chunck needs an update.
    /// </summary>
    /// <param name="location"></param>
    private void UpdateChunck(VoxelLocation location)
    {
      Translator translator = this._chuncks[location];
      IReadonlyVoxelMesh child = this._map.GetChild(location);

      if (translator.LastUpdateTime < child.LastUpdateTime)
      {
        VoxelLocation worldLocation = location.Mul(this._map.ChildDimensions).Add(this.PartWorldLocation);

        translator.PartToTranslate = child;
        translator.PartWorldLocation = worldLocation;

        translator.Translate();
      }

      this._newChuncks[location] = translator;
      this._chuncks.Remove(location);
    }
  }
}
