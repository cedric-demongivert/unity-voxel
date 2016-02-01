using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A voxel location in space.
  /// </summary>
  public interface IVoxelLocation
  {
    int X
    {
      get;
      set;
    }

    int Y
    {
      get;
      set;
    }

    int Z
    {
      get;
      set;
    }

    Vector3 Vector
    {
      get;
      set;
    }
  }
}
