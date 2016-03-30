using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace org.rnp.commit
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   An object used to do computations into ICommitListener objects.
  /// </summary>
  public interface ICommitable<T>
  {
    void Commit();
    void Bind(ICommitListener<T> listener);
    void Unbind(ICommitListener<T> listener);
  }
}
