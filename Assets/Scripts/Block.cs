// Author: Olexandr Matveyev


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Block : MonoBehaviour
{
    public static GameObject left_selector = null;
    public static GameObject right_selector = null;

    private static float block_x = 0.0f;

    private static GameObject selectorBlock = null;

    //Get this Block
    private static Block block = null;

    //Can a Block be Selected
    public static bool blockCanBeSelected = true;

    //Can a Block be Moved
    public static bool blockCanBeMoved = true;

    //Save Previously Selected BlockID in order to select this block again
    public static int previouslySelectedBlockID = -1;

    //ID Counter
    private static int blocksCounter = 0;

    //Block ID
    private int blockID = 0;

    //Block Tag
    private string blockTag = "";

    //Identify whether block is selected
    private bool isSelected = false;

    //Move Direction
    private float moveDirection = 0.0f;

    //Block Moved Horizontally
    private float blockMoved_X = 0.0f;

    //Is Block Moving
    private bool isBlockMoving = false;
    private bool isBlockFalling = false;

    //Are Blocks on Sides
    private bool isBlockOnLeft = false;
    private bool isBlockOnRight = false;
    private bool isBlockOnBottom = false;

    private bool isBlockOnTop = false;
    private Block topBlock = null;


    private bool blockOnLeft = false;
    private Block tempBlockLeft = null;

    private bool blockOnRight = false;
    private Block tempBlockRight = null;

    private bool blockOnBottom = false;
    private Block tempBlockBottom = null;

    public static bool isDefaultSelected = true;

    public void Awake()
    {
        //ID
        this.blockID = blocksCounter;
        blocksCounter++;

        //Tag
        this.blockTag = this.tag;

        
    }

    void Start()
    {
        if(!isDefaultSelected)
        {
            // default Selection
            if(!this.tag.Equals("StaticBlock") && !this.tag.Equals("selector"))
            {
                isDefaultSelected = true;
                this.OnMouseDown();
            }
        }
    }

    void Update()
    {
        if (GetComponent<Collider2D>() != null)
        {
            if (GetComponent<Collider2D>().enabled)
            {
                if (!isBlockOnLeft && !isBlockFalling && isSelected)
                {
                    if (moveDirection == -1)
                    {
                        CheckIsBlockOnLeft();
                        MoveBlock();
                    }
                }
                if (!isBlockOnRight && !isBlockFalling && isSelected)
                {
                    if (moveDirection == 1)
                    {
                        CheckIsBlockOnRight();
                        MoveBlock();
                    }
                }
            }
            if (!isBlockOnBottom && !isBlockMoving)
            {
                CheckIsBlockOnBottom();
                MoveBlockDown();
            }
        }
    }

    //Block Selection
    public void OnMouseDown()
    {
        block = null;

        if (blockCanBeSelected)
        {
            if(left_selector != null)
            {
                DestroySelector(left_selector);
                left_selector = null;
            }
            if(right_selector != null)
            {
                DestroySelector(right_selector);
                right_selector = null;
            }

            UnselectAllBlocks();

            CheckIsBlockOnTop();

            isSelected = true;
            previouslySelectedBlockID = blockID;
            CheckIsBlockOnLeft();
            CheckIsBlockOnRight();
            CheckIsBlockOnBottom();

            block = this;

            //createSelectorBlock();

            //Reset selected block X possition
            block_x = 0.0f;
            block_x = this.transform.position.x;
            createBlockControls();
        }
    }

    public static void resetBlockControls(Block tmp)
    {
        //tmp.createBlockControls(tmp);
        tmp.OnMouseDown();
    }

    public void createBlockControls()
    {
        if(!isBlockOnLeft)
        {
            AddBlockControl(-1.0f, 0.0f);
        }

        if(!isBlockOnRight)
        {
            AddBlockControl(0.0f, 1.0f);
        }
        
    }

    public void AddBlockControl(float left, float right)
    {
        //Add selector to selected block
        if(BlockControl.selectorTmp != null)
        {
            if(left == -1.0f)
            {
                left_selector = (GameObject)Instantiate(BlockControl.selectorTmp);

                // Flip by X left selector
                SpriteRenderer sr = left_selector.GetComponentInParent<SpriteRenderer>();
                sr.flipX = true;

                //Set X and Y for mover
                Vector3 current_poss = left_selector.transform.position;
                current_poss.x = (this.transform.position.x + left);
                current_poss.y = this.transform.position.y;
                left_selector.transform.position = current_poss;

                left_selector.tag = "LEFT";
            }
            if(right == 1.0f)
            {
                right_selector = (GameObject)Instantiate(BlockControl.selectorTmp);

                //Set X and Y for mover
                Vector3 current_poss = right_selector.transform.position;
                current_poss.x = (this.transform.position.x + right);
                current_poss.y = this.transform.position.y;
                right_selector.transform.position = current_poss;

                right_selector.tag = "RIGHT";
            }
        }
    }

    public string leftOrRightSelector()
    {
        //Identify what selector has been clicked

        string side = "";
        float selector_x = this.transform.position.x;

        if (Math.Round(selector_x, 2) < Math.Round(block_x, 2))
        {
            side = "LEFT";
        }
        else if (Math.Round(selector_x, 2) > Math.Round(block_x, 2))
        {
            side = "RIGHT";
        }

        return side;
    }


    public void UnselectAllBlocks()
    {
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
                {
                    b.topBlock = null;
                    b.SetIsSelected = false;
                    b.SetMoveDirection = 0;
                }
            }
        }
    }

    //================================ Block Movement ================================//
    //Move Block Horizontally
    public void MoveBlock()
    {
        if (GetComponent<Collider2D>() != null && !this.GetBlockTag.Equals("selector"))
        {
            if (GetComponent<Collider2D>().enabled)
            {
                if (isSelected)
                {
                    if (blockMoved_X <= 1.0f)
                    {
                        //Move Block
                        Vector3 currentPosition = gameObject.transform.position;
                        currentPosition.x += (float)Math.Round((0.1f * moveDirection), 2);
                        gameObject.transform.position = currentPosition;

                        isBlockMoving = true;
                        blockCanBeSelected = false;
                        blockCanBeMoved = false;
                        blockMoved_X += 0.1f;
                    }
                    else
                    {
                        isBlockMoving = false;
                        blockMoved_X = 0.0f;
                        moveDirection = 0;

                        HorizontalMovementsStopped();
                    }
                }
            }
        }
    }

    //Move Block Vertically
    public void MoveBlockDown()
    {
        if (GetComponent<Collider2D>() != null && !this.GetBlockTag.Equals("selector"))
        {
            if (!isBlockOnBottom)
            {
                //Move Block
                Vector3 currentPosition = gameObject.transform.position;
                currentPosition.y += (float)Math.Round((-0.1f), 2);
                gameObject.transform.position = currentPosition;

                isBlockFalling = true;
                blockCanBeSelected = false;
                blockCanBeMoved = false;
            }
            else
            {
                isBlockFalling = false;

                VerticalMovementsStopped();   
            }
        }
    }
    //===============================================================================//

    public void HorizontalMovementsStopped()
    {
        CheckIsBlockOnBottom();
        if (isBlockOnBottom)
        {
            VerticalMovementsStopped();
        }
    }

    public void VerticalMovementsStopped()
    {
        BlockControl.MoveAllBlocksDown();
    }

    //============================= Are Blocks on Sides ==============================//
    public void CheckIsBlockOnLeft()
    {
        foreach (Block temp in BlockControl.GetAllBlocks())
        {
            if (temp != null)
            {
                if (!temp.gameObject.Equals(this.gameObject))
                {
                    if ((float)Math.Round(this.transform.position.y, 2) == (float)Math.Round(temp.transform.position.y, 2))
                    {
                        if ((Mathf.Abs((float)Math.Round(this.transform.position.x, 2) - (float)Math.Round(temp.transform.position.x, 2))) == 1)
                        {
                            //Is Block on Left
                            if (Math.Round(temp.transform.position.x, 2) < Math.Round(this.transform.position.x, 2))
                            {
                                this.isBlockOnLeft = true;
                                break;
                            }
                            else
                            {
                                this.isBlockOnLeft = false;
                            }
                        }
                        else
                        {
                            this.isBlockOnLeft = false;
                        }
                    }
                    else
                    {
                        this.isBlockOnLeft = false;
                    }
                }
            }
        }
    }

    public void CheckIsBlockOnRight()
    {
        foreach (Block temp in BlockControl.GetAllBlocks())
        {
            if (temp != null)
            {
                if (!temp.gameObject.Equals(this.gameObject))
                {
                    if ((float)Math.Round(this.transform.position.y, 2) == (float)Math.Round(temp.transform.position.y, 2))
                    {
                        if ((Mathf.Abs((float)Math.Round(this.transform.position.x, 2) - (float)Math.Round(temp.transform.position.x, 2))) == 1)
                        {
                            //Is Block on Right
                            if (Math.Round(temp.transform.position.x, 2) > Math.Round(this.transform.position.x, 2))
                            {
                                this.isBlockOnRight = true;
                                break;
                            }
                            else
                            {
                                this.isBlockOnRight = false;
                            }
                        }
                        else
                        {
                            this.isBlockOnRight = false;
                        }
                    }
                    else
                    {
                        this.isBlockOnRight = false;
                    }
                }
            }
        }
    }

    public void CheckIsBlockOnBottom()
    {
        foreach (Block temp in BlockControl.GetAllBlocks())
        {
            if (temp != null)
            {
                if (!temp.gameObject.Equals(this.gameObject))
                {
                    if ((float)Math.Round(this.transform.position.x, 2) == (float)Math.Round(temp.transform.position.x, 2))
                    {
                        if (Mathf.Abs((float)Math.Round(this.transform.position.y, 2) - (float)Math.Round(temp.transform.position.y, 2)) == 1)
                        {
                            //Is Block on Bottom
                            if (Math.Round(temp.transform.position.y, 2) < Math.Round(this.transform.position.y, 2))
                            {
                                this.isBlockOnBottom = true;
                                break;
                            }
                            else
                            {
                                this.isBlockOnBottom = false;
                            }
                        }
                        else
                        {
                            this.isBlockOnBottom = false;
                        }
                    }
                    else
                    {
                        this.isBlockOnBottom = false;
                    }
                }
            }
        }
    }

    public void CheckIsBlockOnTop()
    {
        foreach (Block temp in BlockControl.GetAllBlocks())
        {
            if (temp != null)
            {
                if (!temp.gameObject.Equals(this.gameObject))
                {
                    if ((float)Math.Round(this.transform.position.x, 2) == (float)Math.Round(temp.transform.position.x, 2))
                    {
                        if (Mathf.Abs((float)Math.Round(this.transform.position.y, 2) - (float)Math.Round(temp.transform.position.y, 2)) == 1)
                        {
                            //Is Block on Bottom
                            if (Math.Round(temp.transform.position.y, 2) > Math.Round(this.transform.position.y, 2))
                            {
                                this.isBlockOnTop = true;
                                this.topBlock = temp;
                                break;
                            }
                            else
                            {
                                this.isBlockOnTop = false;
                                this.topBlock = null;
                            }
                        }
                        else
                        {
                            this.isBlockOnTop = false;
                            this.topBlock = null;
                        }
                    }
                    else
                    {
                        this.isBlockOnTop = false;
                        this.topBlock = null;
                    }
                }
            }
        }
    }
    //===============================================================================//

    public void MoveAllBlocksDown()
    {
        bool blocksMovedDown = false;
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null)
                {
                    b.CheckIsBlockOnBottom();
                    if (!b.IsBlockOnBottom)
                    {
                        blocksMovedDown = false;
                        break;
                    }
                    else
                    {
                        blocksMovedDown = true;
                    }
                }
            }
        }

        // Used just to take out annoying Visual Code massage
        if(blocksMovedDown)
        {
            blocksMovedDown = true;
        }
    }

    public void CheckAllBloks()
    {
        bool test = false;
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null)
                {
                    if (b.IsBlockMoving || b.IsBlockFalling)
                    {
                        test = false;
                        break;
                    }
                    else
                    {
                        test = true;
                    }
                }
            }
        }
        if (test)
        {
            EnableBlockSelection();
        }
    }

    public void DestroyBlock()
    {
        if(!this.tag.Equals("BlackBlock"))
        {
            Destroy(this.gameObject, 0.05f);
            Destroy(this, 0.05f);
        }
    }

    public static void DestroySelector(GameObject b)
    {
        if(!b.tag.Equals("BlackBlock"))
        {
            Destroy(b, 0.05f);
            Destroy(b, 0.05f);
        }
    }
    //===============================================================================//

    public void EnableBlockSelection() 
    {
        //unselect all blocks
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null)
                {
                    b.SetIsSelected = false;
                }
            }
        }

        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null)
                {
                    if (b.GetBlockID == Block.previouslySelectedBlockID)
                    {
                        b.SetIsSelected = true;
                        block = this;
                        break;
                    }
                }
            }
        }
    }

    public int GetBlockID
    {
        get { return blockID; }
    }

    public string GetBlockTag
    {
        get { return blockTag; }
    }

    public static Block SetBlock
    {
        set { block = value; }
    }

    public static Block GetBlock
    {
        get { return block; }
    }

    public bool IsBlockOnLeft
    {
        get { return isBlockOnLeft; }
    }

    public bool IsBlockOnRight
    {
        get { return isBlockOnRight; }
    }

    public bool IsBlockOnBottom
    {
        get { return isBlockOnBottom; }
    }

    public bool IsBlockOnTop
    {
        get { return isBlockOnTop; }
    }

    public Block GetTopBlock
    {
        get { return topBlock; }
    }

    public bool SetIsSelected
    {
        set { isSelected = value; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
    }

    public bool IsBlockMoving
    {
        get { return isBlockMoving; }
    }

    public bool IsBlockFalling
    {
        get { return isBlockFalling; }
    }

    public static bool BlockCanBeSelected
    {
        set { blockCanBeSelected = value; }
        get { return blockCanBeSelected; }
    }

    public static bool BlockCanBeMoved
    {
        set { blockCanBeMoved = value; }
        get { return blockCanBeMoved; }
    }

    public float SetMoveDirection
    {
        set { moveDirection = value; }
    }

    public GameObject GetLeftSelector
    {
        get {return left_selector; }
    }

    public GameObject GetRightSelector
    {
        get {return right_selector; }
    }

}
