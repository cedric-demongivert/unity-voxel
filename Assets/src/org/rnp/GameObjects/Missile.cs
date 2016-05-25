using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Missile : Emitted {


    private VoxelLocation posDepart;
    private float speed = 1f;


	// Use this for initialization
	void Start () {
        posDepart = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (target == null)
            Kill();
        
        MoveTo(target.transform.position);


        // IF HORS RANGE
        if (Vector3.Distance(transform.position, posDepart) > 100)
            Kill();
    }

    private void MoveTo(Vector3 goal)
    {
        transform.LookAt(goal);
        transform.position = Vector3.MoveTowards(transform.position, goal, speed);
    }



    void OnCollisionEnter(Collision col)
    {
        Kill();
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
