using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle_Player : Battle_Character
{
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private GameObject damageTakenObject;
    [SerializeField] private float currentAnimatedHealth;
    private float healthDrainAnimationSpeed;
    private float healthDrainAnimationDelay;

    private float healthDrainAnimationTimePassed;

    [SerializeField] private bool testDamage;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        maxHealth = 100.0f;
        currentHealth = maxHealth;
        currentAnimatedHealth = maxHealth;

        healthDrainAnimationSpeed = 10.0f;
        healthDrainAnimationDelay = 1.0f;
        healthDrainAnimationTimePassed = 0f;

        //TakeDamage(30.0f);
        testDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHealthBar();
        previousHealth = currentHealth;

        //Test to simulate taking damage
        if (testDamage == true)
        {
            TakeDamage(30f, Energy.Elements.Normal);
            testDamage = false;
        }
        
    }

    //Update the player's currentHealth and animate their health bar.
    //The red health bar changes to the new currentHealth value immediately.
    //AnimateHealthBar is then called to inform behvaior of the yellow bar
    public override void TakeDamage(float damageTaken, Energy.Elements element)
    {
        previousHealth = currentHealth;
        currentHealth -= damageTaken * elementalWeaknesses[(int)element];

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBarObject.transform.localScale = new Vector3(currentHealth * (1.0f / maxHealth), healthBarObject.transform.localScale.y, healthBarObject.transform.localScale.z);

        //Reset time passed and position for animating yellow bar to interrupt any previous animation
        currentAnimatedHealth = previousHealth;
        damageTakenObject.transform.localScale = new Vector3(previousHealth * (1.0f / maxHealth), damageTakenObject.transform.localScale.y, damageTakenObject.transform.localScale.z);
        healthDrainAnimationTimePassed = 0;
    }

    //Animate draining of the yellow health bar to show recently taken damage.
    void AnimateHealthBar()
    {
        //If health is greater than expected (the playr has been healed, for example), immediately update both health bars.
        if (previousHealth < currentHealth)
        {
            healthBarObject.transform.localScale = new Vector3(currentHealth * (1.0f / maxHealth), healthBarObject.transform.localScale.y, healthBarObject.transform.localScale.z);

            currentAnimatedHealth = currentHealth;
            damageTakenObject.transform.localScale = new Vector3(currentHealth * (1.0f / maxHealth), damageTakenObject.transform.localScale.y, damageTakenObject.transform.localScale.z);
            healthDrainAnimationTimePassed = 0;
        }

        //If currentAnimatedHealth is greater than actual health, reduce the length of the yellow bar by healthDrainAnimationSpeed.
        else if (currentAnimatedHealth > currentHealth)
        {
            //Wait until the amount of time specified by healthDrainAnimationDelay to begin animating health drain.
            if (healthDrainAnimationTimePassed <= healthDrainAnimationDelay)
            {
                healthDrainAnimationTimePassed += Time.deltaTime;
            }
            else
            {
                currentAnimatedHealth -= healthDrainAnimationSpeed * Time.deltaTime;
                damageTakenObject.transform.localScale = new Vector3 ((currentAnimatedHealth) * (1.0f / maxHealth), damageTakenObject.transform.localScale.y, damageTakenObject.transform.localScale.z);
            }
        }
        //Reset healthDrainAnimationTimePassed once animation is complete
        //If currentAnimationHealth is somehow lower than currentHealth, set it to be equal to currentHealth.
        else
        {
            healthDrainAnimationTimePassed = 0f;
            currentAnimatedHealth = currentHealth;
        }
    }
}
