using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Fire : Node_Element
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        EnterDirectionA = Directions.Up;
        EnterDirectionB = Directions.Down;
        elementType = Energy.Elements.Fire;
        BlockPath = "Elements/Element_Fire";
        toolTip = Tooltip_Manager.ToolTips.Fire;
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
