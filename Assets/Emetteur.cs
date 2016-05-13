using UnityEngine;
using System.Collections;
using org.rnp.voxel.mesh;
using org.rnp.voxel.utils;

public class Emetteur : MonoBehaviour {

    public Water water;

    public Vector3 position;

    public float tickTime = 1;
    private float timeLeft;

    

    // Use this for initialization
    void Start () {
        timeLeft = 0;
    }
	
	// Update is called once per frame
	void Update () {
    timeLeft += Time.deltaTime;

    while(timeLeft >= tickTime)
    {
      water.createParticule(position);
      timeLeft -= tickTime;
    }
  }


}
