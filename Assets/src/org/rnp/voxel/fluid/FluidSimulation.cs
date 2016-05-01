using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.fluid
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A fluid simulation system.
  /// </summary>
  public abstract class FluidSimulation : VoxelMeshContainer
  {
    /// <summary>
    ///   Launch the simulation for a finite amount of time.
    /// </summary>
    /// <param name="duration"></param>
    public abstract void Simulate(float duration);

    /// <summary>
    ///   Add some fluid into the simulation.
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="location"></param>
    /// <param name="speed"></param>
    public abstract void Add(float quantity, Vector3 location, Vector3 speed);

    /// <summary>
    ///   Remove some fluid of the simulation.
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="location"></param>
    public abstract void Remove(float quantity, Vector3 location);
  }
}
