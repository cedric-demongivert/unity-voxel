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
  ///   A voxel mesh made of many chuncks that are sub-meshes of a specific dimension.
  /// </summary>
  public abstract class ChunckVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Dimensions of each chunks of the mesh.
    /// </summary>
    public abstract Dimensions3D ChunckDimensions
    {
      get;
    }

    /// <summary>
    ///   Iterate over each chuncks location of the mesh.
    /// </summary>
    public abstract IEnumerable<VoxelLocation> ChunckLocations {
      get;
    }

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use chunck's coordinates, in other words if you
    ///   want the chunck at (1,0,1) you will obtain the sub-mesh at location
    ///   (chunckWidth, 0, chunckDepth) of dimensions ChunckDimensions.
    ///   
    ///   Each returned chunck are in a readonly state, if you want to modify a chunck,
    ///   you have to modify the parent mesh directly.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public abstract VoxelMesh GetChunck(int x, int y, int z);

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use chunck's coordinates, in other words if you
    ///   want the chunck at (1,0,1) you will obtain the sub-mesh at location
    ///   (chunckWidth, 0, chunckDepth) of dimensions ChunckDimensions.
    ///   
    ///   Each returned chunck are in a readonly state, if you want to modify a chunck,
    ///   you have to modify the parent mesh directly.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public abstract VoxelMesh GetChunck(VoxelLocation location);

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use world coordinates, it will returns the chunck that
    ///   contains the voxel at the specified location.
    ///   
    ///   Each returned chunck are in a readonly state, if you want to modify a chunck,
    ///   you have to modify the parent mesh directly.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelMesh GetChunckAt(int x, int y, int z)
    {
      return this.GetChunckAt(new VoxelLocation(x, y, z));
    }

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use world coordinates, it will returns the chunck that
    ///   contains the voxel at the specified location.
    ///   
    ///   Each returned chunck are in a readonly state, if you want to modify a chunck,
    ///   you have to modify the parent mesh directly.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual VoxelMesh GetChunckAt(VoxelLocation location)
    {
      return this.GetChunck(location.Div(this.ChunckDimensions));
    }

    /// <summary>
    ///   Convert a world location to a chunck-relative location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToLocale(int x, int y, int z)
    {
      return this.ToLocale(new VoxelLocation(x, y, z));
    }

    /// <summary>
    ///   Transform a world location to a chunck location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToChunckLocation(int x, int y, int z)
    {
      Dimensions3D chunckDimensions = this.ChunckDimensions;

      return new VoxelLocation(
        x / (float)chunckDimensions.Width,
        y / (float)chunckDimensions.Height,
        z / (float)chunckDimensions.Depth
      );
    }

    /// <summary>
    ///   Transform a world location to a chunck location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToChunckLocation(VoxelLocation location)
    {
      Dimensions3D chunckDimensions = this.ChunckDimensions;

      return new VoxelLocation(
        location.X / (float)chunckDimensions.Width,
        location.Y / (float)chunckDimensions.Height,
        location.Z / (float)chunckDimensions.Depth
      );
    }

    /// <summary>
    ///   Convert a world location to a chunck-relative location.
    /// </summary>
    /// <param name="worldLocation"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToLocale(VoxelLocation worldLocation)
    {
      Dimensions3D chunckDimensions = this.ChunckDimensions;
      VoxelLocation location = worldLocation.Mod(chunckDimensions);

      return location.Add(
        (location.X < 0) ? chunckDimensions.Width : 0,
        (location.Y < 0) ? chunckDimensions.Height : 0,
        (location.Z < 0) ? chunckDimensions.Depth : 0
      );
    }
  }
}
