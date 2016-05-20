using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Tower : MonoBehaviour {


    #region Attributes
    private int HP_init, HP;
    private int level;
    private int cost;
    public Emetteur emet;
    GameObject go;
    #endregion

    private int att_time=0;
    private bool targetInZone=false;

    private GameObject target;

    // Use this for initialization
    void Start () {
        #region Init Character
        HP_init = 100; HP= HP_init;
        level = 1;
        cost = 1;
        #endregion


        go = this.GetComponent<GameObject>();

        
    }
	
	// Update is called once per frame
	void Update () {
        


    }


    #region Functions


    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            this.Die();
    }



    #endregion
}
