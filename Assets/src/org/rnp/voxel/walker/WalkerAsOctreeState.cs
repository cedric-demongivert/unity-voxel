using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.submesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Simulate a walk along an octree.
  /// </summary>
  public class WalkerAsOctreeState : IWalkerState
  {
    private IVoxelMesh _node;

    private IVoxelLocation _location;

    private int _cursor;

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelLocation Location
    {
      get { return _location; }
      set { this._location = value; }
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelMesh Node
    {
      get { return this._node; }
    }

    /// <summary>
    ///   Create a new walker state.
    /// </summary>
    /// <param name="node"></param>
    public WalkerAsOctreeState(IVoxelMesh node)
    {
      this._node = node;
      this._cursor = -1;
      this._location = VoxelLocation.Zero;
    }

    /// <summary>
    ///   Create a copy of an existing class.
    /// </summary>
    /// <param name="toCopy"></param>
    public WalkerAsOctreeState(WalkerAsOctreeState toCopy)
    {
      this._node = toCopy._node;
      this._cursor = toCopy._cursor;
      this._location = new VoxelLocation(toCopy._location);
    }

    /// <summary>
    ///   Return child dimensions.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    protected IDimensions3D GetDimensionsFor(int x, int y, int z)
    {
      Dimensions3D childDimensions = new Dimensions3D();

      if (x == 0)
      {
        childDimensions.Width = this._node.Width / 2;
      }
      else
      {
        childDimensions.Width = this._node.Width - (this._node.Width / 2);
      }

      if (y == 0)
      {
        childDimensions.Height = this._node.Height / 2;
      }
      else
      {
        childDimensions.Height = this._node.Height - (this._node.Height / 2);
      }

      if (z == 0)
      {
        childDimensions.Depth = this._node.Depth / 2;
      }
      else
      {
        childDimensions.Depth = this._node.Depth - (this._node.Depth / 2);
      }

      return childDimensions;
    }

    /// <summary>
    ///   Return offset for a child.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    protected IVoxelLocation GetStartFor(int x, int y, int z)
    {
      VoxelLocation start = new VoxelLocation();

      if (x == 1)
      {
        start.X = this._node.Width / 2;
      }

      if (y == 1)
      {
        start.Y = this._node.Height / 2;
      }

      if (z == 1)
      {
        start.Z = this._node.Depth / 2;
      }

      return start;
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelMesh Next()
    {
      if (this._cursor < 8) this._cursor += 1;
      if (this._cursor > 7) return null;

      int x = this._cursor % 2;
      int y = (this._cursor / 2) % 2;
      int z = (this._cursor / 4) % 2;

      IVoxelMesh mesh = new SubMesh(
        this._node,
        this.GetStartFor(x,y,z),
        this.GetDimensionsFor(x,y,z)
      );
      
      if (mesh.IsEmpty()) return this.Next();
      else return mesh;
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelLocation GetLocation()
    {
      if (this._cursor < 0 || this._cursor > 7)
      {
        return null;
      }
      else
      {
        int x = this._cursor % 2;
        int y = (this._cursor / 2) % 2;
        int z = (this._cursor / 4) % 2;

        return this.GetStartFor(x, y, z).Add(this._location);
      }
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"/>
    public IWalkerState Copy()
    {
      return new WalkerAsOctreeState(this);
    }
  }
}
