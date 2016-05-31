using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


    protected Life myLife;

	// Use this for initialization
	void Start () {
        myLife = GetComponent<Life>();
        if(myLife == null)
        {
            throw new MissingComponentException("Player object should contain a Life component.");
        }
	}
	
	// Update is called once per frame
	void Update () {
	}


    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "enemy")
        {
            myLife.takeDamage(1);
        }
    }


}
