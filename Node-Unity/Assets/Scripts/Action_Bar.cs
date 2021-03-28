using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Bar : MonoBehaviour
{
    //The array of 10 action slots that sit above the node grid.
    public static GameObject[] actions;

    [SerializeField] private GameObject[] actionsProxy; //I'm going to figure out how to intelligently use singletons at some point but for now I'm using this silly method to get around Unity not serializing statics.

    // Start is called before the first frame update
    void Start()
    {
        actions = actionsProxy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
