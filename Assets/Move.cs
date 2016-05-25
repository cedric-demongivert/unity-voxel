using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {


    private bool toRight = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (toRight) {
            transform.Translate(new Vector3(5, 0, 0));
            if (transform.position.x > 100)
                toRight = false;
        }
        else {
            transform.Translate(new Vector3(-5, 0, 0));
            if (transform.position.x < -100)
                toRight = true;
        }
    }
}
