using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.fluid
{
  public abstract class FluidSimulation : VoxelMeshContainer
  {
    public abstract void Simulate(float duration);

    public abstract void Add(float quantity, Vector3 location, Vector3 speed);

    public abstract void Remove(float quantity, Vector3 location);
  }
}
