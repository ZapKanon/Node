using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Corner : Node_Path
{
    // Start is called before the first frame update
    protected override void Start()
    {
        EnterDirectionA = Directions.Right;
        EnterDirectionB = Directions.Down;
        BlockPath = "Paths/Path_Corner";
        UpdateEnterDirections();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UpdateEnterDirections()
    {
        base.UpdateEnterDirections();

        EnterDirectionA = (Directions)(((int)BlockRotation + 1) % 4);

        EnterDirectionB = (Directions)(((int)BlockRotation + 2) % 4);
    }
}
