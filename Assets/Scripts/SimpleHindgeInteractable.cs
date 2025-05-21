using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleHindgeInteractable : XRSimpleInteractable
{

    private Transform grabHand;
    private bool isLocked;

    protected override void OnEnable()
    {
        base.OnEnable();
        isLocked = true;
    }

    protected virtual void Update()
    {
        if (grabHand != null)
        {
            transform.LookAt(grabHand, transform.forward);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(!isLocked)
        {
            base.OnSelectEntered(args);
            grabHand = args.interactorObject.transform;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabHand = null;
    }


    protected void OnDoorUnlock()
    {
        isLocked = false;
    }
    protected void OnDoorLock()
    {
        isLocked = true;
    }


}
