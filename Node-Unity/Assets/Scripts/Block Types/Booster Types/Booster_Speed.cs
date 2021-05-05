using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_Speed : Node_Booster
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        //Speed is a passive effect, so it shouldn't override other boosters.
        //boosterType = Energy.Boosters.Speed;
        BlockPath = "Boosters/Booster_Speed";
        toolTip = Tooltip_Manager.ToolTips.Speed;
        base.Awake();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ModifyEnergy()
    {
        base.ModifyEnergy();
        PossessedEnergy.travelSpeed -= 0.45f;
        //Set a maximum speed on energy so it doesn't teleport straight to the end of the path.
        if (PossessedEnergy.travelSpeed < 0.1f)
        {
            PossessedEnergy.travelSpeed = 0.1f;
        }
    }
}
