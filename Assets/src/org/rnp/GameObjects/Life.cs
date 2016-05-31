using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

    public int HP_init;
    public int HP { get; protected set; }

	// Use this for initialization
	void Start () {
        if (HP_init > 0)
            HP = HP_init;
        else
            HP = 100;
	}
	
	// Update is called once per frame
	void Update () {
        if (HP <= 0)
            Kill();    
	}

    



    // The object's HP decrease by damage points
    //  Return the HP left.
    public int takeDamage(int damage)
    {
        if (damage > 0)
            HP -= damage;
        return HP;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }



}
