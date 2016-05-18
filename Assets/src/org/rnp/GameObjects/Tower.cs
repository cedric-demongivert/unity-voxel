using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Tower : MonoBehaviour {


    #region Attributes
    private int HP_init, HP;
    private int level;
    private int attack_speed;
    private int cost;
    public Emetteur emet;
    private int range;
    GameObject go;
    #endregion

    private int att_time=0;


    public GameObject target;

    // Use this for initialization
    void Start () {
        #region Init Character
        HP_init = 100; HP= HP_init;
        level = 1;
        attack_speed = 100;
        cost = 1;
        range = 10;
        #endregion

        go = this.GetComponent<GameObject>();

        
    }
	
	// Update is called once per frame
	void Update () {
        att_time++;
        if(att_time >= attack_speed)
        {
            this.LaunchMissile(new Vector3(5, 5, 5));
            att_time = 0;
        }

	}


    #region Functions

    private void LaunchMissile(VoxelLocation aim)
    {
        emet.Create();
    }

    private void Die()
    {
        go.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            this.Die();
    }



    #endregion
}
