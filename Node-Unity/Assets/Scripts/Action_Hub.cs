using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Action_Hub : MonoBehaviour
{
    //Sounds: ActionSelect, ActionDeselect, ActionActivation, ActionWaiting
    [FMODUnity.EventRef] public string eventPathSelectAction;
    private EventInstance eventSelectAction;

    [FMODUnity.EventRef] public string eventPathDeselectAction;
    private EventInstance eventDeselectAction;

    [FMODUnity.EventRef] public string eventPathActionActivation;
    private EventInstance eventActionActivation;

    [FMODUnity.EventRef] public string eventPathActionWaiting;
    private EventInstance eventActionWaiting;

    public bool Active { get; set; } //Does this action contain energy?

    public bool Selected { get; set; } //Has this action been selected for use?

    public Energy PossessedEnergy { get; set; }

    public SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private GameObject conductorObject;
    [SerializeField] private GameObject elementObject;
    [SerializeField] private GameObject boosterObject;

    public SpriteRenderer conductorRenderer;
    public SpriteRenderer elementRenderer;
    public SpriteRenderer boosterRenderer;

    FMOD.Studio.PLAYBACK_STATE selectActionState;
    FMOD.Studio.PLAYBACK_STATE actionWaitingState;

    bool alphaIncreasing; // Used for pulsing of sprite while actionWaiting plays.
    float waitingValue;

    // Start is called before the first frame update
    void Start()
    {
        eventSelectAction = FMODUnity.RuntimeManager.CreateInstance(eventPathSelectAction);
        eventDeselectAction = FMODUnity.RuntimeManager.CreateInstance(eventPathDeselectAction);
        eventActionActivation = FMODUnity.RuntimeManager.CreateInstance(eventPathActionActivation);
        eventActionWaiting = FMODUnity.RuntimeManager.CreateInstance(eventPathActionWaiting);

        Active = false;
        Selected = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        conductorRenderer = conductorObject.GetComponent<SpriteRenderer>();
        elementRenderer = elementObject.GetComponent<SpriteRenderer>();
        boosterRenderer = boosterObject.GetComponent<SpriteRenderer>();

        alphaIncreasing = false;
        waitingValue = 0.54f; //Length of time for a hub's alpha value to travel from 1.0 -> 0.0 or vice versa. (Should be equal to 1/2 duration of actionWaiting sound.)
    }

    // Update is called once per frame
    void Update()
    {
        if (Selected)
        {
            eventSelectAction.getPlaybackState(out selectActionState);
            eventActionWaiting.getPlaybackState(out actionWaitingState);

            //Once the selectAction sound has finished playing, start the actionWaiting loop if it hasn't started already.
            if (selectActionState == FMOD.Studio.PLAYBACK_STATE.STOPPED && actionWaitingState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                //Trigger a sound.
                eventActionWaiting.start();
            }


            //Change alpha value to match actionWaiting pulsing sound
            Color tempColor = spriteRenderer.color;

            if (alphaIncreasing == false)
            {
                tempColor.a -= Time.deltaTime / waitingValue;
                spriteRenderer.color = tempColor;

                if (spriteRenderer.color.a <= 0f)
                {
                    alphaIncreasing = true;
                }
            }
            else
            {
                tempColor.a += Time.deltaTime / waitingValue;
                spriteRenderer.color = tempColor;

                if (spriteRenderer.color.a >= 1f)
                {
                    alphaIncreasing = false;
                }
            }
        }
    }

    //Receive energy from a Node_Block and become an active action.
    public void ReceiveEnergy(Energy receivedEnergy)
    {
        //If this action already possessed energy, prevent new energy from overwriting.

        if (PossessedEnergy == null)
        {
            PossessedEnergy = receivedEnergy;
            Active = true;
            spriteRenderer.sprite = activeSprite;
            PossessedEnergy.GetComponent<SpriteRenderer>().enabled = false;

            DisplayActionData();

            //Trigger a sound.
            eventActionActivation.start();
        }
        else
        {
            Destroy(receivedEnergy.gameObject);
        }
    }

    //Show data sprites based on action parameters obtained from energy.
    public void DisplayActionData()
    {
        //Create filepaths based on energy parameters.
        string conductorPath = "ConductorSprites/" + PossessedEnergy.Conductor.ToString() + "Icon";
        string elementPath = "ElementSprites/" + PossessedEnergy.Element.ToString() + "Icon";
        string boosterPath = "BoosterSprites/" + PossessedEnergy.Booster.ToString() + "Icon";

        Debug.Log(boosterPath);

        //Obtain and set the sprites from constructed filepaths.
        conductorRenderer.sprite = Resources.Load<Sprite>(conductorPath);
        elementRenderer.sprite = Resources.Load<Sprite>(elementPath);
        boosterRenderer.sprite = Resources.Load<Sprite>(boosterPath);
    }

    //Receive enemy selection from Battle_Manager and execute action with that target.
    public void TargetEnemy(Battle_Enemy targetedEnemy)
    {
        PossessedEnergy.currentTarget = targetedEnemy;
        PossessedEnergy.Execute(Battle_Manager.selectedEnemy);

        //Trigger a sound.
        eventActionWaiting.stop(STOP_MODE.IMMEDIATE);

        Battle_Manager.selectedAction = null;
        Deactivate();
    }

    private void OnMouseOver()
    {
        if (Active)
        Battle_Manager.tooltipManager.UpdateToolTip(Tooltip_Manager.ToolTips.Action, null, this);

    }

    //Select this action when clicked if active.
    private void OnMouseDown()
    {
        if (Active)
        {
            Select();
        }
    }

    private void OnMouseExit()
    {
        Battle_Manager.tooltipManager.ClearToolTip();
    }

    //Set this action as selected for use.
    public void Select()
    {
        //Disable selection status on all actions.
        foreach (GameObject action in Action_Bar.actions)
        {
            action.GetComponent<Action_Hub>().Deselect(false);
        }

        //Then set this action as selected.
        Selected = true;
        Battle_Manager.selectedAction = this;
        spriteRenderer.sprite = selectedSprite;

        //Healing actions automatically execute on selection (for now you can't heal enemies)
        if (PossessedEnergy.Conductor == Energy.Conductors.Heal)
        {
            PossessedEnergy.currentTarget = Battle_Manager.player;
            PossessedEnergy.Execute(PossessedEnergy.currentTarget);

            Battle_Manager.selectedAction = null;
            Deactivate();
        }
        else
        {
            //Trigger a sound.
            eventSelectAction.start();
        }
    }

    public void Deselect(bool deactivating)
    {
        if (Selected == true)
        {
            Selected = false;

            if (Active)
            {
                spriteRenderer.sprite = activeSprite;
            }
            else
            {
                spriteRenderer.sprite = inactiveSprite;
            }

            //Only play deselection sound if not executing an attack.
            if (deactivating == false)
            {
                //Trigger a sound.
                eventActionWaiting.stop(STOP_MODE.IMMEDIATE);
                eventDeselectAction.start();
            }
        }

        ResetAlpha();
        Battle_Manager.tooltipManager.ClearToolTip();
    }

    //Clear this action's PossessedEnergy and set all sprites to inactive states.
    public void Deactivate()
    {
        Active = false;
        Deselect(true);
        spriteRenderer.sprite = inactiveSprite;
        conductorRenderer.sprite = null;
        elementRenderer.sprite = null;
        boosterRenderer.sprite = null;

        Destroy(PossessedEnergy.gameObject);
        PossessedEnergy = null;
    }

    //When deselecting a hub, rest opacity to 100%.
    public void ResetAlpha()
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = 1.0f;
        spriteRenderer.color = tempColor;

        alphaIncreasing = false;
    }
}
