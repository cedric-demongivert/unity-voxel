using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.fluid
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Eularian based simulation.
  /// </summary>
  public class EulerianSimulation2D : FluidSimulation
  {
    public int Width;
    public int Height;

    public Vector2 ExternalForces = Vector2.down;
    public float FluidDensity = 1000;
    public float FluidViscosity = 0f;

    private struct Cell
    {
      public float Pressure;
      public Vector2 Speed;
      public bool IsWater;

      public Cell Fill()
      {
        this.IsWater = true;
        return this;
      }

      public Cell Dry()
      {
        return this.Reset();
      }

      public Cell Reset()
      {
        this.IsWater = false;
        this.Speed = Vector2.zero;
        this.Pressure = 0;
        return this;
      }

      public Cell AddSpeed(Vector2 speed)
      {
        this.Speed += speed;
        return this;
      }

      public Cell Set(Cell cell)
      {
        this.IsWater = cell.IsWater;
        this.Pressure = cell.Pressure;
        this.Speed = cell.Speed;
        return this;
      }

      public Cell Add(Cell cell)
      {
        this.Pressure += cell.Pressure;
        this.Speed += cell.Speed;
        return this;
      }

      public Cell SetSpeed(Vector2 vector2)
      {
        this.Speed = vector2;
        return this;
      }
    }

    private VoxelMesh _mesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
    private Cell[,] _grid;

    private float _timeLeft = 0;

    /// <see cref="org.rnp.voxel.VoxelMeshContainer"/>
    public override VoxelMesh Mesh
    {
      get
      {
        return _mesh;
      }
    }
    
    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this._grid = new Cell[Width+2, Height+2];
      this._timeLeft = 0;
    }

    public void Render()
    {
      for(int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          if(this._grid[i, j].IsWater)
          {
            this._mesh[i, j, 0] = new Color32((byte)255, (byte)255, (byte)255, 0);
          }
          else
          {
            this._mesh[i, j, 0] = Voxels.Empty;
          }
        }
      }

      this._mesh.Commit();
    }

    private Vector2 GetMaxSpeed()
    {
      Vector2 result = new Vector2();

      for(int i = 0; i < this.Width; ++i)
      {
        for(int j = 0; j < this.Height; ++j)
        {
          if(this._grid[i,j].Speed.sqrMagnitude > result.sqrMagnitude)
          {
            result = this._grid[i, j].Speed;
          }
        }
      }

      return result;
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update()
    {
      this.Simulate(Time.deltaTime);
      this.Render();
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Add(Vector3 location, Vector3 speed)
    {
      int x = Mathf.FloorToInt(location.x);
      int y = Mathf.FloorToInt(location.y);
      this._grid[x, y] = this._grid[x, y].Fill().AddSpeed(new Vector2(speed.x, speed.y));
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Remove(Vector3 location)
    {
      int x = Mathf.FloorToInt(location.x);
      int y = Mathf.FloorToInt(location.y);
      this._grid[x, y].Reset();
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Simulate(float duration)
    {
      if(this.HasNextTime())
      {
        this._timeLeft += duration;
        float nextTime = this.NextTime();

        if (this._timeLeft > nextTime)
        {
          while (this._timeLeft > nextTime)
          {
            this.DoSimulation(nextTime);
            this._timeLeft -= nextTime;

            nextTime = this.NextTime();
          }
        }
      }
      else
      {
        this._timeLeft = 0;
      }
      
    }

    private bool HasNextTime()
    {
      Vector2 maxSpeed = this.GetMaxSpeed();
      return !Mathf.Approximately(maxSpeed.sqrMagnitude, 0);
    }

    private float NextTime()
    {
      Vector2 maxSpeed = this.GetMaxSpeed();
      return 1f / maxSpeed.magnitude;
    }

    private void DoSimulation(float step)
    {
      Cell[,] result;
      result = this.Advect(step);
      result = this.ApplyViscosity(result, step);
      result = this.ApplyForces(result, step);

      result = this.ApplyBoundaries(result);

      result = this.PressureSolve(result, step);

      this.Commit(result);
    }

    private Cell[,] ApplyBoundaries(Cell[,] result)
    {
      for(int i = 0; i < this.Width; ++i)
      {
        if (result[i, 0].Speed.y < 0) result[i, 0].Speed.y = 0;
        if (result[i, this.Height - 1].Speed.y > 0) result[i, this.Height - 1].Speed.y = 0;
      }

      for (int i = 0; i < this.Height; ++i)
      {
        if (result[0, i].Speed.x < 0) result[0, i].Speed.x = 0;
        if (result[this.Width - 1, i].Speed.x > 0) result[this.Width - 1, i].Speed.x = 0;
      }

      return result;
    }

    private Cell[,] PressureSolve(Cell[,] result, float step)
    {
      float[,] divergence = this.Divergence(result, step);
      float[,] pressureResult = this.PressureSolve(divergence);

      return this.GradientSubstract(result, 0.5f, pressureResult);
    }

    private Cell[,] GradientSubstract(Cell[,] result, float halfdrx, float[,] pressureResult)
    {
      for (int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          float pL = 0;
          float pR = 0;
          float pB = 0;
          float pT = 0;

          if (this.Contains(i - 1, j)) pL = pressureResult[i - 1, j];
          else pL = pressureResult[i, j];

          if (this.Contains(i + 1, j)) pR = pressureResult[i + 1, j];
          else pR = pressureResult[i, j];

          if (this.Contains(i, j - 1)) pB = pressureResult[i, j - 1];
          else pB = pressureResult[i, j];

          if (this.Contains(i, j + 1)) pT = pressureResult[i, j + 1];
          else pT = pressureResult[i, j];

          result[i, j].Speed -= halfdrx * new Vector2(pR - pL, pT - pB);
        }
      }

      return result;
    }

    private float[,] PressureSolve(float[,] divergence)
    {
      float[,] result = new float[this.Width, this.Height];
      float[,] next = new float[this.Width, this.Height];

      for (int jacobiIteration = 0; jacobiIteration < 40; ++jacobiIteration)
      {
        for (int i = 0; i < this.Width; ++i)
        {
          for (int j = 0; j < this.Height; ++j)
          {
            next[i, j] = this.Jacobi(
              i, j,
              -1f,
              1f / 4f,
              result, divergence
            );
          }
        }

        float[,] tmp = result;
        result = next;
        next = tmp;
      }

      return result;
    }
    
    private float[,] Divergence(Cell[,] initial, float step)
    {
      float[,] result = new float[this.Width, this.Height];

      for (int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          if(initial[i,j].IsWater)
          {
            result[i,j] = this.Divergence(i, j, 0.5f, initial);
          }
        }
      }

      return result;
    }

    private float Divergence(int i, int j, float halfrdx, Cell[,] w)
    {
      Vector2 wL = Vector2.zero;
      Vector2 wR = Vector2.zero;
      Vector2 wB = Vector2.zero;
      Vector2 wT = Vector2.zero;

      if (this.Contains(i - 1, j)) wL = w[i - 1, j].Speed;
      else wL = -w[i, j].Speed;

      if (this.Contains(i + 1, j)) wR = w[i + 1, j].Speed;
      else wR = -w[i, j].Speed;

      if (this.Contains(i, j - 1)) wB = w[i, j - 1].Speed;
      else wB = -w[i, j].Speed;

      if (this.Contains(i, j + 1)) wT = w[i, j + 1].Speed;
      else wT = -w[i, j].Speed;

      return halfrdx * ((wR.x - wL.x) + (wT.y - wB.y));
    }

    private void Commit(Cell[,] result)
    {
      this._grid = result;
    }

    private Cell[,] AllocateGrid()
    {
      return new Cell[this.Width, this.Height];
    }

    private Cell[,] Advect(float step)
    {
      Cell[,] result = this.AllocateGrid();

      for (int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          this.Advect(result, i, j, step);
        }
      }

      return result;
    }

    private void Advect(Cell[,] result, int i, int j, float step)
    {
      Cell cell = this._grid[i, j];
      if (cell.IsWater)
      {
        Vector2 nextLocation = new Vector2(i, j) + cell.Speed * step;
        if (this.Contains(nextLocation))
        {
          int x = Mathf.FloorToInt(nextLocation.x);
          int y = Mathf.FloorToInt(nextLocation.y);
          result[x, y] = result[x, y].Fill().Add(cell);
        }
      }
    }
    
    private Cell[,] ApplyViscosity(Cell[,] previous, float step)
    {
      if(this.FluidViscosity > 0)
      {
        Cell[,] result = this.AllocateGrid();
        Cell[,] next = this.AllocateGrid();

        for (int jacobiIteration = 0; jacobiIteration < 20; ++jacobiIteration)
        {
          for (int i = 0; i < this.Width; ++i)
          {
            for (int j = 0; j < this.Height; ++j)
            {
              if (previous[i, j].IsWater)
              {
                next[i, j] = next[i, j].Fill().SetSpeed(this.Jacobi(
                  i, j,
                  1f / (this.FluidViscosity * step),
                  1f / (4f + 1f / (this.FluidViscosity * step)),
                  result, previous
                ));
              }
            }
          }

          Cell[,] tmp = result;
          result = next;
          next = tmp;
        }

        return result;
      }
      else
      {
        return previous;
      }
    }
    
    private Cell[,] ApplyForces(Cell[,] result, float step)
    {
      for (int i = 0; i < this.Width; ++i)
      {
        for (int j = 0; j < this.Height; ++j)
        {
          this.ApplyForces(result, i, j, step);
        }
      }

      return result;
    }

    private void ApplyForces(Cell[,] result, int i, int j, float step)
    {
      if (result[i, j].IsWater)
      {
        result[i, j].Speed += this.ExternalForces * step;
      }
    }

    private Vector2 Jacobi(int i, int j, float alpha, float beta, Cell[,] gridX, Cell[,] gridB)
    {
      Vector2 xL = Vector2.zero;
      Vector2 xR = Vector2.zero;
      Vector2 xB = Vector2.zero;
      Vector2 xT = Vector2.zero;

      if (this.Contains(i - 1, j)) xL = gridX[i - 1, j].Speed;
      else xL = -gridX[i, j].Speed;

      if (this.Contains(i + 1, j)) xR = gridX[i + 1, j].Speed;
      else xR = -gridX[i, j].Speed;

      if (this.Contains(i, j - 1)) xB = gridX[i, j - 1].Speed;
      else xB = -gridX[i, j].Speed;

      if (this.Contains(i, j + 1)) xT = gridX[i, j + 1].Speed;
      else xT = -gridX[i, j].Speed;

      Vector2 bC = gridB[i, j].Speed;

      return (xL + xR + xB + xT + alpha * bC) * beta;
    }

    private float Jacobi(int i, int j, float alpha, float beta, float[,] gridX, float[,] gridB)
    {
      float xL = 0f;
      float xR = 0f;
      float xB = 0f;
      float xT = 0f;

      if (this.Contains(i - 1, j)) xL = gridX[i - 1, j];
      else xL = gridX[i, j];

      if (this.Contains(i + 1, j)) xR = gridX[i + 1, j];
      else xR = gridX[i, j];

      if (this.Contains(i, j - 1)) xB = gridX[i, j - 1];
      else xB = gridX[i, j];

      if (this.Contains(i, j + 1)) xT = gridX[i, j + 1];
      else xT = gridX[i, j];


      float bC = gridB[i, j];

      return (xL + xR + xB + xT + alpha * bC) * beta;
    }

  private Vector2 Laplacian(int i, int j, Cell[,] grid)
    {
      Vector2 c10 = Vector2.zero;
      Vector2 c20 = Vector2.zero;
      Vector2 c01 = Vector2.zero;
      Vector2 c02 = Vector2.zero;

      if (this.Contains(i - 1, j)) c10 = grid[i - 1, j].Speed;
      else c10 = -grid[i, j].Speed;

      if (this.Contains(i + 1, j)) c20 = grid[i + 1, j].Speed;
      else c20 = -grid[i, j].Speed;

      if (this.Contains(i, j - 1)) c01 = grid[i, j - 1].Speed;
      else c01 = -grid[i, j].Speed;

      if (this.Contains(i, j + 1)) c02 = grid[i, j + 1].Speed;
      else c02 = -grid[i, j].Speed;

      return c10 + c20 + c01 + c02 - 4 * grid[i, j].Speed;
    }
        
    private bool Contains(Vector2 location)
    {
      return location.x >= 0 && location.x < this.Width && location.y >= 0 && location.y < this.Height;
    }


    private bool Contains(int i, int j)
    {
      return i >= 0 && i < this.Width && j >= 0 && j < this.Height;
    }

  }
}
