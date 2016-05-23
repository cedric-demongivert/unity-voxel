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


        col= GetComponent<SphereCollider>();
        if(onCollision)
            col.radius = range; 
        else
            col.enabled = false;
        


    }

    // Update is called once per frame
    void Update () {
        if (spawnTime > spawnDelay)
        {
            if (onCollision && targetInZone)
            {

                if(target == null)
                {
                    targetInZone = false;
                    target = null;
                }
                else if(Vector3.Distance(transform.position, target.transform.position) < range)
                {
                    GameObject emittedObj = (GameObject)Create();
                    Emitted emitted = emittedObj.GetComponent<Emitted>();
                    emitted.Aim(target);
                    spawnTime = 0;
                }

            }
        }
        else spawnTime++;
	}

    void OnCollisionEnter(Collision col)
    {
        target = col.gameObject;
        if (target == null) Debug.Log(System.DateTime.Now + "   NULLLLLLLLLLLLL");
        targetInZone = true;
        Debug.Log(System.DateTime.Now + "   Collision");
    }

    public Object Create()
    {
        return Instantiate(TemplateToEmit, transform.position, transform.rotation);
    }

    

}
