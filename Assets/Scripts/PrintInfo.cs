using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrintInfo : MonoBehaviour
{
    private Text fpsText;
    private Text gameStatus;
    private float deltaTime = 0.0f;

    public void Awake()
    {
        fpsText = GameObject.Find("OutputText").GetComponent<Text>();

        gameStatus = GameObject.Find("GameStatus").GetComponent<Text>();

        /*
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString("#FF851B", out myColor);
        gameStatus.color = myColor;
        */
        
    }

    void Start ()
    {

    }
	
	void Update ()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        float temp = (float)Math.Round((fps), 2);
        fpsText.text = "FPS: " + temp;

        gameStatus.text = "Blocks: " + BlockControl.totalBlocks;
    }

    public void PrintTest()
    {

    }
}
