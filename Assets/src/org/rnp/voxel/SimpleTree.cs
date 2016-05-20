using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Simple Procedural Tree
  /// </summary>
  [ExecuteInEditMode]
  public class SimpleTree : VoxelMeshContainer
  {
    public VoxelMesh Tree = new MapVoxelMesh(new Dimensions3D(8, 8, 8));

    public Color32 MainTrunckColor = new Color32(108, 40, 26, 0);
    public Color32 MainTrunckInnerColor = new Color32(141, 72, 45, 0);

    public Color32 MainLeafColor = new Color32(38, 154, 81, 0);

    public int TrunckMinHeight = 5;
    public int TrunckMaxHeight = 8;

    public int TrunckRadius = 2;

    public int LeafMinRadius = 3;
    public int LeafMaxRadius = 6;
    public int LeafMinHeight = 4;
    public int LeafMaxHeight = 6;

    public override VoxelMesh Mesh
    {
      get
      {
        return Tree;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.Generate();
    }

    public void Generate()
    {
      this.Tree.Clear();

      int trunckHeight = UnityEngine.Random.Range(this.TrunckMinHeight, this.TrunckMaxHeight);
      int leafRadius = UnityEngine.Random.Range(this.LeafMinRadius, this.LeafMaxRadius);
      int leafHeight = UnityEngine.Random.Range(this.LeafMinHeight, this.LeafMaxHeight);

      this.GenerateTrunck(trunckHeight);
      this.GenerateLeaf(trunckHeight, leafRadius, leafHeight);

      this.Tree.Commit();
    }

    private void GenerateLeaf(int trunckHeight, int leafRadius, int leafHeight)
    {
      for (int i = -leafRadius; i < leafRadius; ++i)
      {
        for (int k = -leafRadius; k < leafRadius; ++k)
        {
          for (int j = 0; j < leafHeight; ++j)
          {
            this.Tree[i, j + trunckHeight, k] = this.MainLeafColor;
          }
        }
      }
    }

    private void GenerateTrunck(int trunckHeight)
    {
      for(int i = - TrunckRadius; i < TrunckRadius; ++i)
      {
        for (int k = -TrunckRadius; k < TrunckRadius; ++k)
        {
          Color32 trunckColor = this.MainTrunckColor;

          if(i > -TrunckRadius && i < TrunckRadius-1 && k > -TrunckRadius && k < TrunckRadius -1)
          {
            trunckColor = this.MainTrunckInnerColor;
          }

          for (int j = 0; j < trunckHeight; ++j)
          {
            this.Tree[i, j, k] = trunckColor;
          }
        }
      }

    }
  }
}
