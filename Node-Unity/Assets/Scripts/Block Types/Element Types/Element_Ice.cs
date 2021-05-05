using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Ice : Node_Element
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        EnterDirectionA = Directions.Up;
        EnterDirectionB = Directions.Down;
        elementType = Energy.Elements.Ice;
        BlockPath = "Elements/Element_Ice";
        toolTip = Tooltip_Manager.ToolTips.Ice;
        tooltipColor = new Color32(32, 145, 244, 255);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
