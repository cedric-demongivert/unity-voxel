using UnityEngine;
using System.Collections;

public class Emetteur : MonoBehaviour {
    
    public GameObject missileTemplate;

    private float spawnDelay=5f, spawnTime=5f;

    // Use this for initialization
    void Start () {
        //InvokeRepeating("Create", spawnDelay, spawnTime);

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Create()
    {
        Instantiate(missileTemplate, transform.position, transform.rotation);
    }



}
