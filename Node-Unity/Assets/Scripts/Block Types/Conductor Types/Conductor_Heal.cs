using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor_Heal : Node_Conductor
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        conductorType = Energy.Conductors.Heal;
        BlockPath = "Conductors/Conductor_Heal";
        toolTip = Tooltip_Manager.ToolTips.Heal;
        tooltipColor = new Color32(75, 183, 73, 255);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
