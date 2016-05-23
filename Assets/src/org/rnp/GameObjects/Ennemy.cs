using UnityEngine;
using System.Collections;

public class Ennemy : Emitted {

    private int HP, HP_init;

    private float speed;
    private int damage;
    private int attack_speed;
    private int range;
    private int type;

	// Use this for initialization
	void Start () {
        HP_init = 100;
        HP = HP_init;
        speed = 0.1f;
        attack_speed = 1;
        range = 10;
	}
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(new Vector3(0.1f, 0, 0));
	
	}

    // If Ennemy takes damages
    void TakeDamage(int damage)
    {
        HP -= damage;
        Die();
    }

    // Ennemy Die
    void Die ()
    {
        if (HP <= 0) gameObject.SetActive(false);
    }
}
