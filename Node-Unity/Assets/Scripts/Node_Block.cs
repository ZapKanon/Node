using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node_Block : MonoBehaviour
{
    public enum Directions
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum Rotations
    { 
        Rotate0,
        Rotate90,
        Rotate180,
        Rotate270
    }

    //Sprites to be swapped when rotating the block.
    //Several blocks involve symbols or text that should not rotate with the rest of the block, so multiple sprites are needed.
    [SerializeField] public Sprite sprite0;
    [SerializeField] protected Sprite sprite90;
    [SerializeField] protected Sprite sprite180;
    [SerializeField] protected Sprite sprite270;

    [SerializeField] protected SpriteRenderer spriteRenderer;

    //Is this block a source node?
    [SerializeField] public bool IsSource { get; set; }
    //Is this block a receiver node?
    [SerializeField] public bool IsReceiver { get; set; }

    [SerializeField] protected float EnergyPossessionTimer { get; set; }

    protected float prevPossessionTimer;

    //Most blocks have two sides from which energy can enter.
    [field: SerializeField] public Directions EnterDirectionA { get; set; }
    [field: SerializeField] public Directions EnterDirectionB {get; set; }

    //the filepath to this block's prefab. Used for instantiation.
    public string BlockPath { get; set; }

    //The exit direction energy will take when leaving the node. This is the opposite entrance to the one the energy used to enter. 
    [field:SerializeField] public Directions ExitDirection { get; set; }

    [field: SerializeField] public Rotations BlockRotation { get; set; }
    [field: SerializeField] public bool HasEnergy { get; set; }
    protected bool HadEnergy { get; set; }
    [field: SerializeField] public Vector2 GridPosition { get; set; }
    public Energy PossessedEnergy { get; set; }

    public bool Placed { get; set; }

    [SerializeField] private bool testRotation;

    public Tooltip_Manager.ToolTips toolTip; //Correlates to ToolTip enum in ToolTip_Manager.
    public Color32 tooltipColor;

    protected virtual void Start()
    {

    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //BlockRotation = Rotations.Rotate0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite0;
        HasEnergy = false;
        HadEnergy = false;
        EnergyPossessionTimer = 0f;
        Placed = true;
        IsSource = false;
        IsReceiver = false;
        tooltipColor = Color.black;

        testRotation = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateEnergy();

        //Test to simulate taking block rotation
        if (testRotation == true)
        {
            RotateClockwise();
            testRotation = false;
        }

        //If the block is not currently placed in the grid, follow the cursor
        if (Placed == false)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);
        }
    }

    //Keep track of how long energy has been in this block's system.
    //Animate said energy accordingly, and attempt to send it to another block upon reaching an interval specified in Battle_Manager
    public void UpdateEnergy()
    {
        //Only update energy if there is energy to update.
        if (HasEnergy == true)
        {
            EnergyPossessionTimer += Time.deltaTime;
            //If this block just obtained energy, position the energy sprite based on this block's Enter and ExitDirection
            if (HadEnergy == false)
            {
                //Call ModifyEnergy on the first frame when energy enters this block.
                ModifyEnergy();
                PossessedEnergy.GetComponent<SpriteRenderer>().enabled = true;
            }
            //Move energy through first half of block.
            else if (EnergyPossessionTimer < PossessedEnergy.travelSpeed / 2f)
            {
                if (EnterDirectionA != ExitDirection)
                {
                    MoveEnergyFirstHalf(EnterDirectionA);
                }
                else if (EnterDirectionB != ExitDirection)
                {
                    MoveEnergyFirstHalf(EnterDirectionB);
                }
            }
            //If the energy has just completed half of its movmeent through this block, update its initial position to the center of this block.
            else if (prevPossessionTimer < PossessedEnergy.travelSpeed / 2f && EnergyPossessionTimer >= PossessedEnergy.travelSpeed)
            {
                PossessedEnergy.transform.position = transform.position;
            }
            //Move energy through second half of block.
            else if (EnergyPossessionTimer >= PossessedEnergy.travelSpeed / 2f && EnergyPossessionTimer < PossessedEnergy.travelSpeed)
            {
                MoveEnergySecondHalf();
            }
            //If the energy has completed its movement, send it to the next block.
            else if (EnergyPossessionTimer >= PossessedEnergy.travelSpeed)
            {               
                FindSendTarget();
                EnergyPossessionTimer = 0f;
            }
        }
        else
        {
            EnergyPossessionTimer = 0f;
        }
        

        HadEnergy = HasEnergy;
        prevPossessionTimer = EnergyPossessionTimer;
    }

    //The first half of energy movement has a direction based on the block's used EnterDirection.
    public void MoveEnergyFirstHalf(Directions enterDirection)
    {
        float energyProgressPercent = EnergyPossessionTimer / (PossessedEnergy.travelSpeed / 2f);

        switch (enterDirection)
        {
            case Directions.Up:
                PossessedEnergy.transform.position = new Vector3(transform.position.x, transform.position.y + 0.12f - 0.12f * energyProgressPercent, 0f);
                PossessedEnergy.transform.rotation = Quaternion.identity;
                break;

            case Directions.Right:
                PossessedEnergy.transform.position = new Vector3(transform.position.x + 0.12f - 0.12f * energyProgressPercent, transform.position.y, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;

            case Directions.Down:
                PossessedEnergy.transform.position = new Vector3(transform.position.x, transform.position.y - 0.12f + 0.12f * energyProgressPercent, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case Directions.Left:
                PossessedEnergy.transform.position = new Vector3(transform.position.x - 0.12f + 0.12f * energyProgressPercent, transform.position.y, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }

    //The second half of energy movement has a direction based on the block's ExitDirection.
    public void MoveEnergySecondHalf()
    {
        float energyProgressPercent = (EnergyPossessionTimer - PossessedEnergy.travelSpeed / 2) / (PossessedEnergy.travelSpeed / 2);

        switch (ExitDirection)
        {
            case Directions.Up:
                PossessedEnergy.transform.position = new Vector3(transform.position.x, transform.position.y + 0.12f * energyProgressPercent, 0f);
                PossessedEnergy.transform.rotation = Quaternion.identity;
                break;

            case Directions.Right:
                PossessedEnergy.transform.position = new Vector3(transform.position.x + 0.12f * energyProgressPercent, transform.position.y, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;

            case Directions.Down:
                PossessedEnergy.transform.position = new Vector3(transform.position.x, transform.position.y - 0.12f * energyProgressPercent, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case Directions.Left:
                PossessedEnergy.transform.position = new Vector3(transform.position.x - 0.12f * energyProgressPercent, transform.position.y, 0f);
                PossessedEnergy.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }

    //Change the block's enterDirections A and B to reflect the current rotation.
    public virtual void UpdateEnterDirections()
    {
        //Default to EnterDirections of Up and Down (at Rotate0)
        //Child clases with different layouts (corners etc.) will overwrite this.
        EnterDirectionA = (Directions)BlockRotation;

        EnterDirectionB = (Directions)(((int)BlockRotation + 2) % 4);

        //Update the active sprite for the block to match the new rotation
        switch (BlockRotation)
        {
            case Rotations.Rotate0:
                spriteRenderer.sprite = sprite0;
                break;

            case Rotations.Rotate90:
                spriteRenderer.sprite = sprite90;
                break;

            case Rotations.Rotate180:
                spriteRenderer.sprite = sprite180;
                break;

            case Rotations.Rotate270:
                spriteRenderer.sprite = sprite270;
                break;
        }
    }

    //The player can rotate blocks clockwise when holding them.
    //This changes the enter and exit directions of the block as well as its shwon image.
    public virtual void RotateClockwise()
    {
        //Rotation cannot occur if there is energy in the block.
        if (!HasEnergy)
        {
            //Increase the block's rotation value by 1, looping back to Rotate0 if necessary.
            BlockRotation = (Rotations)(((int)BlockRotation + 1) % 4);
            //Update the block's 2 EnterDirections to reflect the new rotation.
            UpdateEnterDirections();
        }
        else
        {
            Debug.Log("Cannot rotate block as it currently contains energy.");
        }
    }

    //Most blocks will apply an effect to energy that passes through.
    //The effect depends on the block.
    public virtual void ModifyEnergy()
    {
        //No modification by default (used for path blocks)
    }

    //Based on this block's ExitDirection, find the appropriate adjacent block.
    public virtual void FindSendTarget()
    {
        Node_Block targetBlock;
        //Debug.Log("Finding Send target...");

        switch (ExitDirection)
        {
            case Directions.Up:
                if (GridPosition.y - 1 >= 0)
                {
                    if (Grid.nodeGrid[(int)GridPosition.x, (int)GridPosition.y - 1] != null)
                    {
                        targetBlock = Grid.nodeGrid[(int)GridPosition.x, (int)GridPosition.y - 1].GetComponent<Node_Block>();
                        AttemptSend(targetBlock);
                    }
                    else
                    {
                        DeleteEnergy();
                        break;
                    }
                    
                }
                //If the target block would be off the top of the grid, send to an Action slot.
                else
                {
                    Action_Bar.actions[Mathf.FloorToInt(GridPosition.x / 2)].GetComponent<Action_Hub>().ReceiveEnergy(PossessedEnergy);
                    PossessedEnergy = null;
                    HasEnergy = false;
                }

                break;

            case Directions.Right:
                if (GridPosition.x + 1 < Grid.nodeGrid.GetLength(0))
                {
                    if (Grid.nodeGrid[(int)GridPosition.x + 1, (int)GridPosition.y] != null)
                    {
                        targetBlock = Grid.nodeGrid[(int)GridPosition.x + 1, (int)GridPosition.y].GetComponent<Node_Block>();
                        AttemptSend(targetBlock);
                    }
                    else
                    {
                        //Send fails if targetBlock doesn't exist.
                        DeleteEnergy();
                        break;
                    }
                }
                else
                {
                    DeleteEnergy();
                }

                break;

            case Directions.Down:
                if (GridPosition.y + 1 < Grid.nodeGrid.GetLength(1))
                {
                    if (Grid.nodeGrid[(int)GridPosition.x, (int)GridPosition.y + 1] != null)
                    {
                        targetBlock = Grid.nodeGrid[(int)GridPosition.x, (int)GridPosition.y + 1].GetComponent<Node_Block>();
                        AttemptSend(targetBlock);
                    }
                    else
                    {
                        //Send fails if targetBlock doesn't exist.
                        DeleteEnergy();
                        break;
                    }
                }
                else
                {
                    DeleteEnergy();
                }

                break;

            case Directions.Left:
                if (GridPosition.x - 1 >= 0)
                {
                    if (Grid.nodeGrid[(int)GridPosition.x - 1, (int)GridPosition.y] != null)
                    {
                        targetBlock = Grid.nodeGrid[(int)GridPosition.x - 1, (int)GridPosition.y].GetComponent<Node_Block>();
                        AttemptSend(targetBlock);
                    }
                    else
                    {
                        //Send fails if targetBlock doesn't exist.
                        DeleteEnergy();
                        break;
                    }
                }
                else
                {
                    DeleteEnergy();
                }

                break;
        }
    }

    //Transfer energy from this block to another that has been found and approved by AttemptSend
    public void AttemptSend(Node_Block targetBlock)
    {
        //Debug.Log("Target block exists, really attempting send...");
        if (!targetBlock.HasEnergy)
        {
            if (targetBlock.EnterDirectionA == (Directions)(((int)ExitDirection + 2) % 4)) //Results in opposite direction from ExitDirection.
            {
                if (!targetBlock.IsSource && !targetBlock.IsReceiver) //Sources and receivers don't have dynamic ExitDirections.
                {
                    targetBlock.ExitDirection = targetBlock.EnterDirectionB;
                }
                targetBlock.HasEnergy = true;
                targetBlock.PossessedEnergy = PossessedEnergy;
                PossessedEnergy.EnteredNewBlock();
                PossessedEnergy = null;
                HasEnergy = false;
            }
            else if (targetBlock.EnterDirectionB == (Directions)(((int)ExitDirection + 2) % 4) && !targetBlock.IsSource && !targetBlock.IsReceiver) //Sources and receivers don't have a second EnterDirection.
            {
                targetBlock.ExitDirection = targetBlock.EnterDirectionA;
                targetBlock.HasEnergy = true;
                targetBlock.PossessedEnergy = PossessedEnergy;
                PossessedEnergy.EnteredNewBlock();
                PossessedEnergy = null;
                HasEnergy = false;
            }
            else
            {
                //Send fails if targetBlock doesn't have the corresponding EnterDirection.
                DeleteEnergy();
            }
        }
        else
        {
            //Send fails if targetBlock already has energy.
            DeleteEnergy();
        }
    }

    //Removes energy from this block and deletes the energy from the scene. Called whenever a send fails.
    public void DeleteEnergy()
    {
        Debug.Log("Deleting Energy...");
        Destroy(PossessedEnergy.gameObject);
        PossessedEnergy = null;
        HasEnergy = false;
    }
}
