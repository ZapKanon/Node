using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A red-faced, draconic enemy

public class Jaou_Enemy : Battle_Enemy
{
    // Start is called before the first frame update
    //Individual enemies sepcify their action speeds
    protected override void Start()
    {
        base.Start();
        actionSpeed = 5.0f;
        maxHealth = 100;
        currentHealth = maxHealth;
        SetUpActions();

        healthDrainAnimationSpeed = 10.0f;
        healthDrainAnimationDelay = 1.0f;
        healthDrainAnimationTimePassed = 0f;
        elementalWeaknesses[1] = 1.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();  
    }

    //This enemy's actions
    //Created here and added to availableActions list
    protected override void SetUpActions()
    {
        Energy newAction;
        availableActions = new Energy[2];

        //Action 0: Normal Attack
        newAction = gameObject.AddComponent<Energy>();

        newAction.Strength = 8;
        newAction.Conductor = Energy.Conductors.Attack;
        newAction.Element = Energy.Elements.Normal;
        newAction.Booster = Energy.Boosters.None;
        newAction.currentTarget = Battle_Manager.player;

        availableActions[0] = newAction;

        //Action 1: Stronger Fire Attack
        newAction = gameObject.AddComponent<Energy>();

        newAction.Strength = 15;
        newAction.Conductor = Energy.Conductors.Attack;
        newAction.Element = Energy.Elements.Fire;
        newAction.Booster = Energy.Boosters.None;
        newAction.currentTarget = Battle_Manager.player;

        availableActions[1] = newAction;
    }

    protected override Energy ChooseAction()
    {
        float actionRoll = random.Next(0, 101);
        
        //This enemy executes Action 0 70% of the time and Action 1 the remaining 30% of the time
        if (actionRoll <= 70)
        {
            return availableActions[0];
        }
        else
        {
            return availableActions[1];
        }
    }
}
