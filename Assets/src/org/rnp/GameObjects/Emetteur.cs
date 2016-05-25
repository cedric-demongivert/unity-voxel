using UnityEngine;
using System.Collections;

public class Emetteur : MonoBehaviour {
    
    public GameObject TemplateToEmit;

    public Comportement comp;



    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (comp.Emit())
            Create();
	}


    public Object Create()
    {
        GameObject emittedObj = (GameObject )Instantiate(TemplateToEmit, transform.position, transform.rotation);
        Emitted emitted = emittedObj.GetComponent<Emitted>();

        comp.ChooseTarget(emitted);

        return emittedObj;
    }

    

}
