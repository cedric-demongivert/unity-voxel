using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.mesh
{
  [CustomEditor(typeof(SphereVoxelMesh))]
  public class SphereVoxelMeshEditor : Editor
  {
    /// <summary>
    ///   The edited MenuBar.
    /// </summary>
    public SphereVoxelMesh TargetMesh
    {
      get
      {
        return this.target as SphereVoxelMesh;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.TargetMesh.Radius = EditorGUILayout.IntField("Radius", this.TargetMesh.Radius);
      this.TargetMesh.Color = EditorGUILayout.ColorField("Color", this.TargetMesh.Color);

      if(this.TargetMesh.IsDirty)
      {
        this.TargetMesh.Commit();
      }

      EditorGUILayout.EndVertical();
    }
    
  }
}
