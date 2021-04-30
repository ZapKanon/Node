using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Governs tooltips displayed in the upper-right corner of the battle screen.
//The displayed tooltip corresponds to the element over which the player is currently hovering.
public class Tooltip_Manager : MonoBehaviour
{
    public GameObject sourceTooltip;
    public GameObject straightTooltip;
    public GameObject cornerTooltip;
    public GameObject fireTooltip;
    public GameObject iceTooltip;
    public GameObject electricTooltip;
    public GameObject healTooltip;
    public GameObject distanceTooltip;
    public GameObject actionTooltip;

    public enum ToolTips
    {
        Source,
        Straight,
        Corner,
        Fire,
        Ice,
        Electric,
        Heal,
        Distance,
        Action
    }

    public GameObject currentTooltip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changes the visible tooltip image.
    public void UpdateToolTip(ToolTips newTooltip)
    {
        Debug.Log("Updating tooltip");
        if (currentTooltip != null)
        currentTooltip.SetActive(false);

        switch (newTooltip)
        {
            case ToolTips.Source:
                currentTooltip = sourceTooltip;
                break;
            case ToolTips.Straight:
                currentTooltip = straightTooltip;
                break;
            case ToolTips.Corner:
                currentTooltip = cornerTooltip;
                break;
            case ToolTips.Fire:
                currentTooltip = fireTooltip;
                break;
            case ToolTips.Ice:
                currentTooltip = iceTooltip;
                break;
            case ToolTips.Electric:
                currentTooltip = electricTooltip;
                break;
            case ToolTips.Heal:
                currentTooltip = healTooltip;
                break;
            case ToolTips.Distance:
                currentTooltip = distanceTooltip;
                break;
            case ToolTips.Action:
                currentTooltip = actionTooltip;
                break;
        }

        currentTooltip.SetActive(true);
    }

    //Returns the tooltip area to a blank state.
    public void ClearToolTip()
    {
        if (currentTooltip != null)
        {
            currentTooltip.SetActive(false);
            currentTooltip = null;
        }
    }
}
