using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Missile : Emitted {


    private VoxelLocation posDepart;
    private float speed = 1f;


	// Use this for initialization
	void Start () {
        posDepart = transform.position;
        LookForEnemy();
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

    private void LookForEnemy()
    {/*
        GameObject[] targets = GameObject.FindGameObjectsWithTag("target");
        GameObject target = targets[0];
        if(target == null)  Kill();

        float minDist = Vector3.Distance(transform.position, target.transform.position);
        for(int i=1; i<targets.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, targets[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                target = targets[i];
            }
        }*/

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
