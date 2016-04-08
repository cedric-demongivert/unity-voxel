using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

public class Emetteur : MonoBehaviour {

    public Water water;

    public Vector3 position;

    public float initTime = 1;
    private float timeLeft;

    

    // Use this for initialization
    void Start () {
        timeLeft = initTime;
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            water.createParticule(position);
            timeLeft = initTime;
            //Debug.Log("particule");
        }
    }


}
