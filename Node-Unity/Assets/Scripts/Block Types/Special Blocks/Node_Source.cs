using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Source : Node_Block
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        EnterDirectionA = Directions.Down;
        //Source blocks only have one unique entrance direction, but it shouldn't be used at all anyway.
        EnterDirectionB = EnterDirectionA;
        ExitDirection = Directions.Up;
        BlockPath = "Special/Node_Source";
        toolTip = Tooltip_Manager.ToolTips.Source;
        base.Awake();

        IsSource = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void RotateClockwise()
    {
        //Source blocks cannot be rotated.
    }
}
