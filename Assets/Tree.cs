using UnityEngine;
using System.Collections;
using org.rnp.voxel;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using System;

public class Tree : VoxelMeshContainer {

  private VoxelMesh _tree = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
  
  public int TrunckMinHeight = 4;
  public int TrunckMaxHeight = 6;

  public int Levels = 2;

  public int Seed = -1;

  public Color32 TrunckColor = new Color32(108, 40, 26, 0);
  public Color32 LeafMainColor = new Color32(38, 154, 81, 0);
  public Color32 LeafSecondaryColor = new Color32(38, 154, 81, 0);

  public override VoxelMesh Mesh
  {
    get
    {
      return this._tree;
    }
  }

  // Use this for initialization
  void Start () {
    if(this.Seed < 0)
    {
      this.Seed = UnityEngine.Random.seed;
    }

    this.Generate();
  }

  private static VoxelLocation[] BRANCH_DIRECTIONS = new VoxelLocation[] {
    VoxelLocation.Left, VoxelLocation.Right, VoxelLocation.Forward, VoxelLocation.Back
  };

  private static VoxelLocation[] BRANCH_LOCATIONS = new VoxelLocation[] {
    VoxelLocation.Left, VoxelLocation.Back, VoxelLocation.Zero, VoxelLocation.Back.Add(VoxelLocation.Left)
  };
  
  /// <summary>
  ///   Génération de notre arbre :
  /// </summary>
  private void Generate()
  {
    // Pour démarer d'une base propre.
    this._tree.Clear();

    // Configuration de la seed
    int oldSeed = UnityEngine.Random.seed;
    UnityEngine.Random.seed = this.Seed;

    int nextHeight = 0;

    for(int i = 0; i < this.Levels; ++i)
    {
      nextHeight = this.GenerateLevel(nextHeight);
    }

    // Retour à l'état initial
    UnityEngine.Random.seed = oldSeed;

    // Sauvegarde des changements.
    this._tree.Commit();
  }

  private int GenerateLevel(int baseHeight)
  {
    VoxelLocation min;
    VoxelLocation max;

    this.GenerateTrunck(baseHeight, out min, out max);
    this.GenerateLeafs(min.Sub(1, 0, 1), max.Add(1, 6, 1));

    return max.Y + 6;
  }

  /// <summary>
  ///  Génération de notre tronc.
  /// </summary>
  private void GenerateTrunck(int baseHeight, out VoxelLocation min, out VoxelLocation max)
  {
    // On fixe la hauteur
    int height = UnityEngine.Random.Range(this.TrunckMinHeight, this.TrunckMaxHeight + 1);

    // On affiche le tronc
    VoxelMeshes.Fill(
      this._tree,
      new VoxelLocation(-1, baseHeight, -1),
      new Dimensions3D(2, height, 2),
      this.TrunckColor
    );

    this.SpawnTrunckBranches(baseHeight, height, out min, out max);
  }

  /// <summary>
  ///   Génère des branches depuis le tronc
  /// </summary>
  private void SpawnTrunckBranches(int baseHeight, int height, out VoxelLocation min, out VoxelLocation max)
  {
    if (height < 4)
    {
      min = new VoxelLocation(-1, baseHeight + height, -1);
      max = new VoxelLocation(1, baseHeight + height, 1);
    }
    else
    {
      min = new VoxelLocation(-1, baseHeight + height, -1);
      max = new VoxelLocation(1, baseHeight + height, 1);

      for (int i = 0; i < Tree.BRANCH_DIRECTIONS.Length; ++i)
      {
        VoxelLocation direction = Tree.BRANCH_DIRECTIONS[i];
        VoxelLocation start = Tree.BRANCH_LOCATIONS[i];

        VoxelLocation position = this.GenerateBranch(
          start.Add(0, baseHeight, 0),
          direction,
          height
        );

        min = min.SetIfMin(position);
        max = max.SetIfMax(position);
      }

      max = max.Add(1, 0, 1);
    }
  }

  /// <summary>
  ///   Génération d'une branche
  /// </summary>
  /// <param name="start">Position de départ par rapport au tronc</param>
  /// <param name="direction">Direction pour étendre la branche</param>
  /// <param name="height">Hauteur du tronc</param>
  /// <returns>La position finale du bout de la branche</returns>
  private VoxelLocation GenerateBranch(VoxelLocation start, VoxelLocation direction, int height)
  {
    // Position initiale de la branche en hauteur
    int spawnHeight = UnityEngine.Random.Range(2, height - 1);

    // S'éloigner de deux cases du tronc
    VoxelLocation branchOut = start.Add(0, spawnHeight - 1, 0).Add(direction.Mul(2));

    // Base de la branche
    VoxelMeshes.FillInclusive(
      this._tree,
      start.Add(0, spawnHeight - 1, 0),
      branchOut,
      this.TrunckColor
    );

    // Fin de la branche
    VoxelMeshes.FillInclusive(
       this._tree,
       branchOut,
       branchOut.Add(0, height - spawnHeight, 0),
       this.TrunckColor
     );

    return this.SpawnSubBranches(branchOut, direction, height - spawnHeight + 1);
  }

  /// <summary>
  ///   Génère des sous-branches depuis une branche
  /// </summary>
  /// <param name="branchBase"></param>
  /// <param name="direction"></param>
  /// <param name="height"></param>
  /// <returns>La position finale du bout de la branche</returns>
  private VoxelLocation SpawnSubBranches(VoxelLocation branchBase, VoxelLocation direction, int height)
  {
    if (height < 4)
    {
      return branchBase.Add(0, height, 0);
    }

    return this.GenerateBranch(
      branchBase,
      direction,
      height
    );
  }

  /// <summary>
  ///   Génération des feuilles
  /// </summary>
  private void GenerateLeafs(VoxelLocation min, VoxelLocation max)
  {
    // Récupération des coordonnées minimum et maximum.
    //VoxelLocation min = First.SetIfMin(Second);
    //VoxelLocation max = First.SetIfMax(Second);

    // Couleur principale.
    VoxelMeshes.Fill(this._tree, min, max, this.LeafMainColor);
    
    int height = max.Y - min.Y;

    // Variantes en fonction de la hauteur.
    if(height > 5)
    {
      VoxelMeshes.Fill(
        this._tree, min.Add(0, 1, 0), 
        new VoxelLocation(max.X, min.Y + 3, max.Z), 
        this.LeafSecondaryColor
      );
      VoxelMeshes.Fill(
        this._tree, min.Add(0, 4, 0),
        new VoxelLocation(max.X, min.Y + 5, max.Z),
        this.LeafSecondaryColor
      );
    }
    else if(height > 3)
    {
      VoxelMeshes.Fill(
        this._tree, min.Add(0, 1, 0),
        new VoxelLocation(max.X, min.Y + 2, max.Z),
        this.LeafSecondaryColor
      );
    }

    // Sauvegarde des changements.
    //this._tree.Commit();
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
