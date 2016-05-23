using UnityEngine;
using System.Collections;

public class Emetteur : MonoBehaviour {
    
    public GameObject TemplateToEmit;
    public int spawnDelay;
    public bool onCollision;
    public float range;


    private int spawnTime = 5;
    private bool targetInZone = false;
    private GameObject target;
    SphereCollider col;


    // Use this for initialization
    void Start () {
        InvokeRepeating("Create", spawnDelay, spawnTime);
        //col= GetComponent<SphereCollider>();
        //col.radius = range;
    }

    // Update is called once per frame
    void Update () {
        if (spawnTime > spawnDelay)
        {
            if (onCollision && targetInZone)
            {
                if (target == null || Vector3.Distance(transform.position, target.transform.position) > range)
                {
                    targetInZone = false;
                    target = null;
                    Debug.Log("Range =" + range);
                    Debug.Log("Distance:" + Vector3.Distance(transform.position, target.transform.position));
                    Debug.LogError("No target");
                }
                else
                {
                    Debug.Log("FIRE");
                    Debug.Log("Range =" + range);
                    Debug.LogError("Distance:" + Vector3.Distance(transform.position, target.transform.position));

                    GameObject emittedObj = (GameObject)Create();
                    Emitted emitted = emittedObj.GetComponent<Emitted>();
                    emitted.Aim(target);
                    spawnTime = 0;

                }
                    

            }
        }
        else spawnTime++;
	}

    public Object Create()
    {
        return Instantiate(TemplateToEmit, transform.position, transform.rotation);
    }

    void OnCollisionEnter(Collision col)
    {
        target = col.gameObject;
        targetInZone = true;
        Debug.LogError("Collision");
    }

}
