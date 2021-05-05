using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Electric : Node_Element
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        EnterDirectionA = Directions.Up;
        EnterDirectionB = Directions.Down;
        elementType = Energy.Elements.Electric;
        BlockPath = "Elements/Element_Electric";
        toolTip = Tooltip_Manager.ToolTips.Electric;
        tooltipColor = new Color32(255, 255, 51, 255);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
