using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject[] squares; 
    private GameObject[,] squareGrid;
    public static GameObject[,] nodeGrid;


    // Start is called before the first frame update
    void Start()
    {
        squareGrid = new GameObject[20, 5];
        nodeGrid = new GameObject[20, 5];

        PopulateSquareGrid();
        PopulateNodeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        //squareGrid[0, 0].GetComponent<Grid_Square>().NodeBlock.BlockRotation = Node_Block.Rotations.Rotate270;
    }

    //Unity can't serialize 2D arrays and I don't feel like setting up custom UI to show one in the inspector so I'm converting from 1D to 2D array here.
    public void PopulateSquareGrid()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                squareGrid[i, j] = squares[(j * 20) + i];

                //Let the square know its position in the grid
                squareGrid[i, j].GetComponent<Grid_Square>().GridPosition = new Vector2(i, j);
            }
        }
    }

    public void PopulateNodeGrid()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (squareGrid[i, j].GetComponent<Grid_Square>().NodeBlock != null)
                {

                    nodeGrid[i, j] = squareGrid[i, j].GetComponent<Grid_Square>().NodeBlock.gameObject;
                    //Debug.Log(squareGrid[i, j].GetComponent<Grid_Square>().NodeBlock.gameObject);
                }
                else
                {
                    nodeGrid[i, j] = null;
                }
            }
        }
    }

    //Send energy from blocks that currently possess energy to valid adjacent blocks
    public void SendAllEnergy()
    {
        List<GameObject> blocksWithEnergy = new List<GameObject>();

        //Check each block in the nodeGrid to determine if it has energy
        foreach(GameObject block in nodeGrid)
        {
            //If a block has energy, add it to a temporary list
            if (block.GetComponent<Node_Block>().HasEnergy == true)
            {
                blocksWithEnergy.Add(block);
            }
        }

        //Attempt an energy send from each of the selected blocks
        foreach(GameObject sendingBlock in blocksWithEnergy)
        {
            AttemptSend(sendingBlock.GetComponent<Node_Block>());
        }
        
    }

    //Determine whether a valid block exists to receive energy from a block that currently possesses energy
    //The receiving block must have an EnterDirection that is opposite to the sending block's ExitDirection. (left -> right, top -> bottom, etc)
    public void AttemptSend(Node_Block sendingBlock)
    {
        Node_Block receivingBlock;

        //If the sending block has an ExitDirection of up...
        if (sendingBlock.ExitDirection == Node_Block.Directions.Up)
        {
            //Set the receiving block to the block located above the sendingBlock in the nodeGrid.
            receivingBlock = nodeGrid[(int)sendingBlock.GridPosition.x, (int)sendingBlock.GridPosition.y - 1].GetComponent<Node_Block>();
            //Check that this receivingBlock exists. (It may not if the square has been left empty or the sendingBlock has been placed on an edge.)
            if (receivingBlock != null)
            {
                //Finally, check whether the receivingBlock's EnterDirection is compatible with the sendingBlock's ExitDirection.
                if (receivingBlock.EnterDirectionA == Node_Block.Directions.Down)
                {
                    SendEnergy(sendingBlock, receivingBlock, "A");
                }
                else if (receivingBlock.EnterDirectionB == Node_Block.Directions.Down)
                {
                    SendEnergy(sendingBlock, receivingBlock, "B");
                }
                else
                {
                    Debug.Log("Invalid entrance direction. Send failed.");
                }
            }
            else
            {
                Debug.Log("Target block does not exist. Send failed.");
            }
        }
        else if (sendingBlock.ExitDirection == Node_Block.Directions.Right)
        {
            receivingBlock = nodeGrid[(int)sendingBlock.GridPosition.x + 1, (int)sendingBlock.GridPosition.y].GetComponent<Node_Block>();
            if (receivingBlock != null)
            {
                if (receivingBlock.EnterDirectionA == Node_Block.Directions.Left)
                {
                    SendEnergy(sendingBlock, receivingBlock, "A");
                }
                else if (receivingBlock.EnterDirectionB == Node_Block.Directions.Left)
                {
                    SendEnergy(sendingBlock, receivingBlock, "B");
                }
                else
                {
                    Debug.Log("Invalid entrance direction. Send failed.");
                }
            }
            else
            {
                Debug.Log("Target block does not exist. Send failed.");
            }
        }
        else if (sendingBlock.ExitDirection == Node_Block.Directions.Down)
        {
            receivingBlock = nodeGrid[(int)sendingBlock.GridPosition.x, (int)sendingBlock.GridPosition.y + 1].GetComponent<Node_Block>();
            if (receivingBlock != null)
            {
                if (receivingBlock.EnterDirectionA == Node_Block.Directions.Up)
                {
                    SendEnergy(sendingBlock, receivingBlock, "A");
                }
                else if (receivingBlock.EnterDirectionB == Node_Block.Directions.Up)
                {
                    SendEnergy(sendingBlock, receivingBlock, "B");
                }
                else
                {
                    Debug.Log("Invalid entrance direction. Send failed.");
                }
            }
            else
            {
                Debug.Log("Target block does not exist. Send failed.");
            }
        }
        else if (sendingBlock.ExitDirection == Node_Block.Directions.Left)
        {
            receivingBlock = nodeGrid[(int)sendingBlock.GridPosition.x - 1, (int)sendingBlock.GridPosition.y].GetComponent<Node_Block>();
            if (receivingBlock != null)
            {
                if (receivingBlock.EnterDirectionA == Node_Block.Directions.Right)
                {
                    SendEnergy(sendingBlock, receivingBlock, "A");
                }
                else if (receivingBlock.EnterDirectionB == Node_Block.Directions.Right)
                {
                    SendEnergy(sendingBlock, receivingBlock, "B");
                }
                else
                {
                    Debug.Log("Invalid entrance direction. Send failed.");
                }
            }
            else
            {
                Debug.Log("Target block does not exist. Send failed.");
            }
        }
    }

    //Send energy from one block to another.
    //This is only called after compatibility between the two blocks has been confirmed.
    public void SendEnergy(Node_Block sendingBlock, Node_Block receivingBlock, string entrance)
    {
        receivingBlock.PossessedEnergy = sendingBlock.PossessedEnergy;
        sendingBlock.PossessedEnergy = null;
        receivingBlock.HasEnergy = true;
        sendingBlock.HasEnergy = false;

        //Specify the entrance used by the energy for future animations and sends
        if (entrance == "A")
        {
            receivingBlock.ExitDirection = receivingBlock.EnterDirectionB;
        }
        else if (entrance == "B")
        {
            receivingBlock.ExitDirection = receivingBlock.EnterDirectionA;
        }
        else
        {
            Debug.Log("Entrance not specified. This isn't supposed to be possible...");
        }
    }

    //Remove blocks from all squares and delete those blocks.
    public void ClearAllBlocks()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //Remove reference to the block in the corresponding square and delete the block.
                Node_Block clearingBlock = squareGrid[i, j].GetComponent<Grid_Square>().NodeBlock;
                if (clearingBlock != null)
                {
                    squareGrid[i, j].GetComponent<Grid_Square>().NodeBlock = null;
                    Destroy(clearingBlock.gameObject);
                }
            }
        }
    }

    //Save data on each block in the grid.
    public void PopulateSaveData(Save_Data a_SaveData)
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log("Attempting block [" + i + ", " + j + "]");
                Debug.Log(nodeGrid[i, j]);
                if (nodeGrid[i, j] != null)
                {
                    //Pass necessary data from the block to the save data, then save it.
                    Node_Block blockScript = nodeGrid[i, j].GetComponent<Node_Block>();
                    Save_Data.BlockData blockData = new Save_Data.BlockData();
                    blockData.path = blockScript.BlockPath;
                    blockData.gridPosition = blockScript.GridPosition;
                    blockData.rotation = blockScript.BlockRotation;
                    a_SaveData.m_AllBlockData.m_BlockData[5 * i + j] = blockData;
                    Debug.Log("[" + i + ", " + j + "] Saved block to data!");
                    a_SaveData.m_Score = 4;
                    
                }
                //Set default data for empty squares.
                else
                {
                    Save_Data.BlockData blockData = new Save_Data.BlockData();
                    blockData.path = "";
                    blockData.gridPosition = new Vector2(i, j);
                    blockData.rotation = 0;
                    a_SaveData.m_AllBlockData.m_BlockData[5 * i + j] = blockData;
                }
            }
        }      
    }

    //Populate the grid with blocks from save data.
    public void LoadFromSaveData(Save_Data a_SaveData)
    {
        ClearAllBlocks();

        Save_Data.BlockData[] savedGrid = a_SaveData.m_AllBlockData.m_BlockData;
        for (int i = 0; i < 100; i++)
        {
            //Debug.Log(savedGrid[i].path);
            if (savedGrid[i].path != "")
            {
                //Obtain the block's prefab from saved filepath.
                GameObject newBlock = Resources.Load<GameObject>(savedGrid[i].path);         

                //Instantiate block at proper location.
                newBlock = Instantiate(newBlock, squareGrid[(int)Mathf.Floor(i / 5), i % 5].transform.position, transform.rotation);

                //Add the block to the nodeGrid and the corresponding square.
                nodeGrid[(int)Mathf.Floor(i / 5), i % 5] = newBlock;
                squareGrid[(int)Mathf.Floor(i / 5), i % 5].GetComponent<Grid_Square>().NodeBlock = newBlock.GetComponent<Node_Block>();

                //Set block properties.
                //newBlock.GetComponent<Node_Block>().BlockRotation = savedGrid[i].rotation;
                squareGrid[(int)Mathf.Floor(i / 5), i % 5].GetComponent<Grid_Square>().NodeBlock.BlockRotation = savedGrid[i].rotation;
                squareGrid[(int)Mathf.Floor(i / 5), i % 5].GetComponent<Grid_Square>().NodeBlock.UpdateEnterDirections();
                //newBlock.GetComponent<Node_Block>().UpdateEnterDirections();
                //squareGrid[(int)Mathf.Floor(i / 5), i % 5].GetComponent<Grid_Square>().NodeBlock.GridPosition = savedGrid[i].gridPosition; (gridPosition isn't actually used from Save_Data for now, it's just for reference when editing the file itself)
                newBlock.GetComponent<Node_Block>().GridPosition = savedGrid[i].gridPosition;
            }
            else
            {
                //Leave square empty. (This is different from being locked)
            }
        }
        Debug.Log("Setting Fire Rotation...");

    }
}
