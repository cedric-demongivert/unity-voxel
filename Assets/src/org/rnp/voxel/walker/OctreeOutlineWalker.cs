using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.mesh.map;
using org.rnp.voxel.mesh.octree;
using org.rnp.voxel.mesh.absolute;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An object that walk along an octree and find full blocks
  ///   of voxel.
  /// </summary>
  public class OctreeOutlineWalker : ICopiable<OctreeOutlineWalker>
  {
    private IList<IWalkerState> stack;

    /// <summary>
    ///   Create a new empty walker.
    /// </summary>
    public OctreeOutlineWalker()
    {
      this.stack = new List<IWalkerState>();
    }

    /// <summary>
    ///   Copy an existing walker.
    /// </summary>
    /// <param name="toCopy"></param>
    public OctreeOutlineWalker(OctreeOutlineWalker toCopy)
    {
      this.stack = new List<IWalkerState>();

      for (int i = 0; i < toCopy.stack.Count; ++i)
      {
        IWalkerState stateToCopy = toCopy.stack[i];
        this.stack.Add(stateToCopy.Copy());
      }
    }

    /// <summary>
    ///   Set a mesh for walk.
    /// </summary>
    /// <param name="mesh"></param>
    public void SetRoot(IVoxelMesh mesh)
    {
      this.stack.Clear();
      this.AddState(mesh);
    }

    /// <summary>
    ///   Add a mesh as a walker state in the state stack.
    /// </summary>
    /// <param name="mesh"></param>
    protected void AddState(IVoxelMesh mesh)
    {
      this.AddState(VoxelLocation.Zero, mesh);
    }

    /// <summary>
    ///   Add a mesh as a walker state in the state stack.
    /// </summary>
    /// <param name="mesh"></param>
    protected void AddState(VoxelLocation location, IVoxelMesh mesh)
    {
      if (mesh.IsEmpty()) return;

      if (mesh is IMapVoxelMesh)
      {
        this.stack.Add(new WalkerMapState((IMapVoxelMesh)mesh));
      }
      else if (mesh is IOctreeVoxelMesh)
      {
        this.stack.Add(new WalkerOctreeState((IOctreeVoxelMesh)mesh));
      }
      else
      {
        this.stack.Add(new WalkerAsOctreeState(mesh));
      }

      this.GetLastState().Location = location;
    }

    /// <summary>
    ///   Return the next captured full voxel mesh.
    /// </summary>
    /// <returns></returns>
    public IVoxelMesh Next()
    {
      return this.Walk();
    }

    /// <summary>
    ///   Walk other a voxel mesh.
    /// </summary>
    /// <returns></returns>
    protected IVoxelMesh Walk()
    {
      while (this.stack.Count > 0)
      {
        IWalkerState state = this.GetLastState();
        IVoxelMesh mesh = state.Next();

        if (mesh == null)
        {
          this.RemoveLastState();
        }
        else
        {
          if (mesh.IsFull())
          {
            return new AbsoluteVoxelMesh(mesh, state.GetLocation());
          }
          else
          {
            this.AddState(state.GetLocation(), mesh);
          }
        }
      }

      return null;
    }

    /// <summary>
    ///   Remove the last added state.
    /// </summary>
    protected void RemoveLastState()
    {
      this.stack.RemoveAt(this.stack.Count - 1);
    }

    /// <summary>
    ///   Get the last added state.
    /// </summary>
    /// <returns></returns>
    protected IWalkerState GetLastState()
    {
      return this.stack[this.stack.Count - 1];
    }

    /// <see cref="org.rnp.voxel.utils.ICopiable"/>
    public OctreeOutlineWalker Copy()
    {
      return new OctreeOutlineWalker(this);
    }
  }
}
