using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    private float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
         float fps = 1.0f / deltaTime;
         // this.gameObject.text = Mathf.Ceil (fps).ToString ();
         this.gameObject.GetComponent<Text>().text = Mathf.Ceil (fps).ToString();
    }
}
