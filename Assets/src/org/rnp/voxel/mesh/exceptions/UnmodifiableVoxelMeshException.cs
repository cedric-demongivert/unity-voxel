﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.voxel.mesh.exceptions
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   An exception raised when a user try to modify a ReadOnly voxel mesh.
  /// </summary>
  public class UnmodifiableVoxelMeshException : Exception
  {
    /// <summary>
    ///   The unmodifiable mesh.
    /// </summary>
    public readonly IVoxelMesh UnmodifiableMesh;

    /// <summary>
    ///   Create a new UnmodifiableVoxelMesh exception.
    /// </summary>
    /// <param name="UnmodifiableMesh"></param>
    public UnmodifiableVoxelMeshException(IVoxelMesh UnmodifiableMesh)
      : base("Trying to modify a ReadOnly voxel mesh.") 
    {
      this.UnmodifiableMesh = UnmodifiableMesh;
    }
  }
}
