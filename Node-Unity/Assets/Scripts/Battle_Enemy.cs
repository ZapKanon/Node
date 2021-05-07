using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public abstract class Battle_Enemy : Battle_Character
{
    //Sounds: EnemyAttack, EnemyTakeDamage, EnemyDefeat
    [FMODUnity.EventRef] public string eventPathEnemyAttack;
    private EventInstance eventEnemyAttack;

    [FMODUnity.EventRef] public string eventPathEnemyTakeDamage;
    private EventInstance eventEnemyTakeDamage;

    [FMODUnity.EventRef] public string eventPathEnemyDefeat;
    private EventInstance eventEnemyDefeat;

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

    [SerializeField] protected SpriteRenderer activeSprite;
    [SerializeField] protected SpriteRenderer inactiveSprite;

    //TEMP Y positions for movement when attacking.
    public float up;
    public float neutral;
    public float down;
    public bool actionSoundTriggered;
    public bool inNeutralPosition;
    public bool woundUp;
    public bool completedAction;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        eventEnemyAttack = FMODUnity.RuntimeManager.CreateInstance(eventPathEnemyAttack);
        eventEnemyTakeDamage = FMODUnity.RuntimeManager.CreateInstance(eventPathEnemyTakeDamage);
        eventEnemyDefeat = FMODUnity.RuntimeManager.CreateInstance(eventPathEnemyDefeat);

        up = transform.position.y + 0.05f;
        neutral = transform.position.y;
        down = transform.position.y - 0.1f;
        activeSprite.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateActionTimer();
        AnimateHealthBar();
    }

    //Enemies perform actions at a consistent rate.
    public void UpdateActionTimer()
    {
        actionTimer += Time.deltaTime;

        //TEMP visual feedback, return to default Y position after attacking.
        if (actionTimer >= 0 && actionTimer < 1f)
        {
            if (inNeutralPosition == false)
            {
                if (transform.position.y < neutral)
                {
                    transform.position += new Vector3(0, 0.15f * Time.deltaTime, 0);
                    activeSprite.color += new Color(0, 0, 0, -2f * Time.deltaTime);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, neutral, transform.position.z);
                    inNeutralPosition = true;
                    activeSprite.color = new Color(1, 1, 1, 0);
                }
            }
        }

        else if (actionTimer >= actionSpeed - 2f && actionTimer < actionSpeed)
        {
            if (actionSoundTriggered == false)
            {
                //Trigger a sound.
                eventEnemyAttack.start();
                actionSoundTriggered = true;
            }

            //TEMP visual feedback, enemy moves upward while winding up their attack.
            if (woundUp == false)
            {
                if (transform.position.y < up)
                {
                    transform.position += new Vector3(0, 0.04f * Time.deltaTime, 0);
                    activeSprite.color += new Color(0, 0, 0, 1f * Time.deltaTime);
                    inNeutralPosition = false;
                }
                //Set enemy Y position exactly once windup is complete.
                else
                {
                    transform.position = new Vector3(transform.position.x, up, transform.position.z);
                    woundUp = true;
                    activeSprite.color = new Color(1, 1, 1, 1);
                }
            }
            else
            {
                if (transform.position.y > down)
                {
                    transform.position += new Vector3(0, -0.5f * Time.deltaTime, 0);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, down, transform.position.z);
                    completedAction = true;
                }
            }
        }

        if (completedAction == true)
        {
            Debug.Log("Executing enemy action.");
            ExecuteAction();
            actionTimer -= actionSpeed;
            woundUp = false;
            completedAction = false;
            actionSoundTriggered = false;
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
        //Trigger a sound.
        eventEnemyDefeat.start();

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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
        else
        {
            //Trigger a sound.
            //eventEnemyTakeDamage.start();
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
