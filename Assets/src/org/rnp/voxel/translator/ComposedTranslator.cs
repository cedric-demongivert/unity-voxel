using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A translator that has sub-translators.
  /// </summary>
  [ExecuteInEditMode]
  public abstract class ComposedTranslator : Translator, IEnumerable
  {
    /// <see cref="https://msdn.microsoft.com/fr-fr/library/system.collections.ienumerable(v=vs.110).aspx"/>
    public abstract IEnumerator GetEnumerator();
  }
}
