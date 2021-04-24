using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Ice : Node_Element
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        EnterDirectionA = Directions.Up;
        EnterDirectionB = Directions.Down;
        elementType = Energy.Elements.Ice;
        BlockPath = "Elements/Element_Ice";
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
