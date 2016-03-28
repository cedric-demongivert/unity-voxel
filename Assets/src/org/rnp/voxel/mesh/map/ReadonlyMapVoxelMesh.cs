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
  public sealed class ReadonlyMapVoxelMesh : ReadonlyVoxelMesh, IMapVoxelMesh
  {
    private IMapVoxelMesh _writableMesh;

    /// <summary>
    ///   Wrap in a readonly implementation, a writable voxel map implementation.
    /// </summary>
    /// <param name="writableMap"></param>
    public ReadonlyMapVoxelMesh(IMapVoxelMesh writableMap) 
      : base(writableMap)
    {
      this._writableMesh = writableMap;
    }

    public ReadonlyMapVoxelMesh(ReadonlyMapVoxelMesh toCopy)
      : base(toCopy)
    {
      this._writableMesh = toCopy._writableMesh;
    }

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
    public HashSet<VoxelLocation> Keys()
    {
      return this._writableMesh.Keys();
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(int x, int y, int z)
    {
      return this._writableMesh.GetChild(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public IReadonlyVoxelMesh GetChild(VoxelLocation location)
    {
      return this._writableMesh.GetChild(location);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public VoxelLocation ToLocale(int x, int y, int z)
    {
      return this._writableMesh.ToLocale(x, y, z);
    }

    /// <see cref="org.rnp.voxel.mesh.map.IMapVoxelMesh"/>
    public Dimensions3D ChildDimensions
    {
      get { return this._writableMesh.ChildDimensions; }
    }
  }
}
