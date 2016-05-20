using UnityEngine;
using System.Collections;

public class Emitted : MonoBehaviour {

    protected GameObject target;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Aim(GameObject _target)
    {
        target = _target;
    }
}
