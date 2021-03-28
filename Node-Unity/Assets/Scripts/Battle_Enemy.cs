using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle_Enemy : Battle_Character
{
    [SerializeField] protected Energy[] availableActions;
    [SerializeField] protected float actionTimer;
    protected float actionSpeed;

    [SerializeField] private Sprite normalSprite;

    [SerializeField] protected GameObject healthBarObject;
    [SerializeField] protected GameObject damageTakenObject;
    [SerializeField] protected float currentAnimatedHealth;
    protected float healthDrainAnimationSpeed;
    protected float healthDrainAnimationDelay;

    protected float healthDrainAnimationTimePassed;

    public bool down;

    // Start is called before the first frame update
    //protected override void Start()
    //{
    //    base.Start();
    //    SetUpActions();
    //}

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateActionTimer();
        AnimateHealthBar();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    //Enemies perform actions at a consistent rate.
    public void UpdateActionTimer()
    {
        actionTimer += Time.deltaTime;

        if (actionTimer >= actionSpeed / 7f && down == true)
        {
            transform.position += new Vector3(0, 0.05f, 0);
            down = false;
        }

        if (actionTimer >= actionSpeed)
        {
            Debug.Log("Executing enemy action.");
            ExecuteAction();
            actionTimer -= actionSpeed;
        }
    }

    //Initialize the enemy's specific available actions and add them to the list
    protected abstract void SetUpActions();

    //Enemy chooses which of its available actions to perform.
    //This decision making process varies by enemy but is likely % chances with weighting.
    protected abstract Energy ChooseAction();

    //Choose and perform an action from the available actions list.
    //The action is sent to the chosen target.
    public void ExecuteAction()
    {
        Energy currentAction = ChooseAction();
        currentAction.Execute();

        //TEMP movement code for visual feedback
        if (!down)
        {
            transform.position += new Vector3(0, -0.05f, 0);
            down = true;
        }
    }

    //Select this enemy when clicked if active.
    protected void OnMouseDown()
    {
        Select();
    }

    //Enemy selection only matters when targeting an enemy after selecting a player action.
    public void Select()
    {
        Debug.Log("Enemy selected!");
        //Set this enemy as selected.
        Battle_Manager.selectedEnemy = this;
        //spriteRenderer.sprite = selectedSprite;
    }

    //Destroy the enemy when health reaches 0.
    public void Death()
    {
        battleManager.DeadEnemy(this);
        Destroy(gameObject);
    }

    //Update the player's currentHealth and animate their health bar.
    //The red health bar changes to the new currentHealth value immediately.
    //AnimateHealthBar is then called to inform behvaior of the yellow bar
    public override void TakeDamage(float damageTaken, Energy.Elements element)
    {
        float previousHealth = currentHealth;
        currentHealth -= damageTaken * elementalWeaknesses[(int)element];

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBarObject.transform.localScale = new Vector3(currentHealth * (1.0f / maxHealth) * 0.6f, healthBarObject.transform.localScale.y, healthBarObject.transform.localScale.z);

        //Reset time passed and position for animating yellow bar to interrupt any previous animation
        currentAnimatedHealth = previousHealth;
        damageTakenObject.transform.localScale = new Vector3(previousHealth * (1.0f / maxHealth) * 0.6f, damageTakenObject.transform.localScale.y, damageTakenObject.transform.localScale.z);
        healthDrainAnimationTimePassed = 0;
    }

    //Animate draining of the yellow health bar to show recently taken damage.
    protected void AnimateHealthBar()
    {
        //If currentAnimatedHealth is greater than actual health, reduce the length of the yellow bar by healthDrainAnimationSpeed.
        if (currentAnimatedHealth > currentHealth)
        {
            //Wait until the amount of time specified by healthDrainAnimationDelay to begin animating health drain.
            if (healthDrainAnimationTimePassed <= healthDrainAnimationDelay)
            {
                healthDrainAnimationTimePassed += Time.deltaTime;
            }
            else
            {
                currentAnimatedHealth -= healthDrainAnimationSpeed * Time.deltaTime;
                damageTakenObject.transform.localScale = new Vector3((currentAnimatedHealth) * (1.0f / maxHealth) * 0.6f, damageTakenObject.transform.localScale.y, damageTakenObject.transform.localScale.z);
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
