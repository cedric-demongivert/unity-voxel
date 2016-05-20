using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.translator
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   A class that manage voxel mesh translations.
  /// </summary>
  public sealed class Translators
  {
    /// <summary>
    ///   Translation configuration.
    /// </summary>
    private Dictionary<string, Dictionary<Type, Type>> _translators;
    
    /// <summary>
    ///   Singleton pattern.
    /// </summary>
    private static Translators _singleton;

    /// <summary>
    ///   Return a translators singleton.
    /// </summary>
    /// <returns></returns>
    public static Translators Instance()
    {
      if (_singleton == null)
      {
        _singleton = new Translators();
      }

      return _singleton;
    }

    /// <summary>
    ///   Generic translators.
    /// </summary>
    public Translators ()
    {
      this._translators = new Dictionary<string, Dictionary<Type, Type>>();
      this.Initialize(typeof(Translators).Assembly);
    }

    /// <summary>
    ///   Add a translator.
    /// </summary>
    /// <param name="style"></param>
    /// <param name="voxelMeshType"></param>
    /// <param name="translatorType"></param>
    private void Register(string style, Type voxelMeshType, Type translatorType)
    {
      this.CreateStyle(style);
      this.GetForStyle(style)[voxelMeshType] = translatorType;
    }

    /// <summary>
    ///   Create a style if not exist.
    /// </summary>
    /// <param name="style"></param>
    private void CreateStyle(string style)
    {
      if (!this._translators.ContainsKey(style))
      {
        this._translators[style] = new Dictionary<Type, Type>();
      }
    }

    /// <summary>
    ///   True if the style exist.
    /// </summary>
    /// <param name="style"></param>
    /// <returns></returns>
    public bool ExistStyle(string style)
    {
      return this._translators.ContainsKey(style.ToLower());
    }

    /// <summary>
    ///   Return a list of translators for a specific style collection.
    /// </summary>
    /// <param name="style"></param>
    /// <returns></returns>
    private Dictionary<Type, Type> GetForStyle(string style)
    {
      return this._translators[style.ToLower()];
    }

    /// <summary>
    ///   Load translators configuration from an assembly.
    /// </summary>
    /// <param name="assembly"></param>
    public void Initialize(Assembly assembly)
    {
      foreach(Type type in assembly.GetTypes())
      {
        foreach (Translate attribute in type.GetCustomAttributes(typeof(Translate), false))
        {
          this.Register(attribute.Style.ToLower(), attribute.Processable, type);
        }
      }
    }

    /// <summary>
    ///   Instanciate a translator for a voxel mesh.
    /// </summary>
    /// <param name="style"></param>
    /// <param name="meshToTranslate"></param>
    /// <param name="worldMesh"></param>
    /// <returns></returns>
    public Translator Generate(string style, VoxelMesh meshToTranslate, VoxelMesh worldMesh = null)
    {
      if (!this.ExistStyle(style))
      {
        return null;
      }

      Type translator = this.GetTranslatorFor(style, meshToTranslate);

      if (translator == null)
      {
        return null;
      }

      GameObject result = new GameObject("Genered Translator");
      result.AddComponent(translator);

      Translator componentTranslator = result.GetComponent<Translator>();
      componentTranslator.Initialize(meshToTranslate, worldMesh);

      return componentTranslator;
    }

    /// <summary>
    ///   Get the translator for a specific voxel mesh.
    /// </summary>
    /// <param name="style"></param>
    /// <param name="meshToTranslate"></param>
    /// <returns></returns>
    private Type GetTranslatorFor(string style, VoxelMesh meshToTranslate)
    {
      Dictionary<Type, Type> translators = this.GetForStyle(style);
      Type selected = null;

      foreach (Type voxelMeshType in translators.Keys)
      {
        if (voxelMeshType.IsInstanceOfType(meshToTranslate) && (selected == null || voxelMeshType.IsSubclassOf(selected)))
        {
          selected = voxelMeshType;
        }
      }

      if (selected == null)
      {
        return null;
      }
      else
      {
        return translators[selected];
      }
    }

    /// <summary>
    ///   Return a list of available styles.
    /// </summary>
    /// <returns></returns>
    public string[] AvailableStyles()
    {
      return _translators.Keys.ToArray();
    }
  }
}
