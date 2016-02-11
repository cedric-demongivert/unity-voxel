using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A voxel location in space. A kind of Vector3 with only int values.
  /// </summary>
  public interface IVoxelLocation
  {
    /// <summary>
    ///   X coordinate, Width.
    /// </summary>
    int X
    {
      get;
      set;
    }

    /// <summary>
    ///   Y coordinate, Height.
    /// </summary>
    int Y
    {
      get;
      set;
    }

    /// <summary>
    ///   Z coordinate, Depth.
    /// </summary>
    int Z
    {
      get;
      set;
    }

    IVoxelLocation Add(int x, int y, int z);
    IVoxelLocation Add(float x, float y, float z);
    IVoxelLocation Add(IVoxelLocation toAdd);

    IVoxelLocation Set(int x, int y, int z);
    IVoxelLocation Set(float x, float y, float z);
    IVoxelLocation Set(IVoxelLocation location);

    IVoxelLocation Mul(int x, int y, int z);
    IVoxelLocation Mul(float x, float y, float z);
    IVoxelLocation Mul(int s);
    IVoxelLocation Mul(float s);
  }
}
