using UnityEngine;
using System.Collections;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   An object form wich we can extract a 3D voxel dimension.
  /// </summary>
  public interface IDimensions3D : ICopiable<IDimensions3D>
  {
    /// <summary>
    ///   Get or set width.
    /// </summary>
    int Width
    {
      get;
      set;
    }

    /// <summary>
    ///   Get or set height.
    /// </summary>
    int Height
    {
      get;
      set;
    }

    /// <summary>
    ///   Get or set depth.
    /// </summary>
    int Depth
    {
      get;
      set;
    }

    /// <summary>
    ///   Set all values of the object.
    /// </summary>
    /// <param name="other"></param>
    void Set(IDimensions3D other);

    /// <summary>
    ///   Set all values of the object.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    void Set(int width, int height, int depth);

    /// <summary>
    ///   Return true if the dimensions is null (0,0,0)
    /// </summary>
    /// <returns></returns>
    bool IsEmpty();
  }
}