<<<<<<< HEAD:Assets/src/org/rnp/voxel/unity/components/SphereVoxelMesh.cs
﻿    using org.rnp.voxel.mesh;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    namespace org.rnp.voxel.unity.components
    {
        /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
        /// <summary>
        ///   A simple, procedural, spheric voxel mesh
        /// </summary>
        [ExecuteInEditMode]
        public class SphereVoxelMesh : VoxelMesh
        {
        /// <summary>
        ///   Sphere radius in voxels.
        /// </summary>
        [SerializeField]
        protected int _Radius;

        /// <summary>
        ///   Sphere color.
        /// </summary>
        [SerializeField]
        protected Color32 _Color;
=======
﻿using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.rnp.voxel.mesh.map;
using UnityEngine;
using org.rnp.voxel.utils;

namespace org.rnp.voxel.unity.components.meshes
{
  /// <author>Cédric DEMONGIVERT [cedric.demongivert@gmail.com]</author>
  /// <summary>
  ///   A simple, procedural, spheric voxel mesh
  /// </summary>
  [ExecuteInEditMode]
  public sealed class SphereVoxelMesh : VoxelMesh
  {
    /// <summary>
    ///   Sphere radius in voxels.
    /// </summary>
    [SerializeField]
    private int _Radius;

    /// <summary>
    ///   Sphere color.
    /// </summary>
    [SerializeField]
    private Color32 _Color;
>>>>>>> 534af84b7fc5ad7f2ff98112c32db95d7cd64c22:Assets/src/org/rnp/voxel/unity/components/meshes/SphereVoxelMesh.cs


        /// <summary>
        ///   Sphere radius in voxels.
        /// </summary>
        public int Radius {
        get
        {
        return _Radius;
        }
        set
        {
        this._Radius = value;
        this.RefreshMesh();
        }
    }

    /// <summary>
    ///   Sphere color.
    /// </summary>
    public Color32 Color {
        get
        {
        return _Color;
        }
        set
        {
        this._Color = value;
        this.RefreshMesh();
        }
    }

    /// <see cref="http://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
    public override void Awake()
    {
        this.RefreshMesh();
    }

    private Vector3 GetPoint(int x, int y, int z)
    {
<<<<<<< HEAD:Assets/src/org/rnp/voxel/unity/components/SphereVoxelMesh.cs
        return (new Vector3(
        (float)(x - this.Radius) + 0.5f,
        (float)(y - this.Radius) + 0.5f,
        (float)(z - this.Radius) + 0.5f
        ));
=======
      Vector3 point = new Vector3(
        x - this.Radius,
        y - this.Radius,
        z - this.Radius
      );

      point.x += 0.5f;
      point.y += 0.5f;
      point.z += 0.5f;

      return point;
>>>>>>> 534af84b7fc5ad7f2ff98112c32db95d7cd64c22:Assets/src/org/rnp/voxel/unity/components/meshes/SphereVoxelMesh.cs
    }

    /// <summary>
    ///   Rebuild the voxel mesh.
    /// </summary>
    public void RefreshMesh()
    {
<<<<<<< HEAD:Assets/src/org/rnp/voxel/unity/components/SphereVoxelMesh.cs
        
        int size = this.Radius * 2;
        Color32 empty = new Color32(0, 0, 0, 255);

        VoxelArray sphere = new VoxelArray(size, size, size);

        Color32 currColor = _Color;

        for (int x = 0; x < size; ++x) 
        {
            for(int y = 0; y < size; ++y) 
            {
                for(int z = 0; z < size; ++z) 
                {
                    Vector3 point = this.GetPoint(x, y, z);

                    if (point.magnitude <= this.Radius)
                    {
                        int newR= currColor.r, newG= currColor.g, newB= currColor.b;

                        sphere[x, y, z] = new Color32((byte)(newR), (byte)(newG), (byte)(newB), 0);
                    }
                    else 
                    {
                        sphere[x, y, z] = empty;
                    }
                }
=======
      int size = this.Radius * 2;

      IVoxelMesh sphere = new MapVoxelMesh();
      
      for(int x = 0; x < size; ++x) 
      {
        for(int y = 0; y < size; ++y) 
        {
          for(int z = 0; z < size; ++z) 
          {
            Vector3 point = this.GetPoint(x, y, z);

            if (point.magnitude <= this.Radius)
            {
              sphere[x - this.Radius, y - this.Radius, z - this.Radius] = this.Color;
            }
            else 
            {
              sphere[x - this.Radius, y - this.Radius, z - this.Radius] = Voxels.Empty;
>>>>>>> 534af84b7fc5ad7f2ff98112c32db95d7cd64c22:Assets/src/org/rnp/voxel/unity/components/meshes/SphereVoxelMesh.cs
            }
        }

        this.Mesh = sphere;
    }
    }
}
