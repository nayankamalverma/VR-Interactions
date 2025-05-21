using System;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class CombinationPanelController : MonoBehaviour
{
    [SerializeField] private EventController eventController;
    [SerializeField] private TextMeshProUGUI userInputText;
    [SerializeField] private XRButtonInteractable[] comboButtons;
    [SerializeField] private XRButtonInteractable lockButton;
    [SerializeField] private XRButtonInteractable resetCodeButton;
    [SerializeField] private Image lockedPanel;
    [SerializeField] private TextMeshProUGUI lockedText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private int[] comboValue = new int[3];

    private bool isLocked;
    private int[] inputValues;
    private int maxButtonPress;
    private int butttonPress;
    private const string unlockedText = "Unlocked";
    private const string uiLayerMask = "UI";
    private const string nothingLayerMask = "Nothing";
    //heading text
    private const string startingText = "Enter 3 digits";
    private const string resetText = "Enter new 3 digits code";

    private void Start()
    {
        isLocked = true;
        infoText.text = startingText;
        maxButtonPress = comboValue.Length;
        inputValues = new int[maxButtonPress];
        ResetInput();
        AddListeners();
    }

    private void AddListeners()
    {
        foreach (var button in comboButtons)
        {
            button.selectEntered.AddListener(OnNumericButtonClick);
        }
        resetCodeButton.selectEntered.AddListener(OnResetButtonClick);
        lockButton.selectEntered.AddListener(OnLockButtonClicked);
    }

    private void OnResetButtonClick(SelectEnterEventArgs arg0)
    {
        if (!isLocked)
        {
            infoText.text = resetText;
            DisableLockAndResetButton();
            UnableNumericalInput();
            ResetInput();
        }
    }
    private void OnLockButtonClicked(SelectEnterEventArgs arg0)
    {
        if (!isLocked)
        {
            isLocked = true;
            eventController.LockDoor();
            lockedPanel.color = Color.red;
            infoText.text = startingText;
            userInputText.text = "000";
            DisableLockAndResetButton();
            UnableNumericalInput();
            ResetInput();
        }
    }

    private void OnNumericButtonClick(SelectEnterEventArgs arg0)
    {
        if (butttonPress >= maxButtonPress)
        {
            ResetInput();
        }
        else
        {
            for (int i = 0; i < comboButtons.Length; i++)
            {
                if (arg0.interactableObject.transform.name == comboButtons[i].transform.name)
                {
                    userInputText.text += i.ToString();
                    inputValues[butttonPress] = i;
                }
                else
                {
                    comboButtons[i].ResetColor();
                }
            }
            butttonPress++;
            if (!isLocked && butttonPress == maxButtonPress)
            {
                for (int i = 0; i < maxButtonPress; i++)
                {
                    comboValue[i] = inputValues[i];
                }
                isLocked = true;
                lockedPanel.color = Color.red;
                infoText.text = startingText;
                Invoke( "ResetInput" ,1.5f);

            }
            //comparing input and code 
            else if (butttonPress == maxButtonPress)
            {
                if (CheckCode()) RightInput();
                else ResetInput();
            }
        }
    }

    private bool CheckCode()
    {
        for (int i = 0; i < maxButtonPress; i++)
        {
            if (inputValues[i] != comboValue[i]) return false;
        }
        // if right input 
        return true;
    }

    private void RightInput()
    {
        isLocked = false;
        eventController.UnlockDoor();
        lockedPanel.color = Color.green;
        lockedText.text = unlockedText;
        DisableNumericalInput();
        UnableLockAndResetButton();
    }

    private void ResetInput()
    {
        userInputText.text = "";
        Array.Clear(inputValues, 0, maxButtonPress);
        butttonPress = 0;
    }

    private void UnableLockAndResetButton()
    {
        lockButton.interactionLayers = InteractionLayerMask.GetMask(uiLayerMask);
        resetCodeButton.interactionLayers = InteractionLayerMask.GetMask(uiLayerMask);
    }
    
    private void DisableLockAndResetButton()
    {
        lockButton.interactionLayers = InteractionLayerMask.GetMask(nothingLayerMask);
        resetCodeButton.interactionLayers = InteractionLayerMask.GetMask(nothingLayerMask);
    }

    private void UnableNumericalInput()
    {
        foreach (XRButtonInteractable b in comboButtons)
        {
           b.interactionLayers = InteractionLayerMask.GetMask(uiLayerMask);
        }
    }

    private void DisableNumericalInput()
    {
        foreach (XRButtonInteractable b in comboButtons)
        {
            b.interactionLayers = InteractionLayerMask.GetMask(nothingLayerMask);
        }
    }

}
