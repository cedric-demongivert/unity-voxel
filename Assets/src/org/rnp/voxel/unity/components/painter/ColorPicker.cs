using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Color picker UI 
  /// </summary>
  [ExecuteInEditMode]
  public class ColorPicker : MonoBehaviour
  {
    public Color PickedColor = Color.black;

    public String PickerTitle = "Color Picker";

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void OnGUI()
    {
    
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {

    }
  }
}
