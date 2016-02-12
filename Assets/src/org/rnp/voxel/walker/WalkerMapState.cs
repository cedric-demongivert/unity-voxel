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

    private HashSet<IVoxelLocation> _nodeKeys;

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
    }

    /// <summary>
    ///   Create a copy of an existing class.
    /// </summary>
    /// <param name="toCopy"></param>
    public WalkerMapState(WalkerMapState toCopy)
    {
      this._node = toCopy._node;
      this._nodeKeys = new HashSet<IVoxelLocation>(toCopy._nodeKeys);
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelMesh Next()
    {
      if (this._nodeKeys.Count <= 0) return null;

      IVoxelLocation nextKey = this._nodeKeys.ElementAt(0);
      this._nodeKeys.Remove(nextKey);

      return this._node.GetChild(nextKey.X, nextKey.Y, nextKey.Z);
    }
  }
}
