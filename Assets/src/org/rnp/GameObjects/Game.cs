using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public GameObject thePlayer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //If the player is dead, end the game.
        if (thePlayer == null)
        {
            throw new MissingComponentException("PLAYER IS DEAD!!!");
        }
	}
}
