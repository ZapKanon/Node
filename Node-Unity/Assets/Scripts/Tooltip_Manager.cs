using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Governs tooltips displayed in the upper-right corner of the battle screen.
//The displayed tooltip corresponds to the element over which the player is currently hovering.
public class Tooltip_Manager : MonoBehaviour
{

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
        Speed,
        Action
    }
    [SerializeField] private GameObject tooltipBlock;
    [SerializeField] private LocalizeStringEvent titleText;
    [SerializeField] private TextMeshProUGUI titleProperties;
    [SerializeField] private LocalizeStringEvent blockSubtitleText;
    [SerializeField] private LocalizeStringEvent blockDescriptionText;

    [SerializeField] private GameObject tooltipAction;
    [SerializeField] private Tooltip_Action tooltipActionIcon;
    [SerializeField] private LocalizeStringEvent actionSubtitleConductorText;
    [SerializeField] private LocalizeStringEvent actionSubtitleElementText;
    [SerializeField] private LocalizeStringEvent actionSubtitleBoosterText;
    [SerializeField] private LocalizeStringEvent actionDescriptionText;
    [SerializeField] private LocalizeStringEvent actionDash1Text;
    [SerializeField] private LocalizeStringEvent actionDash2Text;
    [SerializeField] private LocalizeStringEvent actionDash3Text;
    [SerializeField] private LocalizeStringEvent actionConductorText;
    [SerializeField] private TextMeshProUGUI actionConductorTextMesh;
    [SerializeField] private LocalizeStringEvent actionElementText;
    [SerializeField] private TextMeshProUGUI actionElementTextMesh;
    [SerializeField] private LocalizeStringEvent actionBoosterText;
    [SerializeField] private TextMeshProUGUI actionBoosterTextMesh;
    [SerializeField] private LocalizeStringEvent actionStrengthTitleText;
    [SerializeField] private TextMeshProUGUI actionStrengthValueText;

    public SpriteRenderer currentTooltipSprite;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        tooltipBlock.SetActive(false);
        tooltipAction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Changes the visible tooltip image.
    public void UpdateToolTip(ToolTips newTooltip, Node_Block hoveredBlock = null, Action_Hub hoveredHub = null)
    {
        titleText.StringReference.TableEntryReference = newTooltip.ToString() + "_Title";

        if (newTooltip == ToolTips.Action)
        {
            gameObject.SetActive(true);
            tooltipAction.SetActive(true);

            titleProperties.color = Color.black;

            actionSubtitleConductorText.StringReference.TableEntryReference = "Conductor";
            actionSubtitleElementText.StringReference.TableEntryReference = "Element";
            actionSubtitleBoosterText.StringReference.TableEntryReference = "Booster";
            actionDash1Text.StringReference.TableEntryReference = "Dash";
            actionDash2Text.StringReference.TableEntryReference = "Dash";
            actionDash3Text.StringReference.TableEntryReference = "Dash";
            actionConductorText.StringReference.TableEntryReference = hoveredHub.PossessedEnergy.Conductor.ToString() + "_Title";
            actionElementText.StringReference.TableEntryReference = hoveredHub.PossessedEnergy.Element.ToString() + "_Title";
            actionBoosterText.StringReference.TableEntryReference = hoveredHub.PossessedEnergy.Booster.ToString() + "_Title";
            SetTooltipActionColors(hoveredHub.PossessedEnergy);
            actionStrengthTitleText.StringReference.TableEntryReference = "Action_Strength";
            actionStrengthValueText.text = (hoveredHub.PossessedEnergy.Strength + hoveredHub.PossessedEnergy.BoosterStrength).ToString();
            actionDescriptionText.StringReference.TableEntryReference = "Action_Description";
        }
        else
        {
            gameObject.SetActive(true);
            tooltipBlock.SetActive(true);

            titleProperties.color = hoveredBlock.tooltipColor;

            blockSubtitleText.StringReference.TableEntryReference = newTooltip.ToString() + "_Subtitle";
            blockDescriptionText.StringReference.TableEntryReference = newTooltip.ToString() + "_Description";

            switch (newTooltip)
            {
                case ToolTips.Source:

                    break;
                case ToolTips.Straight:

                    break;
                case ToolTips.Corner:

                    break;
                case ToolTips.Fire:

                    break;
                case ToolTips.Ice:

                    break;
                case ToolTips.Electric:

                    break;
                case ToolTips.Heal:

                    break;
                case ToolTips.Distance:

                    break;
                case ToolTips.Speed:

                    break;
            }
        }     

        if (hoveredBlock != null)
        {
            currentTooltipSprite.sprite = hoveredBlock.sprite0;
        }  

        else if (hoveredHub != null)
            tooltipActionIcon.CopyActionVisual(hoveredHub);
    }

    //Returns the tooltip area to a blank state.
    public void ClearToolTip()
    {
        tooltipBlock.SetActive(false);
        tooltipAction.SetActive(false);
        gameObject.SetActive(false);
        currentTooltipSprite.sprite = null;
        tooltipActionIcon.ClearVisual();
    }

    public void SetTooltipActionColors(Energy hubEnergy)
    {
        //Set Conductor text color.
        switch (hubEnergy.Conductor)
        {    
            case Energy.Conductors.Attack:
                actionConductorTextMesh.color = Color.black;
                break;
            case Energy.Conductors.Heal:
                actionConductorTextMesh.color = new Color32(75, 183, 73, 255);
                break;
        }

        //Set Element text color.
        switch (hubEnergy.Element)
        {
            case Energy.Elements.Normal:
                actionElementTextMesh.color = Color.white;
                break;
            case Energy.Elements.Fire:
                actionElementTextMesh.color = new Color32(244, 120, 32, 255);
                break;
            case Energy.Elements.Ice:
                actionElementTextMesh.color = new Color32(32, 145, 244, 255);
                break;
            case Energy.Elements.Electric:
                actionElementTextMesh.color = new Color32(255, 255, 51, 255);
                break;
        }
    }
}
