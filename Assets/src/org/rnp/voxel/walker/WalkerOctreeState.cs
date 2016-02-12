using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   Walk along an octree.
  /// </summary>
  public class WalkerOctreeState : IWalkerState
  {
    private IOctreeVoxelMesh _node;

    private int _cursor;

    /// <summary>
    ///   Return the octree version of the node.
    /// </summary>
    public IOctreeVoxelMesh OctreeNode
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
    public WalkerOctreeState(IOctreeVoxelMesh node)
    {
      this._node = node;
      this._cursor = 0;
    }

    /// <summary>
    ///   Create a copy of an existing class.
    /// </summary>
    /// <param name="toCopy"></param>
    public WalkerOctreeState(WalkerOctreeState toCopy)
    {
      this._node = toCopy._node;
      this._cursor = toCopy._cursor;
    }

    /// <see cref="org.rnp.voxel.walker.IWalkerState"/>
    public IVoxelMesh Next()
    {
      if (this._cursor > 7) return null;

      IVoxelMesh child = this._node.GetChild( 
        this._cursor % 2,
        (this._cursor/2) % 2,
        (this._cursor/4) % 2
      );

      this._cursor += 1;

      if(child == null) {
        return this.Next();
      }
      else {
        return child;
      }
    }
  }
}
