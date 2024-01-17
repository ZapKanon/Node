using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle_Character : MonoBehaviour
{
    [SerializeField] protected Battle_Manager battleManager;

    public float maxHealth;
    public float currentHealth;
    public float previousHealth;

    protected float startHealthBarLengthScale;

    //Elemental weaknesses / resistances
    //Order matches Energy.Element enum (Normal, Fire, Ice, Electric)
    public float[] elementalWeaknesses;
    public float healAffinity;

    protected System.Random random;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        elementalWeaknesses = new float[4] { 1, 1, 1, 1};
        healAffinity = 1;
        random = new System.Random();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public virtual void TakeDamage(float damageTaken, Energy.Elements element)
    {
        currentHealth -= damageTaken * elementalWeaknesses[(int)element];
    }

    //Handles applying the effects of an action to this character.
    //Ex: Taking damage, incurring a status effect, etc.
    public virtual void ReceiveEnergy(Energy receivedEnergy)
    {
        //Booster behavior
        if (receivedEnergy.Booster == Energy.Boosters.Distance)
        {
            receivedEnergy.Strength += receivedEnergy.BoosterStrength;
        }

        //If the action is an attack:
        if (receivedEnergy.Conductor == Energy.Conductors.Attack)
        {
            TakeDamage(receivedEnergy.Strength, receivedEnergy.Element);
        }

        //If the action is a shield:
        if (receivedEnergy.Conductor == Energy.Conductors.Shield)
        {
            //TODO: Requires creation of status effects
        }

        //If the action is a reflect:
        if (receivedEnergy.Conductor == Energy.Conductors.Reflect)
        {
            //TODO: Requires creation of status effects
        }

        //If the action is a heal:
        if (receivedEnergy.Conductor == Energy.Conductors.Heal)
        {

            //TODO: Change this to use a Heal method
            Debug.Log("Current HP: " + currentHealth);
            Debug.Log("Gaining: " + receivedEnergy.Strength * healAffinity);

            if (currentHealth + receivedEnergy.Strength * healAffinity <= maxHealth)
            {
                currentHealth += receivedEnergy.Strength * healAffinity;
            }
            else
            {
                currentHealth = maxHealth;
            }
            Debug.Log("New HP: " + currentHealth);

        }
    }
}
