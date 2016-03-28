using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.unity.components.translators
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   Attribute that specify what kind of VoxelMesh a translator can compute.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
  public class Translate : Attribute
  {
    public readonly String Style;
    public readonly Type Processable;

    public Translate(String style, Type processable)
    {
      this.Style = style;
      this.Processable = processable;
    }
  }
}
