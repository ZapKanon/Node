using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node_Conductor : Node_Block
{
    protected Energy.Conductors conductorType;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ModifyEnergy()
    {
        PossessedEnergy.Conductor = conductorType;
    }
}
