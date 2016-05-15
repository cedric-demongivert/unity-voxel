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
  [CustomEditor(typeof(FileVoxelMesh))]
  public class FileVoxelMeshEditor : Editor
  {
    /// <summary>
    ///   The edited MenuBar.
    /// </summary>
    public FileVoxelMesh TargetMesh
    {
      get
      {
        return this.target as FileVoxelMesh;
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/Editor.html"/>
    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();

      this.TargetMesh.MeshFile = EditorGUILayout.ObjectField("Voxel Mesh File", this.TargetMesh.MeshFile, typeof(TextAsset), true) as TextAsset;

      EditorGUILayout.EndVertical();
    }

  }
}
