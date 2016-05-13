using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.translator;
using org.rnp.voxel.utils;
using org.rnp.voxel;

public class Water3D : VoxelMeshContainer
{

    public VoxelMesh mesh = new MapVoxelMesh(new Dimensions3D(16, 16, 16));
    public VoxelRenderer vr;
    public MeshRenderer mr;

    public int width, height, depth;
    public Vector3 position;

    public float tickTime = 1;
    private float timeLeft;

    public override VoxelMesh Mesh
    {
        get
        {
            return mesh;
        }
    }

    // Use this for initialization
    void Start () {
        timeLeft = 0;
    }
	
	// Update is called once per frame
	void Update () {

        timeLeft += Time.deltaTime;

        if (timeLeft > tickTime)
        {
            while (timeLeft > tickTime)
            {
                this.Simulate();
                timeLeft -= tickTime;
            }
            mesh.Commit();
        }

        

	}


    public void Simulate()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int d = 0; d < depth; d++)
                {
                    VoxelLocation cell = new VoxelLocation(w, h, d);
                    VoxelLocation cellDown = cell.Sub(0, 1, 0);
                    if (IsAlive(cell))
                    {
                        //Debug.LogWarning("cell " + w.ToString() + "," + h.ToString() + "," + d.ToString());
                        //  Case du dessous vide 
                        if (IsDead(cellDown))
                        {
                            Live(cellDown);
                            Kill(cell);
                            return;
                        }
                        else
                        {
                            bool moved = false;/*
                            for (int i = w - 1; i <= w + 1; i++)
                            {
                                for (int k = d - 1; k <= d + 1; k++)
                                {
                                    //Debug.LogWarning("cell " + i.ToString() + "," + (h-1).ToString() + "," + k.ToString() + "  " + IsAlive(cell));
                                    if (IsDead(new VoxelLocation(i, h - 1, k)))
                                    {
                                        Live(new VoxelLocation(i, h - 1, k));
                                        Kill(cell);
                                        moved = true;
                                        break;
                                    }
                                }
                                if (moved) break;
                            }*/
                            if (!moved)
                            {
                                int mod = 1;
                                for (int j = h - 1; j <= h+1; j++)
                                {
                                    while (!moved
                                    && (w - mod > 0 || w + mod <= width)
                                    && (d - mod > 0 || d + mod <= depth))
                                    {/*
                                        for (int i = w - mod; i <= w + mod; i++)
                                        {
                                            for (int k = d - mod; k <= d + mod; k++)
                                            {
                                                if (IsDead(new VoxelLocation(i, j, k)))
                                                {
                                                    Live(new VoxelLocation(i, j, k));
                                                    Kill(cell);
                                                    moved = true;
                                                    break;
                                                }
                                            }
                                            if (moved) break;
                                        }
                                       */

                                        for(int i= w- mod;i<=w+ mod; i++)
                                        {
                                            if (IsDead(new VoxelLocation(i, j, d-mod)))
                                            {
                                                Live(new VoxelLocation(i, j, d - mod));
                                                Kill(cell);
                                                moved = true;
                                                return;
                                            }
                                            else if (IsDead(new VoxelLocation(i, j, d + mod)))
                                            {
                                                Live(new VoxelLocation(i, j, d + mod));
                                                Kill(cell);
                                                moved = true;
                                                return;
                                            }
                                        }
                                        if (!moved)
                                        {
                                            for (int k = d - mod;  k<= d + mod; k++)
                                            {
                                                if (IsDead(new VoxelLocation(w-mod, j, k)))
                                                {
                                                    Live(new VoxelLocation(w-mod, j, k));
                                                    Kill(cell);
                                                    moved = true;
                                                    return;
                                                }
                                                else if (IsDead(new VoxelLocation(w+mod, j, k)))
                                                {
                                                    Live(new VoxelLocation(w+mod, j, k));
                                                    Kill(cell);
                                                    moved = true;
                                                    return;
                                                }
                                            }
                                        }
                                        mod++;
                                    }
                                    if (moved) return;
                                }
                                
                            }

                            //Debug.LogWarning("-----------");
                        }
                    }
                }
            }
        }
    }




    public void createParticule(VoxelLocation _position)
    {
        Live(_position);
    }

    private void Live(VoxelLocation cell)
    {
        mesh[cell] = new Color32(0, 0, 255, 0);
    }

    private void Kill(VoxelLocation cell)
    {
        mesh[cell] = Voxels.Empty;
    }

    private bool IsAlive(VoxelLocation locToTest)
    {
        return Voxels.HasNegative(locToTest)
              || locToTest.X > (position.x + width)
              || locToTest.Y > (position.y + height)
              || locToTest.Z > (position.z + depth)
              || Voxels.IsNotEmpty(mesh[locToTest]);
    }

    private bool IsDead(VoxelLocation locToTest)
    {
        return !IsAlive(locToTest);
    }

}
