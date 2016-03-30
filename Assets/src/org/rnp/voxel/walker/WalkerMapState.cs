using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Walk along a map.
  /// </summary>
  public class WalkerMapState : IWalkerState
  {
    private IMapVoxelMesh _node;

    private HashSet<VoxelLocation> _nodeKeys;

    private VoxelLocation _location;

    private VoxelLocation _lastKey;

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public VoxelLocation Location
    {
      get { return _location; }
      set { this._location = value; }
    }

    /// <summary>
    ///   Return the map version of the node.
    /// </summary>
    public IMapVoxelMesh MapNode
    {
      get { return this._node; }
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
    public WalkerMapState(IMapVoxelMesh node)
    {
      this._node = node;
      this._nodeKeys = this._node.Keys();
      this._location = VoxelLocation.Zero;
      this._lastKey = null;
    }

    /// <summary>
    ///   Create a copy of an existing class.
    /// </summary>
    /// <param name="toCopy"></param>
    public WalkerMapState(WalkerMapState toCopy)
    {
      this._node = toCopy._node;
      this._nodeKeys = new HashSet<VoxelLocation>(toCopy._nodeKeys);
      this._location = new VoxelLocation(toCopy._location);
      this._lastKey = null;
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelMesh Next()
    {
      if (this._nodeKeys.Count <= 0)
      {
        this._lastKey = null;
        return null;
      }

      this._lastKey = this._nodeKeys.ElementAt(0);
      this._nodeKeys.Remove(this._lastKey);

      return this._node.GetChild(this._lastKey.X, this._lastKey.Y, this._lastKey.Z);
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public VoxelLocation GetLocation()
    {
      if(this._lastKey == null)
      {
        return null;
      }
      else 
      {
        return new VoxelLocation(this._lastKey)
                   .Mul(this._node.ChildWidth, this._node.ChildHeight, this._node.ChildDepth)
                   .Add(this._location);
      }
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"/>
    public IWalkerState Copy()
    {
      return new WalkerMapState(this);
    }
  }
}
