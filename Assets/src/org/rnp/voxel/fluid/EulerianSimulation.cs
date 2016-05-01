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

    private VoxelMesh _mesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));

    public Vector3 Forces;

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
      this._timeLeft = 0;
    }

    public void Render()
    {
      for(int i = 0; i < this.SimulationSize.Width; ++i)
      {
        for (int j = 0; j < this.SimulationSize.Height; ++j)
        {
          for (int k = 0; k < this.SimulationSize.Depth; ++k)
          {
            if(this._grid[new Vector3(i, j, k)].FluidQuantity > 0)
            {
              this._mesh[i, j, k] = new Color32(255, 255, 255, 0);
            }
            else
            {
              this._mesh[i, j, k] = Voxels.Empty;
            }
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
    public override void Add(float quantity, Vector3 location, Vector3 speed)
    {
      this._grid.Add(location, new EularianGrid.Fluid(quantity, speed));
    }

    /// <see cref="org.rnp.voxel.fluid.FluidSimulation"/>
    public override void Remove(float quantity, Vector3 location)
    {
      this._grid.Sub(location, new EularianGrid.Fluid(quantity, Vector3.zero));
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
      this.ApplyForce(next, step);

      this._grid = next;
    }

    private void ApplyForce(EularianGrid next, float step)
    {
      for (int i = 0; i < this.SimulationSize.Width; ++i)
      {
        for (int j = 0; j < this.SimulationSize.Height; ++j)
        {
          for (int k = 0; k < this.SimulationSize.Depth; ++k)
          {
            next.Add(new Vector3(i, j, k), new EularianGrid.Fluid(0, this.Forces * step));
          }
        }
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
      EularianGrid.Fluid cell = this._grid.GetNode(i, j, k);
      if(cell.FluidQuantity > 0)
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
