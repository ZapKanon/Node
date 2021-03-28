using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Element : Node_Block
{
    protected Energy.Elements elementType;

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
        PossessedEnergy.Element = elementType;
    }
}
