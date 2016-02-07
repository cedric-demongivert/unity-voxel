using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.mesh.map
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// 
  /// <summary>
  ///   A infinite voxel mesh based on a map, not writable.
  /// </summary>
  public sealed class ReadonlyMapVoxelMesh : AbstractReadonlyVoxelMesh<IMapVoxelMesh>, IMapVoxelMesh
  {
    /// <summary>
    ///   Wrap in a readonly implementation, a writable voxel map implementation.
    /// </summary>
    /// <param name="writableMap"></param>
    public ReadonlyMapVoxelMesh(IMapVoxelMesh writableMap) 
      : base(writableMap)
    { }

    /// <summary>
    ///   Copy an existing readonly implementation.
    /// </summary>
    /// <param name="toCopy"></param>
    public ReadonlyMapVoxelMesh(ReadonlyMapVoxelMesh toCopy)
      : base((AbstractReadonlyVoxelMesh<IMapVoxelMesh>) toCopy)
    { }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IVoxelMesh Copy()
    {
      return new ReadonlyMapVoxelMesh(this);
    }

    /// <see cref="org.rnp.voxel.mesh.IVoxelMesh"/>
    public override IReadonlyVoxelMesh ReadOnly()
    {
      return this;
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public HashSet<IVoxelLocation> Keys()
    {
      return this._writableMesh.Keys();
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      return this._writableMesh.GetChild(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(IVoxelLocation location)
    {
      return this._writableMesh.GetChild(location);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IVoxelLocation ToLocale(int x, int y, int z)
    {
      return this._writableMesh.ToLocale(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildWidth
    {
      get { return this._writableMesh.ChildWidth; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildHeight
    {
      get { return this._writableMesh.ChildHeight; }
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public int ChildDepth
    {
      get { return this._writableMesh.ChildDepth; }
    }
  }
}
