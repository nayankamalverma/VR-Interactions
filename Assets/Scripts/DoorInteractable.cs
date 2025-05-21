using Assets.Scripts;
using UnityEngine;

public class DoorInteractable : SimpleHindgeInteractable
{

    [SerializeField] private EventController eventController;
    [SerializeField] private Transform dooTransform;


    protected override void OnEnable()
    {
        base.OnEnable();
        eventController.DoorUnlockAction += OnDoorUnlock;
        eventController.DoorLockAction += OnDoorLock;
    }

    protected override void Update()
    {
        base.Update();
        dooTransform.localEulerAngles = new Vector3(dooTransform.localEulerAngles.x,
            transform.localEulerAngles.y,
            dooTransform.localEulerAngles.z);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventController.DoorUnlockAction -= OnDoorUnlock;
        eventController.DoorLockAction -= OnDoorLock;
    }

}
