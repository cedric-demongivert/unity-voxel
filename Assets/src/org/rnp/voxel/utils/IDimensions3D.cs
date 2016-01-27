using UnityEngine;
using System.Collections;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT <cedric.demongivert@gmail.com></author>
  /// 
  /// <summary>
  ///   An object form wich we can extract a 3D voxel dimension.
  /// </summary>
  public interface IDimensions3D
  {
    uint Width
    {
      get;
    }

    uint Height
    {
      get;
    }

    uint Depth{
      get;
    }
  }
}