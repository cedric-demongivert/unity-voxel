using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;
using org.rnp.gui.colorPicker;
using System;
using org.rnp.gui.colorPalette;
using UnityEditor;
using System.Collections.Generic;

namespace org.rnp.voxel.painter
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Unity component for paint.
  /// </summary>
  [ExecuteInEditMode]
  public sealed class Painter : VoxelMeshContainer 
  {
    public VoxelMesh PaintedMesh;
    public PainterColorPalette Palette;
    public ColorPicker Picker;

    [SerializeField]
    private List<PainterTool> _tools = new List<PainterTool>();

    [SerializeField]
    private int _activeTool = -1;

    public int ActiveTool
    {
      get
      {
        return this._activeTool;
      }
      set
      {
        if (this.HasTool(this.ActiveTool))
        {
          this._tools[this._activeTool].OnToolEnd();
        }

        this._activeTool = value;

        if (this.HasTool(this.ActiveTool))
        {
          this._tools[this._activeTool].OnToolStart();
        }
      }
    }

    public override VoxelMesh Mesh
    {
      get
      {
        return this.PaintedMesh;
      }
    }
    
    public int ToolsCount
    {
      get
      {
        return this._tools.Count;
      }
    }

    public IEnumerable<PainterTool> Tools
    {
      get
      {
        foreach(PainterTool tool in this._tools)
        {
          yield return tool;
        }
      }
    }
    
    public void RegisterTool(PainterTool tool)
    {
      if (!this._tools.Contains(tool))
      {
        this._tools.Add(tool);
        tool.Parent = this;
      }
    }

    public void UnregisterTool(PainterTool tool)
    {
      if (this._tools.Contains(tool))
      {
        this._tools.Remove(tool);
        tool.Parent = null;
      }
    }

    public void NewMesh()
    {
      this.PaintedMesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
      this.Palette.Clear();
    }

    public void MoveTool(int start, int end)
    {
      PainterTool item = this._tools[start];
      this._tools.RemoveAt(start);
      this._tools.Insert(end, item);
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Awake()
    {
      this.PaintedMesh = new MapVoxelMesh(new Dimensions3D(8, 8, 8));
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Start()
    {
      if (this._activeTool >= 0)
      {
        this._tools[this._activeTool].OnToolStart();
      }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public void Update() 
    {
      if(this.HasTool(this.ActiveTool))
      {
        this._tools[this.ActiveTool].OnToolUpdate();
      }
    }

    public bool HasTool(int i)
    {
      return i >= 0 && i < this._tools.Count;
    }
    
    public void SaveMesh()
    {
      //Ask for file
      //Don't compile
      var path = EditorUtility.SaveFilePanel(
              "Save Mesh",
              "",
              "Mesh" + ".vxl",
              "vxl");


      //Save Mesh to file
      VoxelFile.Save(PaintedMesh, path);
    }

    public void OpenMesh()
    {
      var file = EditorUtility.OpenFilePanel("", "", "vxl");

      VoxelMesh vm= VoxelFile.Load(file);
      if(vm != null)
          this.PaintedMesh = vm;

      this.PaintedMesh.Commit();

      this.Palette.Clear();

      if (vm != null)
        this.Palette.AnalyseVoxel(vm);
    }
  }
}