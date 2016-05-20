using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  ///
  /// <summary>
  ///   Utility methods for voxel meshes.
  /// </summary>
  public sealed class VoxelMeshes
  {
    /// <summary>
    ///   Copy a part of a voxel mesh and past it in another location.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="toPaste"></param>
    public static void Copy(VoxelMesh toCopy, VoxelMesh toPaste)
    {
      VoxelMeshes.Copy(
        toCopy,
        toPaste,
        toCopy.Start,
        toCopy.Start.Add(toCopy.Dimensions),
        toPaste.Start
      );
    }

    /// <summary>
    ///   Copy a part of a voxel mesh and past it in another location.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="toPaste"></param>
    /// <param name="where"></param>
    public static void Copy(VoxelMesh toCopy, VoxelMesh toPaste, VoxelLocation where)
    {
      VoxelMeshes.Copy(
        toCopy,
        toPaste,
        toCopy.Start,
        toCopy.Start.Add(toCopy.Dimensions),
        where
      );
    }

    /// <summary>
    ///   Copy a part of a voxel mesh and past it in another location.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="toPaste"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static void Copy(VoxelMesh toCopy, VoxelMesh toPaste, VoxelLocation from, VoxelLocation to)
    {
      VoxelMeshes.Copy(
        toCopy,
        toPaste,
        from,
        to,
        toPaste.Start
      );
    }

    /// <summary>
    ///   Copy a part of a voxel mesh and past it in another location.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="toPaste"></param>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="where"></param>
    public static void Copy(VoxelMesh toCopy, VoxelMesh toPaste, VoxelLocation from, VoxelLocation to, VoxelLocation where)
    {
      VoxelMeshes.Copy(
        toCopy, 
        toPaste, 
        from,
        new Dimensions3D(from, to), 
        where);
    }

    /// <summary>
    ///   Copy a part of a voxel mesh and past it in another location.
    /// </summary>
    /// <param name="toCopy"></param>
    /// <param name="toPaste"></param>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="where"></param>
    public static void Copy(VoxelMesh toCopy, VoxelMesh toPaste, VoxelLocation start, Dimensions3D size, VoxelLocation where)
    {
      for (int x = 0; x < size.Width; ++x)
      {
        for (int y = 0; y < size.Height; ++y)
        {
          for (int z = 0; z < size.Depth; ++z)
          {
            toPaste[where.X + x, where.Y + y, where.Z + z] = toCopy[start.X + x, start.Y + y, start.Z + z];
          }
        }
      }
    }

    /// <summary>
    ///   Fill a part of the voxel mesh with a color.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    public static void Fill(VoxelMesh mesh, VoxelLocation start, Dimensions3D size, Color32 color)
    {
      for (int x = 0; x < size.Width; ++x)
      {
        for (int y = 0; y < size.Height; ++y)
        {
          for (int z = 0; z < size.Depth; ++z)
          {
            mesh.Set(x + start.X, y + start.Y, z + start.Z, color);
          }
        }
      }
    }

    /// <summary>
    ///   Fill a part of the voxel mesh with a color.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color"></param>
    public static void Fill(VoxelMesh mesh, VoxelLocation start, VoxelLocation end, Color32 color)
    {
      VoxelLocation min = start.SetIfMin(end);
      VoxelLocation max = start.SetIfMax(end);

      VoxelMeshes.Fill(
        mesh,
        min,
        new Dimensions3D(min, max),
        color
      );
    }

    /// <summary>
    ///   Fill a part of the voxel mesh with a color.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="start"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    public static void FillInclusive(VoxelMesh mesh, VoxelLocation start, Dimensions3D size, Color32 color)
    {
      for (int x = 0; x <= size.Width; ++x)
      {
        for (int y = 0; y <= size.Height; ++y)
        {
          for (int z = 0; z <= size.Depth; ++z)
          {
            mesh.Set(x + start.X, y + start.Y, z + start.Z, color);
          }
        }
      }
    }

    /// <summary>
    ///   Fill a part of the voxel mesh with a color.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="color"></param>
    public static void FillInclusive(VoxelMesh mesh, VoxelLocation start, VoxelLocation end, Color32 color)
    {
      VoxelLocation min = start.SetIfMin(end);
      VoxelLocation max = start.SetIfMax(end);

      VoxelMeshes.FillInclusive(
        mesh,
        min,
        new Dimensions3D(min, max),
        color
      );
    }

    /// <summary>
    ///   Fill a mesh with a color.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="color"></param>
    public static void Fill(VoxelMesh mesh, Color32 color)
    {
      VoxelMeshes.Fill(
        mesh, 
        mesh.Start, 
        mesh.Dimensions, 
        color
      );
    }
  }
}
