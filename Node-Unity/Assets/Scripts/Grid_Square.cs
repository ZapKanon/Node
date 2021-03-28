using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Square : MonoBehaviour
{
    public Vector2 GridPosition { get; set; }

    [field:SerializeField] public Node_Block NodeBlock { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().enabled = true;

        //Rotate the block attached to this square with the right mouse button
        if (Input.GetMouseButtonDown(1) && NodeBlock != null && Battle_Manager.liftedBlock == null)
        {
            if (NodeBlock.HasEnergy == false)
            {
                NodeBlock.RotateClockwise();
            }          
        }
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    
    private void OnMouseDown()
    {
        //If this square contains a block that isn't carrying energy, clicking on the square will remove the block and cause it to follow the cursor.
        if (NodeBlock != null && Battle_Manager.liftedBlock == null && !NodeBlock.HasEnergy)
        {
            Battle_Manager.liftedBlock = NodeBlock;
            NodeBlock.Placed = false;
            NodeBlock.GetComponent<SpriteRenderer>().sortingOrder = 6;
            NodeBlock = null;

            //Doing this deselects any selected Action.
            if (Battle_Manager.selectedAction != null)
            {
                Battle_Manager.selectedAction.Deselect();
                Battle_Manager.selectedAction = null;
            }
        }
        //If this square is empty and a block is following the cursor, place the block onto this square.
        else if (NodeBlock == null && Battle_Manager.liftedBlock != null)
        {
            NodeBlock = Battle_Manager.liftedBlock;
            NodeBlock.Placed = true;
            NodeBlock.GridPosition = GridPosition;
            Battle_Manager.liftedBlock = null;
            NodeBlock.transform.position = transform.position;
            NodeBlock.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
}
