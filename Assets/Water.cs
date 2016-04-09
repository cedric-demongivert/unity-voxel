using UnityEngine;
using System.Collections;
using org.rnp.voxel.translator;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh;
using org.rnp.voxel;
using System;

public class Water : VoxelMeshContainer {

  public VoxelFilter vf;
  public VoxelRenderer vr;
  public MeshRenderer mr;

  private VoxelMesh meshToRender;

  public override VoxelMesh Mesh
  {
    get
    {
      return meshToRender;
    }
  }

  public int width, height;
  public Vector3 position;

  public float initTime = 1;
  private float timeLeft;

  private static int[] table = new int[] {
    0,  //0
    4,  //1
    2,  //2
    6,  //3
    16, //4
    20, //5
    18, //6
    22, //7
    8,  //8
    12, //9
    10, //10
    14, //11
    24, //12
    28, //13
    26, //14
    30, //15
    16, //16
    20, //17
    18, //18
    26, //19
    18, //20
    26, //21    WARNING (22)
    26, //22
    30, //23
    24, //24
    26, //25    WARNING (28)
    22, //26    (26)
    30, //27
    22, //28        (26)
    30, //29
    30, //30
    31  //31
  };

  // Use this for initialization
  public override void Awake ()
  {
    this.meshToRender = new MapVoxelMesh(new Dimensions3D(16, 16, 16));
    vf.Mesh = this;
    vf.Reset();
    timeLeft = initTime;
  }
	
	// Update is called once per frame
	public override void Update () {

    timeLeft -= Time.deltaTime;
    if (timeLeft < 0)
    {
      for (int i = 0; i < width; i++)
      {
        for (int j = 1; j < height; j++)
        {
            VoxelLocation partLoc = new Vector3(i, j, 0);
            ComputeLife(partLoc);
        }
      }

      this.meshToRender.Commit();
      timeLeft = initTime;
    }
  }

  private void ComputeLife(VoxelLocation cell)
  {
    Apply(cell, Next(Capture(cell)));
  }

  private int Capture(VoxelLocation cell)
  {
      int state = 0;

      if (IsAlive(cell.Add(0, 1, 0)))
          state |= 1;
      if (IsAlive(cell.Add(-1, 0, 0)))
          state |= 2;
      if (IsAlive(cell))
          state |= 4;
      if (IsAlive(cell.Add(1, 0, 0)))
          state |= 8;
      if (IsAlive(cell.Add(0, -1, 0)))
          state |= 16;

      return state;
  }
  private void Apply(VoxelLocation cell, int state)
  {
      if ((1 & state) != 0)
      {
          this.Live(cell.Add(0, 1, 0));
      }
      else
          Kill(cell.Add(0, 1, 0));

      if ((2 & state) != 0)
      {
          this.Live(cell.Add(-1, 0, 0));
      }
      else
          Kill(cell.Add(-1, 0, 0));

      if ((4 & state) != 0)
      {
          this.Live(cell);
      }
      else
          Kill(cell);

      if ((8 & state) != 0)
      {
          this.Live(cell.Add(1, 0, 0));
      }
      else
          Kill(cell.Add(1, 0, 0));

      if ((16 & state) != 0)
      {
          this.Live(cell.Add(0, -1, 0));
      }
      else
          Kill(cell.Add(0, -1, 0));
  }
  private int Next(int state)
  {
      return Water.table[state];
  }

  public void createParticule(int posx, int posy)
  {
      createParticule(new VoxelLocation(posx, posy, 0));
  }

  public void createParticule(VoxelLocation _position)
  {
      Live(_position);
  }

 

  private void Live(VoxelLocation cell)
  {
      this.meshToRender[cell] = new Color32(0, 0, 255, 0);
  }
  private void Kill(VoxelLocation cell)
  {
    this.meshToRender[cell] = Voxels.Empty;
  }

  private bool IsAlive(VoxelLocation locToTest)
  {
      return Voxels.HasNegative(locToTest)
            || locToTest.X > (position.x + width)
            || locToTest.Y > (position.y + height)
            || Voxels.IsNotEmpty(this.meshToRender[locToTest]);
  }
  private bool IsDead(VoxelLocation locToTest)
  {
      return !IsAlive(locToTest);
  }
}
