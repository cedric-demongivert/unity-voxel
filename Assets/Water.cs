using UnityEngine;
using System.Collections;
using org.rnp.voxel.translator;
using org.rnp.voxel.utils;
using org.rnp.voxel.mesh;

public class Water : MonoBehaviour {

    public VoxelFilter vf;
    public VoxelRenderer vr;
    public MeshRenderer mr;

    public int width, height;
    public Vector3 position;

    private static int[] table = new int[] {
        0,  //0
        4,  //1
        2,  //2
        6,  //3
        16, //4
        20, //5
        18, //6
        22, //7
        8,  //8
        12, //9
        10, //10
        14, //11
        24, //12
        28, //13
        26, //14
        30, //15
        16, //16
        20, //17
        18, //18
        22, //19
        18, //20
        26, //21    WARNING (22)
        26, //22
        30, //23
        24, //24
        26, //25    WARNING (28)
        26, //26
        30, //27
        26, //28
        30, //29
        30, //30
        31  //31
    };

	// Use this for initialization
	void Start () {
        vf.Mesh = new MapVoxelMesh(new Dimensions3D(16,16,16));
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i=0; i< width; i++)
        {
            for( int j=0; j< height; j++)
            {
                VoxelLocation partLoc = new Vector3(i, j, 0);
                ComputeLife(partLoc);
                

            }
        }
        vf.Mesh.Commit();
    }

    private void ComputeLife(VoxelLocation cell)
    {


    }


    public void createParticule(int posx, int posy)
    {
        createParticule(new VoxelLocation(posx, posy, 0));
    }

    public void createParticule(VoxelLocation _position)
    {
        Live(_position);
    }

 

    private void Live(VoxelLocation cell)
    {
        vf.Mesh[cell] = new Color32(0, 0, 255, 0);
    }
    private void Kill(VoxelLocation cell)
    {
        vf.Mesh[cell] = Voxels.Empty;
    }

    private bool IsAlive(VoxelLocation locToTest)
    {
        return Voxels.HasNegative(locToTest)
              || locToTest.X > (position.x + width)
              || locToTest.Y > (position.y + height)
              || Voxels.IsNotEmpty(vf.Mesh[locToTest]);
    }
    private bool IsDead(VoxelLocation locToTest)
    {
        return !IsAlive(locToTest);
    }
}
