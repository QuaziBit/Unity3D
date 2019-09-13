// Author: Olexandr Matveyev

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtoms : MonoBehaviour
 {

	public static string levelName = "Level_1";
	private string buttonTag = "";

	public void Awake()
    {
        buttonTag = this.tag;
    }

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnMouseDown()
	{
		if (buttonTag.Equals("PLAY"))
		{
			if(Levels.currentLevel == null)
			{
				Levels.currentLevel = levelName;
				SceneLoader.load(levelName);
			}
			else
			{
				SceneLoader.load(Levels.currentLevel);
			}
		}
		if (buttonTag.Equals("LEVELS"))
		{
			SceneLoader.load("LEVELS_1");
		}
		if (buttonTag.Equals("STATS"))
		{
			SceneLoader.load("STATS");
		}
		if (buttonTag.Equals("MENU"))
		{
			SceneLoader.load("MAIN_MENU");
		}
		if (buttonTag.Equals("INFO"))
		{
			SceneLoader.load("TUTORIAL");
		}
		if (buttonTag.Equals("EXIT"))
		{
			Debug.Log("EXIT");
			Application.Quit();
		}

		if (buttonTag.Equals("NEXT"))
		{
			//Load next level
            string tmpLevelName = Levels.currentLevel;
            if(tmpLevelName != null)
            {
                string[] tokens = tmpLevelName.Split('_');
                int l = Int32.Parse(tokens[1]);

                Debug.Log("Last level was: " + l);
                l++;

				//==================================//
				if(l == 17)
				{
					l = 1;
				}
				//==================================//

                string nextLevel = "Level_" + l;
                Debug.Log("Next level is: " + l);

                Levels.currentLevel = nextLevel;
                SceneLoader.load(nextLevel);
            }
		}

		if (buttonTag.Equals("RESET"))
		{
			//Reload level
			SceneLoader.load(Levels.currentLevel);
		}


	}
}
