using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor_Heal : Node_Conductor
{
    // Start is called before the first frame update
    protected override void Start()
    {
        conductorType = Energy.Conductors.Heal;
        BlockPath = "Conductors/Conductor_Heal";
        toolTip = Tooltip_Manager.ToolTips.Heal;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
