using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour 
{
	public static string currentLevelPage = "LEVEL_1";
	public static string currentLevel = null;
	private string buttonName = "";

	public void Awake()
    {
        buttonName = this.name;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnMouseDown()
	{
		if (buttonName.Equals("Level_1"))
		{
			currentLevel = "Level_1";
			SceneLoader.load("Level_1");
		}
		if (buttonName.Equals("Level_2"))
		{
			currentLevel = "Level_2";
			SceneLoader.load("Level_2");
		}
		if (buttonName.Equals("Level_3"))
		{
			currentLevel = "Level_3";
			SceneLoader.load("Level_3");
		}
		if (buttonName.Equals("Level_4"))
		{
			currentLevel = "Level_4";
			SceneLoader.load("Level_4");
		}
		if (buttonName.Equals("Level_5"))
		{
			currentLevel = "Level_5";
			SceneLoader.load("Level_5");
		}
		if (buttonName.Equals("Level_6"))
		{
			currentLevel = "Level_6";
			SceneLoader.load("Level_6");
		}
		if (buttonName.Equals("Level_7"))
		{
			currentLevel = "Level_7";
			SceneLoader.load("Level_7");
		}
		if (buttonName.Equals("Level_8"))
		{
			currentLevel = "Level_8";
			SceneLoader.load("Level_8");
		}
		if (buttonName.Equals("Level_9"))
		{
			currentLevel = "Level_9";
			SceneLoader.load("Level_9");
		}
		if (buttonName.Equals("Level_10"))
		{
			currentLevel = "Level_10";
			SceneLoader.load("Level_10");
		}
		if (buttonName.Equals("Level_11"))
		{
			currentLevel = "Level_11";
			SceneLoader.load("Level_11");
		}
		if (buttonName.Equals("Level_12"))
		{
			currentLevel = "Level_12";
			SceneLoader.load("Level_12");
		}
		if (buttonName.Equals("Level_13"))
		{
			currentLevel = "Level_13";
			SceneLoader.load("Level_13");
		}
		if (buttonName.Equals("Level_14"))
		{
			currentLevel = "Level_14";
			SceneLoader.load("Level_14");
		}
		if (buttonName.Equals("Level_15"))
		{
			currentLevel = "Level_15";
			SceneLoader.load("Level_15");
		}
		if (buttonName.Equals("Level_16"))
		{
			currentLevel = "Level_16";
			SceneLoader.load("Level_16");
		}
	}
}
