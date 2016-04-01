using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.translator
{
  [CustomEditor(typeof(VoxelFilter))]
  public class VoxelFilterEditor : Editor
  {
    /// <summary>
    ///   The edited MenuBar.
    /// </summary>
    public VoxelFilter TargetFilter
    {
      get
      {
        return this.target as VoxelFilter;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.TargetFilter.Mesh = EditorGUILayout.ObjectField("Mesh", this.TargetFilter.Mesh, typeof(VoxelMesh), true) as VoxelMesh;
      this.ChooseStyle();

      if(this.TargetFilter.Mesh != null)
      {
        EditorGUILayout.LabelField(this.TargetFilter.Mesh.GetType().Name, EditorStyles.boldLabel);
        Editor.CreateEditor(this.TargetFilter.Mesh).OnInspectorGUI();
      }

      EditorGUILayout.EndVertical();
    }

    /// <summary>
    ///   GUI for selecting a style.
    /// </summary>
    private void ChooseStyle()
    {
      int styleIndex = 0;
      string[] styles = Translators.Instance().AvailableStyles();
      string selected = this.TargetFilter.Style;

      if (selected == null)
      {
        styleIndex = 0;
      }
      else
      {
        styleIndex = Array.IndexOf(styles, selected);
        styleIndex = (styleIndex < 0) ? 0 : styleIndex;
      }

      styleIndex = EditorGUILayout.Popup("Style", styleIndex, styles);

      this.TargetFilter.Style = styles[styleIndex];
    }

  }
}
