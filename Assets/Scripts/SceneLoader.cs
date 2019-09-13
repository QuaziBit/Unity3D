// Author: Olexandr Matveyev

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public static void load(string level)
	{
		Block.blockCanBeSelected = true;
		Block.isDefaultSelected = false;
		UnityEngine.SceneManagement.SceneManager.LoadScene(level);
	}
}
