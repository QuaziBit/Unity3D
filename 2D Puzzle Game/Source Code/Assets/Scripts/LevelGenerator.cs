using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{
	public GameObject blockControl_pfb = null;
	private GameObject blockControl_pfb_tmp = null;

	public GameObject staticBlock = null;
	
	public GameObject blockOrange = null;
	public GameObject blockBlue = null;
	public GameObject blockGreen = null;

	private List<GameObject> staticBlocks = null;
	private List<GameObject> playerBlocks = null;

	private Vector3 blockPosstMin;
	private Vector3 blockPossMax;


	public void Awake()
	{
		blockPosstMin = new Vector3();
		blockPosstMin.x = -5.5f;
		blockPosstMin.y = -3.5f;

		blockPossMax = new Vector3();
		blockPossMax.x = 5.5f;
		blockPossMax.y = 3.5f;
	}

	// Use this for initialization
	void Start () 
	{
		staticBlocks = new List<GameObject>();

		generateStaticBlocks();
		generatePlayerBlocks();

		blockControl_pfb_tmp = (GameObject)Instantiate(blockControl_pfb);
		BlockControl.CheckAllBloks();

		Block.blockCanBeSelected = true;
		Block.isDefaultSelected = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void generateStaticBlocks()
	{
		// Test
		float x = 2.5f;
		float y = 0.5f;

		for(int i = 0; i < 3; i++)
		{
			GameObject tmp = (GameObject)Instantiate(staticBlock);
			Vector3 poss = tmp.transform.position;
			
			poss.x = x + i;
			poss.y = y + i;
			
			tmp.transform.position = poss;
		}
	}

	public void generatePlayerBlocks()
	{
		// Test
		float x = 2.5f;
		float y = 1.5f;

		for(int i = 0; i < 3; i++)
		{
			GameObject tmp = (GameObject)Instantiate(blockOrange);
			Vector3 poss = tmp.transform.position;
			
			poss.x = x + i;
			poss.y = y + i;
			
			tmp.transform.position = poss;
		}
	}
}
