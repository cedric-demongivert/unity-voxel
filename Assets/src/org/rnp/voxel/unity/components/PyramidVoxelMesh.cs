using org.rnp.voxel.mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace org.rnp.voxel.unity.components
{
    /// <author>Arwine GOPITHANDIRANE [arwine.gopitchandirane@gmail.com]</author>
    /// <summary>
    ///   A simple, procedural, pyramidal voxel mesh
    /// </summary>
    [ExecuteInEditMode]
    public class PyramidVoxelMesh : VoxelMesh
    {
        /// <summary>
        ///   Pyramid Height in voxels.
        /// </summary>
        [SerializeField]
        protected int _Height;

        /// <summary>
        ///   Pyramid color.
        /// </summary>
        [SerializeField]
        protected Color32 _Color;

        /// <summary>
        ///   Pyramid Height in voxels.
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
        ///   Sphere color.
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
            int size = this.Height + (this.Height -1);
            int modify_size = size;
            Color32 empty = new Color32(0, 0, 0, 255);

            VoxelArray pyramid = new VoxelArray(size, size, size);

            for (int y = 0; y < this.Height; ++y)
            {
                for (int x = size - modify_size; x < modify_size; ++x)
                {
                    for (int z = size - modify_size; z < modify_size; ++z)
                    {
                        Vector3 point = this.GetPoint(x, y, z);

                        pyramid[x, y, z] = this.Color;
                    }
                }
                modify_size -= 1;
            }

            this.Mesh = pyramid;
        }
    }
}
