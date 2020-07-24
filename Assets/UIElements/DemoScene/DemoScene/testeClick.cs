using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testeClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveMesh() {

        this.GetComponent<MeshRenderer>().enabled = !this.GetComponent<MeshRenderer>().enabled;

    }
}
