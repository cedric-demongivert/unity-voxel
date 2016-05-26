using UnityEngine;
using System.Collections;

public class Emetteur : MonoBehaviour {
    
    public GameObject[] TemplateToEmit;

    public Comportement comp;



    // Use this for initialization
    void Start () {
       
    }

    // Update is called once per frame
    void Update () {
        if (TemplateToEmit.Length>0 && comp.Emit())
            Create();
	}


    private Object Create()
    {
        GameObject emittedObj = (GameObject )Instantiate(ChooseToEmit(), transform.position, transform.rotation);
        Emitted emitted = emittedObj.GetComponent<Emitted>();

        comp.ChooseTarget(emitted);

        return emittedObj;
    }

    private GameObject ChooseToEmit()
    {
        return TemplateToEmit[Random.Range(0, TemplateToEmit.Length)];
    }

    

}
