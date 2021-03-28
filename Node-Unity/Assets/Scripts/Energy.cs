using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public enum Elements
    {
        Normal,
        Fire,
        Ice,
        Electric
    }

    public enum Conductors
    {
        Attack,
        Shield,
        Reflect,
        Heal
    }

    public enum Boosters
    {
        None,
        Distance
    }

    [field:SerializeField] public float Strength { get; set; }

    [field: SerializeField] public float BoosterStrength { get; set; }
    [field: SerializeField] public Elements Element { get; set; }
    [field: SerializeField] public Conductors Conductor { get; set; }
    [field: SerializeField] public Boosters Booster { get; set; }
    [field: SerializeField] public Vector2 GridPosition { get; set; }


    [SerializeField] private Battle_Character[] validTargets;
    public Battle_Character currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        //All energy defaults to Attack type
        Conductor = Conductors.Attack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called when energy successfully moves from one block to another.
    public void EnteredNewBlock()
    {
        //By default, energy gains 1 strength per block entered.
        Strength++;

        //The booster conductor increments a counter by 1 per block entered.
        if (Booster == Boosters.Distance)
        {
            BoosterStrength++;
        }
    }

    //Applies this energy's effects to a previously specified target if valid.
    public void Execute()
    {
        if (CheckValidTarget())
        {
            currentTarget.ReceiveEnergy(this);
            //Debug.Log("Actually sending energy to valid target.");
        }
    }

    //Determine if current target exists and isn't dead.
    public virtual bool CheckValidTarget()
    {
        //Debug.Log("Is the target valid?");
        Debug.Log(currentTarget);
        if (currentTarget != null && currentTarget.currentHealth > 0)
        {
            return true;
        }

        //Debug.Log("No!");
        return false;
    }
}
