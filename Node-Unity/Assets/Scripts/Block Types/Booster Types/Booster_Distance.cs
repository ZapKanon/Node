using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_Distance : Node_Booster
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        boosterType = Energy.Boosters.Distance;
        BlockPath = "Boosters/Booster_Distance";
        toolTip = Tooltip_Manager.ToolTips.Distance;
        base.Awake();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
