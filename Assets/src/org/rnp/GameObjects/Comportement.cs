using UnityEngine;
using System.Collections;

public abstract class Comportement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract bool Emit();
    public abstract void ChooseTarget(Emitted created);
}
