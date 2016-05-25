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
  ///   Procedural Bush
  /// </summary>
  [ExecuteInEditMode]
  public class Bush : VoxelMeshContainer
  {
    private VoxelMesh _bushMesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));

    public int Seed = 478516;

    public Color32 TrunckColor = new Color32(108, 40, 26, 0);
    public Color32 LeafMainColor = new Color32(38, 154, 81, 0);
    public Color32 LeafSecondaryColor = new Color32(14, 103, 62, 0);

    public int Levels = 1;

    public int TrunckMinHeight = 4;
    public int TrunckMaxHeight = 6;

    [Range(0f, 1f)]
    public float BranchSpawningChance = 0.7f;

    [Range(0f, 1f)]
    public float SubBranchSpawningChance = 0.6f;
    
    public int TrunckBranchSpawningMinHeightOffset = 1;
    public int TrunckBranchSpawningMaxHeightOffset = 2;

    public int SubBranchSpawningMinHeightOffset = 1;
    public int SubBranchSpawningMaxHeightOffset = 2;

    public int LeafMinHeight = 3;
    public int LeafMaxHeight = 7;

    public override VoxelMesh Mesh
    {
      get
      {
        return _bushMesh;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.Generate();
    }

    private static VoxelLocation[] BRANCH_DIRECTIONS = new VoxelLocation[] {
      VoxelLocation.Left, VoxelLocation.Right, VoxelLocation.Forward, VoxelLocation.Back
    };

    private static VoxelLocation[] BRANCH_OFFSETS = new VoxelLocation[] {
      VoxelLocation.Left, VoxelLocation.Back, VoxelLocation.Zero, VoxelLocation.Back.Add(VoxelLocation.Left)
    };

    /// <summary>
    ///   Generate a branch
    /// </summary>
    /// <param name="start"></param>
    /// <param name="height"></param>
    /// <param name="direction"></param>
    /// <returns>Branch final location</returns>
    protected VoxelLocation GenerateBranch(VoxelLocation start, int height, VoxelLocation direction)
    {
      // Generate branch
      VoxelLocation branchOut = start.Add(direction.Mul(2));

      VoxelMeshes.FillInclusive(
        this._bushMesh,
        start,
        branchOut,
        this.TrunckColor
      );

      VoxelMeshes.FillInclusive(
         this._bushMesh,
         branchOut,
         branchOut.Add(0, height-1, 0),
         this.TrunckColor
       );

      return this.SpawnSubBranches(branchOut, height, direction);      
    }

    /// <summary>
    ///   Spawn branch, for a branch
    /// </summary>
    /// <param name="branchOut"></param>
    /// <param name="height"></param>
    /// <param name="direction"></param>
    /// <returns>Branch final location</returns>
    private VoxelLocation SpawnSubBranches(VoxelLocation branchOut, int height, VoxelLocation direction)
    {
      if (height - SubBranchSpawningMaxHeightOffset >= SubBranchSpawningMinHeightOffset)
      {
        if (height > 2 && UnityEngine.Random.value < SubBranchSpawningChance)
        {
          int branchHeight = UnityEngine.Random.Range(SubBranchSpawningMinHeightOffset, height - SubBranchSpawningMaxHeightOffset);

          return this.GenerateBranch(
            branchOut.Add(0, branchHeight, 0),
            height - branchHeight,
            direction
          );
        }
      }

      return branchOut.Add(0, height, 0);
    }

    /// <summary>
    ///   Generate a trunck for a bush
    /// </summary>
    /// <param name="location"></param>
    /// <param name="height"></param>
    /// <param name="min">Min branch bounding box location</param>
    /// <param name="max">Max branch bounding box location</param>
    protected void GenerateTrunck(VoxelLocation location, int height, out VoxelLocation min, out VoxelLocation max)
    {
      // Generate Trunck
      VoxelMeshes.FillInclusive(
        this._bushMesh, 
        location.Sub(1, 0, 1), 
        location.Add(0, height, 0), 
        this.TrunckColor
      );

      // Generate branches
      this.SpawnBranches(location, height, out min, out max);
    }

    /// <summary>
    ///   Generate branches for a trunck.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="height"></param>
    /// <param name="min">Min branch bounding box location</param>
    /// <param name="max">Max branch bounding box location</param>
    private void SpawnBranches(VoxelLocation location, int height, out VoxelLocation min, out VoxelLocation max)
    {
      min = location.Add(-1, height, -1);
      max = location.Add(0, height, 0);
 
      if (height - TrunckBranchSpawningMaxHeightOffset < TrunckBranchSpawningMinHeightOffset) return;

      for (int i = 0; i < Bush.BRANCH_DIRECTIONS.Length; ++i)
      {
        VoxelLocation direction = Bush.BRANCH_DIRECTIONS[i];
        VoxelLocation offset = Bush.BRANCH_OFFSETS[i];

        if(UnityEngine.Random.value < this.BranchSpawningChance)
        {
          int branchHeight = UnityEngine.Random.Range(TrunckBranchSpawningMinHeightOffset, height - TrunckBranchSpawningMaxHeightOffset);

          VoxelLocation finalPoint = this.GenerateBranch(
            location.Add(0, branchHeight, 0).Add(offset),
            height - branchHeight,
            direction
          );

          min = min.SetIfMin(finalPoint);
          max = max.SetIfMax(finalPoint);
        }
      }
    }

    /// <summary>
    ///   Launch a bush generation
    /// </summary>
    public void Generate()
    {
      int oldSeed = UnityEngine.Random.seed;
      UnityEngine.Random.seed = this.Seed;

      this._bushMesh.Clear();

      int height = 0;

      for(int i = 0; i < this.Levels; ++i)
      {
        VoxelLocation min;
        VoxelLocation max;

        this.GenerateTrunck(
          VoxelLocation.Zero.Add(0, height, 0),
          UnityEngine.Random.Range(this.TrunckMinHeight, this.TrunckMaxHeight),
          out min,
          out max
        );
        
        height = max.Y + 1 + this.GenerateLeaf(
          min, max
        );
      }
      
      this._bushMesh.Commit();
      UnityEngine.Random.seed = oldSeed;
    }

    /// <summary>
    ///   Generate Bush Leaf
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private int GenerateLeaf(VoxelLocation min, VoxelLocation max)
    {
      int leafHeight = UnityEngine.Random.Range(LeafMinHeight, LeafMaxHeight);

      // Main Color
      VoxelMeshes.FillInclusive(
        this._bushMesh,
        min.Sub(1, 0, 1),
        max.SetIfMax(0, min.Y + leafHeight, 0).Add(1, 0, 1),
        this.LeafMainColor
      );

      // Secondary Color
      if (leafHeight >= 5)
      {
        foreach(int i in new int[] { 1, 2, 4 })
          VoxelMeshes.FillInclusive(
              this._bushMesh,
              min.Sub(1, 0, 1).Add(0, i, 0),
              max.Add(1, i, 1),
              this.LeafSecondaryColor
          );
      }
      else
      {
        VoxelMeshes.FillInclusive(
            this._bushMesh,
            min.Sub(1, 0, 1).Add(0, 1, 0),
            max.Add(1, 1, 1),
            this.LeafSecondaryColor
        );
      }

      return leafHeight;
    }
  }
}
