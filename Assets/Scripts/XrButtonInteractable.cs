using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class XRButtonInteractable : XRSimpleInteractable
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color[] buttonColors;

    private Color normalColor;
    private Color highlightedColor;
    private Color pressedColor;
    private Color selectedColor;
    private bool isPressed;

    private void Start()
    {
        normalColor = buttonColors[0];
        highlightedColor = buttonColors[1];
        pressedColor = buttonColors[2];
        selectedColor = buttonColors[3];

        buttonImage.color = normalColor;
        isPressed = false;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        isPressed = false;
        buttonImage.color = highlightedColor;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if(!isPressed) buttonImage.color = normalColor;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isPressed = true;
        buttonImage.color = pressedColor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        buttonImage.color = selectedColor;
    }
}
