using UnityEngine;
using System.Collections;
using org.rnp.voxel.utils;

public class Tower : MonoBehaviour {


    #region Attributes
    public Emetteur emetteur;
    GameObject go;
    #endregion

    private int att_time=0;
    private bool targetInZone=false;

    private GameObject target;

    // Use this for initialization
    void Start () {
        #region Init
        go = this.GetComponent<GameObject>();
        #endregion
    }
	
	// Update is called once per frame
	void Update () {
    }


    #region Functions


    private void Die()
    {
        Destroy(gameObject);
    }


    #endregion
}
