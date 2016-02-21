using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.unity.gui.exception
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Throwed when an user choose incoherent configuration values for
  /// a GUIColorPicker method.
  /// </summary>
  public class ColorPickerMissConfigurationException : Exception
  {
    public ColorPickerMissConfigurationException(String msg)
      : base(msg) 
    { }
  }
}
