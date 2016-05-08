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
  public class EulerianSimulation : FluidSimulation
  {
    /// <summary>
    ///   Bounds of the simulation.
    /// </summary>
    public Dimensions3D SimulationSize;

    private EularianGrid _grid;

    private EularianGrid _oldGrid;

    private VoxelMesh _mesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));

    public Vector3 Forces;

    public float fluidDensity = 1000f;

    private float _timeLeft;

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
      this._grid = new EularianGrid(SimulationSize);
      this._oldGrid = _grid;
      this._timeLeft = 0;
    }

    public void Render()
    {
      int k = 0;

      for(int i = 0; i < this.SimulationSize.Width; ++i)
      {
        for (int j = 0; j < this.SimulationSize.Height; ++j)
        {
          if(this._grid[new Vector3(i, j, k)].IsEmpty)
          {
            this._mesh[i, j, k] = Voxels.Empty;
          }
          else
          {
            this._mesh[i, j, k] = new Color32((byte)255, (byte)255, (byte)255, 0);
          }
        }
      }

      this._mesh.Commit();
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
      this._grid.Add(location, new EularianGrid.Fluid(speed));
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Remove(Vector3 location)
    {
      this._grid.Remove(location);
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Simulate(float duration)
    {
      this._timeLeft += duration;
      float nextTime = this.NextTime();

      if(this._timeLeft > nextTime)
      {
        while (this._timeLeft > nextTime)
        {
            this.DoSimulation(nextTime);
            this._timeLeft -= nextTime;

            nextTime = this.NextTime();
        }
      }
    }

    private float NextTime()
    {
      Vector3 maxSpeed = this._grid.GetMaxSpeed();

      if(maxSpeed.sqrMagnitude <= 0)
      {
        return float.MaxValue;
      }
      else
      {
        return 1f / maxSpeed.magnitude;
      }
    }

    private void DoSimulation(float step)
    {
      EularianGrid next = new EularianGrid(this.SimulationSize);

      this.Advect(next, step);
      this.PressureSolve(next, step);

      this._oldGrid = this._grid;
      this._grid = next;
    }

    private void PressureSolve(EularianGrid next, float step)
    {
      for (int i = 0; i < this.SimulationSize.Width; ++i)
      {
        for (int j = 0; j < this.SimulationSize.Height; ++j)
        {
          for (int k = 0; k < this.SimulationSize.Depth; ++k)
          {
            this.PressureSolve(i, j, k, step, next);
          }
        }
      }
    }

    private void PressureSolve(int i, int j, int k, float step, EularianGrid next)
    {
      EularianGrid.Fluid cell = next.Get(i, j, k);
      if (!cell.IsEmpty)
      {
        Vector3 newSpeed = cell.FluidSpeed + this.Forces * step;

        if(next.Contains(i + 1, j, k))
        {
          newSpeed.x -= step * ((next[i + 1, j, k].Pressure - next[i, j, k].Pressure) / this.fluidDensity);
        }
        else
        {
          newSpeed.x -= step * ((next[i + 1, j, k].Pressure) / this.fluidDensity);
        }

        if (next.Contains(i, j + 1, k))
        {
          newSpeed.y -= step * ((next[i, j + 1, k].Pressure - next[i, j, k].Pressure) / this.fluidDensity);
        }
        else
        {
          newSpeed.y -= step * ((next[i, j + 1, k].Pressure) / this.fluidDensity);
        }

        if (next.Contains(i, j, k + 1))
        {
          newSpeed.z -= step * ((next[i, j, k + 1].Pressure - next[i, j, k].Pressure) / this.fluidDensity);
        }
        else
        {
          newSpeed.z -= step * ((next[i, j, k + 1].Pressure) / this.fluidDensity);
        }

        cell.FluidSpeed = newSpeed;

        next.Set(i, j, k, cell);
      }
    }

    private void Advect(EularianGrid next, float step)
    {
      for (int i = 0; i < this.SimulationSize.Width; ++i)
      {
        for (int j = 0; j < this.SimulationSize.Height; ++j)
        {
          for (int k = 0; k < this.SimulationSize.Depth; ++k)
          {
            this.Advect(i, j, k, step, next);
          }
        }
      }
    }

    private void Advect(int i, int j, int k, float step, EularianGrid next)
    {
      EularianGrid.Fluid cell = this._grid.Get(i, j, k);
      if(!cell.IsEmpty)
      {

        Vector3 nextLocation = new Vector3(i, j, k) + cell.FluidSpeed * step;
        if (next.Contains(nextLocation))
        {
          next.Add(nextLocation, cell);
        }
      }
    }
    
  }
}
