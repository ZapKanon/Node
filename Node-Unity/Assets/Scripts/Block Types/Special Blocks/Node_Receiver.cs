using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Receiver : Node_Block
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        EnterDirectionA = Directions.Down;
        //Receiver blocks only have one unique entrance direction.
        EnterDirectionB = EnterDirectionA;
        ExitDirection = Directions.Up;
        BlockPath = "Special/Node_Receiver";
        base.Awake();

        IsReceiver = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void RotateClockwise()
    {
        //Receiver blocks cannot be rotated.
    }
}
