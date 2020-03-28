using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlockControl : MonoBehaviour
{
    private int target = 60;

    public GameObject selector;
    public static GameObject selectorTmp;
    public GameObject nextLevelButton;
    public GameObject resetLevelButton;

    public static GameObject nextLevelButtonTmp;
    public static GameObject resetLevelButtonTmp;

    public static int totalBlocks = 0;
    private static Block[] allBlocks;
    private static bool blocksDestroyed = false;
    private static bool blocksMovedDown = false;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;

        FindAllBlocks();
        CountBlocks();

        selectorTmp = selector;

        //selectorBlockTmp = selectorBlock;

        nextLevelButtonTmp = nextLevelButton;
        resetLevelButtonTmp = resetLevelButton;

        //defaultSelection();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!blocksMovedDown && blocksDestroyed)
        {
            blocksDestroyed = false;
            blocksMovedDown = false;
            BlockControl.MoveAllBlocksDown();
            CountBlocks();
        }
        else if(blocksMovedDown && !blocksDestroyed)
        {
            blocksDestroyed = false;
            blocksMovedDown = false;
            BlockControl.EnableBlockSelection();
            CountBlocks();
        }

        if(Application.targetFrameRate != target)
        {
            Application.targetFrameRate = target;
        }
    }

    public static void DestroyBlocks()
    {
        bool blockOnLeft = false;
        bool blockOnRight = false;
        bool blockOnBottom = false;

        Block tempBlockLeft = null;
        Block tempBlockRight = null;
        Block tempBlockBottom = null;

        foreach (Block b in GetAllBlocks())
        {
            if (b != null && !b.tag.Equals("BlackBlock"))
            {
                if (b.GetComponent<Collider2D>() != null)
                {
                    if (!b.IsBlockMoving && !b.IsBlockFalling && b.IsBlockOnBottom)
                    {
                        //Left
                        foreach (Block temp in GetAllBlocks())
                        {
                            if (temp != null)
                            {
                                if (temp.GetComponent<Collider2D>() != null)
                                {
                                    if (!temp.gameObject.Equals(b.gameObject))
                                    {
                                        if ((float)Math.Round(b.transform.position.y, 2) == (float)Math.Round(temp.transform.position.y, 2))
                                        {
                                            if ((Mathf.Abs((float)Math.Round(b.transform.position.x, 2) - (float)Math.Round(temp.transform.position.x, 2))) == 1)
                                            {
                                                //Is Block on Left
                                                if (Math.Round(temp.transform.position.x, 2) < Math.Round(b.transform.position.x, 2))
                                                {
                                                    if (temp.GetBlockTag.Equals(b.GetBlockTag))
                                                    {
                                                        blockOnLeft = true;
                                                        tempBlockLeft = temp;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        blockOnLeft = false;
                                                    }
                                                }
                                                else
                                                {
                                                    blockOnLeft = false;
                                                }
                                            }
                                            else
                                            {
                                                blockOnLeft = false;
                                            }
                                        }
                                        else
                                        {
                                            blockOnLeft = false;
                                        }
                                    }
                                }
                            }
                        }

                        //Right
                        foreach (Block temp in BlockControl.GetAllBlocks())
                        {
                            if (temp != null)
                            {
                                if (temp.GetComponent<Collider2D>() != null)
                                {
                                    if (!temp.gameObject.Equals(b.gameObject))
                                    {
                                        if ((float)Math.Round(b.transform.position.y, 2) == (float)Math.Round(temp.transform.position.y, 2))
                                        {
                                            if ((Mathf.Abs((float)Math.Round(b.transform.position.x, 2) - (float)Math.Round(temp.transform.position.x, 2))) == 1)
                                            {
                                                //Is Block on Right
                                                if (Math.Round(temp.transform.position.x, 2) > Math.Round(b.transform.position.x, 2))
                                                {
                                                    if (temp.GetBlockTag.Equals(b.GetBlockTag))
                                                    {
                                                        blockOnRight = true;
                                                        tempBlockRight = temp;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        blockOnRight = false;
                                                    }
                                                }
                                                else
                                                {
                                                    blockOnRight = false;
                                                }
                                            }
                                            else
                                            {
                                                blockOnRight = false;
                                            }
                                        }
                                        else
                                        {
                                            blockOnRight = false;
                                        }
                                    }
                                }
                            }
                        }

                        //Bottom
                        foreach (Block temp in GetAllBlocks())
                        {
                            if (temp != null)
                            {
                                if (temp.GetComponent<Collider2D>() != null)
                                {
                                    if (!temp.gameObject.Equals(b.gameObject))
                                    {
                                        if ((float)Math.Round(b.transform.position.x, 2) == (float)Math.Round(temp.transform.position.x, 2))
                                        {
                                            if (Mathf.Abs((float)Math.Round(b.transform.position.y, 2) - (float)Math.Round(temp.transform.position.y, 2)) == 1)
                                            {
                                                //Is Block on Bottom
                                                if (Math.Round(temp.transform.position.y, 2) < Math.Round(b.transform.position.y, 2))
                                                {
                                                    if (temp.GetBlockTag.Equals(b.GetBlockTag))
                                                    {
                                                        blockOnBottom = true;
                                                        tempBlockBottom = temp;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        blockOnBottom = false;
                                                    }
                                                }
                                                else
                                                {
                                                    blockOnBottom = false;
                                                }
                                            }
                                            else
                                            {
                                                blockOnBottom = false;
                                            }
                                        }
                                        else
                                        {
                                            blockOnBottom = false;
                                        }
                                    }
                                }
                            }
                        }


                        if (blockOnLeft && tempBlockLeft != null)
                        {
                            tempBlockLeft.DestroyBlock();
                        }

                        if (blockOnRight && tempBlockRight != null)
                        {
                            tempBlockRight.DestroyBlock();
                        }

                        if (blockOnBottom && tempBlockBottom != null)
                        {
                            tempBlockBottom.DestroyBlock();
                        }

                        if (blockOnLeft || blockOnRight || blockOnBottom)
                        {
                            blocksDestroyed = true;

                            Block.SetBlock = null;
                            b.DestroyBlock();
                        }

                        blockOnLeft = false;
                        blockOnRight = false;
                        blockOnBottom = false;

                        tempBlockLeft = null;
                        tempBlockRight = null;
                        tempBlockBottom = null;
                    }
                }
            }
        }
        if(blocksDestroyed)
        {
            //CountBlocks();
            blocksMovedDown = false;
        }
        else
        {
            blocksMovedDown = true;
        }
    }

    public static void MoveAllBlocksDown()
    {
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
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
        if (blocksMovedDown)
        {
            DestroyBlocks();
        }
    }

    public static void CheckAllBloks()
    {
        bool test = false;
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
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
            BlockControl.EnableBlockSelection();
        }
    }

    public static void EnableBlockSelection()
    {
        //unselect all blocks
        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
                {
                    b.SetIsSelected = false;
                }
            }
        }

        foreach (Block b in BlockControl.GetAllBlocks())
        {
            if (b != null)
            {
                if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
                {
                    if (b.GetBlockID == Block.previouslySelectedBlockID)
                    {
                        b.SetIsSelected = true;
                        Block.SetBlock = b;
                        break;
                    }
                }
            }
        }

        Block.BlockCanBeSelected = true;
        Block.BlockCanBeMoved = true;

        if(Block.GetBlock != null)
        {
            Block.resetBlockControls(Block.GetBlock);
        }
    }

    public static void EnableORDisableCollider2D(bool enable)
    {
        if (enable == false)
        {
            foreach (Block b in allBlocks)
            {
                if (b != null)
                {
                    if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
                    {
                        if (!b.IsSelected)
                        {
                            b.GetComponent<Collider2D>().enabled = false;
                        }
                    }
                }
            }
        }
        if (enable == true)
        {
            foreach (Block b in allBlocks)
            {
                if (b != null)
                {
                    if (b.GetComponent<Collider2D>() != null && !b.GetBlockTag.Equals("selector"))
                    {
                        b.GetComponent<Collider2D>().enabled = true;
                    }
                }
            }
        }
    }

    public static void FindAllBlocks()
    {
        allBlocks = FindObjectsOfType(typeof(Block)) as Block[];
    }

    public static void CountBlocks()
    {
        totalBlocks = 0;
        if(allBlocks != null)
        {
            foreach(Block b in allBlocks)
            {
                if(b != null && !b.tag.Equals("BlackBlock"))
                {
                    if(!b.tag.Equals("StaticBlock") && !b.GetBlockTag.Equals("selector"))
                    {
                        totalBlocks++;
                    }
                }
            }

            //Create button [Next Level] after all blocks removed
            if(totalBlocks == 0)
            {
                GameObject tmp = (GameObject)Instantiate(BlockControl.nextLevelButtonTmp);
            }
            else if(totalBlocks == 1)
            {
                GameObject tmp = (GameObject)Instantiate(BlockControl.resetLevelButtonTmp);
            }
        }
    }

    public static Block[] GetAllBlocks()
    {
        return allBlocks;
    }
}
