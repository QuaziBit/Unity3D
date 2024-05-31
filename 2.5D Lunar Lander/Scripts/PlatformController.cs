using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Material platformOff;
    public Material platformOn;

    public Light lightA;
    public Light lightB;
    public Light lightC;

    private bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsActivated() {
        return this.isActivated;
    }

    public void activatePlatform() {
        if (isActivated) { return; }

        lightA.enabled = false;
        lightB.enabled = false;
        lightC.enabled = true;
        
        Material[] materials = this.GetComponent<MeshRenderer>().materials;
        materials[0] = platformOn;
        this.GetComponent<MeshRenderer>().materials = materials;

        isActivated = true;

        string output1 = string.Format("platform ID: {0}", this.GetInstanceID());
        string output2 = string.Format("name: {0}", this.name);
        string output3 = string.Format("isActivated: {0}", isActivated);
        string output = string.Format("PlatformController {0} - {1} - {2}", output1, output2, output3);

        Debug.Log(output);
    }
}
