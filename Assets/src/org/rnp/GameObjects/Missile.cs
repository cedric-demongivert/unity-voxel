using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Missile : MonoBehaviour {


    private VoxelLocation posDepart;
    private float speed = 1f;
    public GameObject aim;


	// Use this for initialization
	void Start () {
        posDepart = transform.position;
        LookForEnemy();
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(new Vector3(0.1f, 0, 0));
        MoveTo(aim.transform.position);


        // IF HORS RANGE
        if (Vector3.Distance(transform.position, posDepart) > 100)
            Destroy(gameObject);
        
	}

    public void Aim(GameObject goal)
    {
        aim = goal;
    }

    private void MoveTo(Vector3 goal)
    {
        transform.LookAt(goal);
        transform.position = Vector3.MoveTowards(transform.position, goal, speed);
    }

    private void LookForEnemy()
    {
        aim = GameObject.FindGameObjectsWithTag("target")[0];
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
