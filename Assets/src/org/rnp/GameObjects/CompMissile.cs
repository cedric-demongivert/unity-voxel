using UnityEngine;
using System.Collections;
using System;

public class CompMissile : Comportement {

    public int spawnDelay;
    public float range;


    private int spawnTime = 0;
    private bool targetInZone = false;
    private GameObject target;
    SphereCollider myCol;

    // Use this for initialization
    void Start () {
        myCol = GetComponent<SphereCollider>();
        myCol.radius = range;

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "enemy")
        {
            target = col.gameObject;
            targetInZone = true;
        }
        
    }

    public override bool Emit()
    {
        if (spawnTime > spawnDelay)
        {
            if (targetInZone)
            {

                if (target == null)
                {
                    targetInZone = false;
                    target = null;
                }
                else if (Vector3.Distance(transform.position, target.transform.position) < range)
                {
                    spawnTime = 0;
                    return true;
                    //emitted.Aim(target);
                    
                }

            }
        }
        else spawnTime++;

        return false;
    }

    public override void ChooseTarget(Emitted created)
    {
        created.Aim(target);
    }
}
