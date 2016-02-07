using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.translator;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.utils;
using org.rnp.voxel.unity.components.meshes;
using UnityEngine;

namespace org.rnp.voxel.unity.components.translators
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple protoypic translator. Do not use in final games.
  /// </summary>
  [ExecuteInEditMode]
  public class MapTranslator : OctreeTranslator
  {
    /// <see cref="org.rnp.voxel.translator.ITranslator"/>
    public override void Translate()
    {
      this.Clear();

      if (this.VoxelMesh == null || this.VoxelMesh.Mesh == null)
      {
        return;
      }

      if (this.VoxelMesh.Mesh is IMapVoxelMesh)
      {
        this.TranslateMap(this.VoxelMesh.Mesh.Start, (IMapVoxelMesh)this.VoxelMesh.Mesh);
      }
      else if (this.VoxelMesh.Mesh is IOctreeVoxelMesh)
      {
        this.TranslateTree(this.VoxelMesh.Mesh.Start, (IOctreeVoxelMesh)this.VoxelMesh.Mesh);
      }
      else
      {
        this.TranslateLeaf(this.VoxelMesh.Mesh.Start, this.VoxelMesh.Mesh);
      }
    }

    public void TranslateMap(IVoxelLocation start, IMapVoxelMesh map)
    {
      foreach (IVoxelLocation chunckLocation in map.Keys())
      {
        IVoxelMesh chunck = map.GetChild(chunckLocation);
        IVoxelLocation chunckStart = new VoxelLocation(chunckLocation);
        chunckStart.Mul(map.ChildWidth, map.ChildHeight, map.ChildDepth);
        chunckStart.Add(start);

        if (chunck is IOctreeVoxelMesh)
        {
          this.TranslateTree(chunckStart, (IOctreeVoxelMesh)chunck);
        }
        else
        {
          this.TranslateLeaf(chunckStart, chunck);
        }
      }
    }
  }
}
