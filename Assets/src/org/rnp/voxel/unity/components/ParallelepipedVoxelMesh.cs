using org.rnp.voxel.mesh.array;
using org.rnp.voxel.unity.components.meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components
{
    /// <author>Arwine GOPITHANDIRANE [arwine.gopitchandirane@gmail.com]</author>
    /// <summary>
    ///   A simple, procedural, parallelepiped voxel mesh
    /// </summary>
    [ExecuteInEditMode]
    public class ParallelepipedVoxelMesh : VoxelMesh
    {
        /// <summary>
        ///   Parallelepiped Height in voxels.
        /// </summary>
        [SerializeField]
        protected int _Height;

        /// <summary>
        ///   Parallelepiped Width in voxels.
        /// </summary>
        [SerializeField]
        protected int _Width;

        /// <summary>
        ///   Parallelepiped Depth in voxels.
        /// </summary>
        [SerializeField]
        protected int _Depth;

        /// <summary>
        ///   Parallelepiped color.
        /// </summary>
        [SerializeField]
        protected Color32 _Color;

        /// <summary>
        ///   Parallelepiped Height in voxels.
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                this._Height = value;
                this.RefreshMesh();
            }
        }

        /// <summary>
        ///   Parallelepiped Width in voxels.
        /// </summary>
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                this._Width = value;
                this.RefreshMesh();
            }
        }

        /// <summary>
        ///   Parallelepiped Depth in voxels.
        /// </summary>
        public int Depth
        {
            get
            {
                return _Depth;
            }
            set
            {
                this._Depth = value;
                this.RefreshMesh();
            }
        }

        /// <summary>
        ///   Parallelepiped color.
        /// </summary>
        public Color32 Color
        {
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

        /// <summary>
        ///   Return a point in space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        protected Vector3 GetPoint(int x, int y, int z)
        {
            return (new Vector3(
              (float)(x - this.Height) + 0.5f,
              (float)(y - this.Height) + 0.5f,
              (float)(z - this.Height) + 0.5f
            ));
        }

        /// <summary>
        ///   Rebuild the voxel mesh.
        /// </summary>
        public void RefreshMesh()
        {
            int width = this.Width;
            int height = this.Height;
            int depth = this.Depth;

            Color32 empty = new Color32(0, 0, 0, 255);

            ArrayVoxelMesh parallelepiped = new ArrayVoxelMesh(width, height, depth);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int z = 0; z < depth; ++z)
                    {
                        parallelepiped[x, y, z] = this.Color;
                    }
                }
            }

            this.Mesh = parallelepiped;
        }
    }
}
