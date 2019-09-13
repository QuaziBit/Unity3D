// Author: Olexandr Matveyev

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveButton : MonoBehaviour
{
    private int currentLevelPage = 1;

    //Button Tag
    private string buttonTag = "";

    private float moveDirection = 0.0f;

    private MoveButton moveButton = null;

    public void Awake()
    {
        buttonTag = this.tag;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnMouseDown()
    {
        buttonTag = this.tag;

        if (buttonTag.Equals("PREVIOUS_LEVEL"))
        {
            Debug.Log("PREVIOUS_LEVEL");
            listLevels("PREVIOUS_LEVEL");
        }
        else if (buttonTag.Equals("NEXT_LEVEL"))
        {
            Debug.Log("NEXT_LEVEL");
            listLevels("NEXT_LEVEL");
        }


        if (buttonTag.Equals("MoveLeft") || buttonTag.Equals("LEFT"))
        {
            moveDirection = -1.0f;
            if(Block.left_selector != null)
            {
                Block.DestroySelector(Block.left_selector);
            }
            if(Block.right_selector != null)
            {
                Block.DestroySelector(Block.right_selector);
            }

            blockControll();
        }
        else if (buttonTag.Equals("MoveRight") || buttonTag.Equals("RIGHT"))
        {
            moveDirection = 1.0f;
            if(Block.left_selector != null)
            {
                Block.DestroySelector(Block.left_selector);
            }
            if(Block.right_selector != null)
            {
                Block.DestroySelector(Block.right_selector);
            }

            blockControll();
        }
    }

    public void listLevels(string arg)
    {
        if(arg.Equals("NEXT_LEVEL"))
        {
            string[] tokens = Levels.currentLevelPage.Split('_');
            int lp = Int32.Parse(tokens[1]);

            if(lp < 2)
            {
                lp++;
            }

            string levelPageName = "LEVELS_" + lp;
            Debug.Log("Current level page: " + levelPageName);
            Levels.currentLevelPage = levelPageName;

            SceneLoader.load(levelPageName);
        }
        else if(arg.Equals("PREVIOUS_LEVEL"))
        {
            string[] tokens = Levels.currentLevelPage.Split('_');
            int lp = Int32.Parse(tokens[1]);

            if(lp >= 2)
            {
                lp--;
            }

            string levelPageName = "LEVELS_" + lp;
            Debug.Log("Current level page: " + levelPageName);
            Levels.currentLevelPage = levelPageName;
            
            SceneLoader.load(levelPageName);
        }
    }

    public void blockControll()
    {
        if (Block.blockCanBeMoved)
        {
            if (Block.GetBlock != null)
            {
                Block.GetBlock.SetMoveDirection = moveDirection;
                //Block.GetBlock.removeSelectorBlock();

                //Block.GetBlock.CheckIsBlockOnLeft();
                //Block.GetBlock.CheckIsBlockOnRight();
                //Block.GetBlock.CheckIsBlockOnBottom();

            }
        }
    }

    public float GetMoveDirection
    {
        get { return moveDirection; }
    }
}
