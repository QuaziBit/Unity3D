using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExtraDockController : MonoBehaviour
{
    public Material materialOff;
    public Material materialOn;

    private bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMaterial() {
        Debug.Log("AsteroidExtraDockController updateMaterial");

        if (isActivated) { return; }

        Material[] materials = this.GetComponent<MeshRenderer>().materials;
        materials[0] = materialOn;
        this.GetComponent<MeshRenderer>().materials = materials;

        isActivated = true;
    }
}
