using UnityEngine;
using System.Collections;
using System;

public class CompEnemy : Comportement {


    public GameObject Path;

    public int spawnDelay;
    private int spawnTime = 0;



    public override void ChooseTarget(Emitted created)
    {
        MoveEnnemy me = created.GetComponent<MoveEnnemy>();
        me.Init(Path);
    }

    public override bool Emit()
    {
        if (spawnTime > spawnDelay)
        {
            spawnTime = 0;
            return true;
        }
        else spawnTime++;

        return false;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
