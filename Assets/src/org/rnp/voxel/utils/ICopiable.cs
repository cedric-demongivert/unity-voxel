using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.utils
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Simple copiable object.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface ICopiable<T>
  {
    T Copy();
  }
}
