using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

//A visual copy of Action_Hub. Used to match the appearance of a hovered action in the tooltip interface.
public class Tooltip_Action : MonoBehaviour
{
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        conductorRenderer = conductorObject.GetComponent<SpriteRenderer>();
        elementRenderer = elementObject.GetComponent<SpriteRenderer>();
        boosterRenderer = boosterObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Copy the appearance of a real Action_Hub.
    public void CopyActionVisual(Action_Hub realAction)
    {
        gameObject.SetActive(true);
        conductorRenderer.sprite = realAction.conductorRenderer.sprite;
        elementRenderer.sprite = realAction.elementRenderer.sprite;
        boosterRenderer.sprite = realAction.boosterRenderer.sprite;
    }

    //Resets the appearance of this tooltip action.
    public void ClearVisual()
    {
        if (gameObject.activeInHierarchy == true)
        {
            conductorRenderer.sprite = null;
            elementRenderer.sprite = null;
            boosterRenderer.sprite = null;
            gameObject.SetActive(false);
        }
    }
}
