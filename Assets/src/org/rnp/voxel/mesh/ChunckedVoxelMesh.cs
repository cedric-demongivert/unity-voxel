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
  public abstract class ChunckedVoxelMesh : VoxelMesh
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
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public abstract ChunckVoxelMesh GetChunck(int x, int y, int z);

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use chunck's coordinates, in other words if you
    ///   want the chunck at (1,0,1) you will obtain the sub-mesh at location
    ///   (chunckWidth, 0, chunckDepth) of dimensions ChunckDimensions.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public abstract ChunckVoxelMesh GetChunck(VoxelLocation location);

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use world coordinates, it will returns the chunck that
    ///   contains the voxel at the specified location.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual ChunckVoxelMesh GetChunckAt(int x, int y, int z)
    {
      return this.GetChunckAt(new VoxelLocation(x, y, z));
    }

    /// <summary>
    ///   Return a chunck of the mesh.
    ///   
    ///   This methods use world coordinates, it will returns the chunck that
    ///   contains the voxel at the specified location.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual ChunckVoxelMesh GetChunckAt(VoxelLocation location)
    {
      return this.GetChunck(location.Div(this.ChunckDimensions));
    }

    /// <summary>
    ///   Convert a chunck-relative location to a world location.
    /// </summary>
    /// <param name="cx"></param>
    /// <param name="cy"></param>
    /// <param name="cz"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToWorldLocation(int cx, int cy, int cz, int x, int y, int z)
    {
      return new VoxelLocation(cx, cy, cz).Mul(this.ChunckDimensions).Add(x, y, z);
    }

    /// <summary>
    ///   Convert a chunck-relative location to a world location.
    /// </summary>
    /// <param name="chunckLocation"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToWorldLocation(VoxelLocation chunckLocation, int x, int y, int z)
    {
      return chunckLocation.Mul(this.ChunckDimensions).Add(x, y, z);
    }

    /// <summary>
    ///   Convert a chunck-relative location to a world location.
    /// </summary>
    /// <param name="chunckLocation"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public virtual VoxelLocation ToWorldLocation(VoxelLocation chunckLocation, VoxelLocation location)
    {
      return chunckLocation.Mul(this.ChunckDimensions).Add(location);
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
