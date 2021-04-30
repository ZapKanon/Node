using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Straight : Node_Path
{
    // Start is called before the first frame update
    protected override void Start()
    {
        EnterDirectionA = Directions.Up;
        EnterDirectionB = Directions.Down;
        BlockPath = "Paths/Path_Straight";
        toolTip = Tooltip_Manager.ToolTips.Straight;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
