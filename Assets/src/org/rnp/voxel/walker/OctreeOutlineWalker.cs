using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;

namespace org.rnp.voxel.walker
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   An object that walk along an octree and find full blocks
  ///   of voxel.
  /// </summary>
  public class OctreeOutlineWalker
  {
    private IList<IVoxelMesh> stack;

    public OctreeOutlineWalker()
    {
      this.stack = new List<IVoxelMesh>();
    }

    public void SetRoot(IVoxelMesh mesh)
    {
      this.stack.Clear();
      this.stack.Add(mesh);
    }

    public IVoxelMesh Next()
    {
      return this.Walk();
    }

    protected IVoxelMesh Walk()
    {
      return null;
    }
  }
}
