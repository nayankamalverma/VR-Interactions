using Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleHindgeInteractable : XRSimpleInteractable
{

    [SerializeField] private EventController eventController;
    private Transform grabHand;

    private bool isLocked;

    private void OnEnable()
    {
        isLocked = false;
        eventController.OnDoorUnlock += OnDoorUnlock;
        eventController.OnDoorLock += OnDoorLock;
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
        base.OnSelectEntered(args);
        grabHand = args.interactorObject.transform;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabHand = null;
    }

    private void OnDoorUnlock()
    {
        isLocked = false;
    }
    private void OnDoorLock()
    {
        isLocked = true;
    }

    private void OnDisable()
    {
        eventController.OnDoorUnlock -= OnDoorUnlock;
        eventController.OnDoorLock -= OnDoorLock;
    }
}
