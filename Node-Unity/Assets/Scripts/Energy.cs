using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Energy : MonoBehaviour
{
    //Sounds: Attack_Normal, Attack_Fire, Attack_Electric, Attack_Ice, Heal
    [FMODUnity.EventRef] public string eventPathAttackNormal;
    private EventInstance eventAttackNormal;

    [FMODUnity.EventRef] public string eventPathAttackFire;
    private EventInstance eventAttackFire;

    [FMODUnity.EventRef] public string eventPathAttackElectric;
    private EventInstance eventAttackElectric;

    [FMODUnity.EventRef] public string eventPathAttackIce;
    private EventInstance eventAttackIce;

    [FMODUnity.EventRef] public string eventPathHeal;
    private EventInstance eventHeal;

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
        Distance,
        Speed
    }

    [field:SerializeField] public float Strength { get; set; }

    [field: SerializeField] public float BoosterStrength { get; set; }
    [field: SerializeField] public Elements Element { get; set; }
    [field: SerializeField] public Conductors Conductor { get; set; }
    [field: SerializeField] public Boosters Booster { get; set; }
    [field: SerializeField] public Vector2 GridPosition { get; set; }


    [SerializeField] private Battle_Character[] validTargets;
    public Battle_Character currentTarget;

    public float travelSpeed;

    // Start is called before the first frame update
    void Start()
    {
        eventAttackNormal = FMODUnity.RuntimeManager.CreateInstance(eventPathAttackNormal);
        eventAttackFire = FMODUnity.RuntimeManager.CreateInstance(eventPathAttackFire);
        eventAttackElectric = FMODUnity.RuntimeManager.CreateInstance(eventPathAttackElectric);
        eventAttackIce = FMODUnity.RuntimeManager.CreateInstance(eventPathAttackIce);
        eventHeal = FMODUnity.RuntimeManager.CreateInstance(eventPathHeal);

        //All energy defaults to Attack type
        Conductor = Conductors.Attack;

        //Set travel speed to default according to Battle_Manager.
        travelSpeed = Battle_Manager.energyTravelSpeed;
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
    public void Execute(Battle_Character target)
    {
        Debug.Log("Executing...");
        if (CheckValidTarget(target))
        {
            Debug.Log("Valid!");
            currentTarget.ReceiveEnergy(this);

            if (Conductor == Energy.Conductors.Attack)
            {
                //Trigger a sound for each attack element.
                if (Element == Energy.Elements.Normal)
                {
                    eventAttackNormal.start();
                }
                else if (Element == Energy.Elements.Fire)
                {
                    eventAttackFire.start();
                }
                else if (Element == Energy.Elements.Electric)
                {
                    eventAttackElectric.start();
                }
                else if (Element == Energy.Elements.Ice)
                {
                    eventAttackIce.start();
                }
            }
            else if (Conductor == Energy.Conductors.Heal)
            {
                eventHeal.start();
            }
        }
        //TODO: Make this less jank. Enemies currently have the energy script attached to them directly so destroying the energy gameObject would destroy the enemy entirely.
        if (gameObject.TryGetComponent(out SpriteRenderer _))
        {
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
    }

    //Determine if current target exists and isn't dead.
    public virtual bool CheckValidTarget(Battle_Character target)
    {
        currentTarget = target;
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
