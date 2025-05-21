using Assets.Scripts;
using UnityEngine;

public class DoorInteractable : SimpleHindgeInteractable
{

    [SerializeField] private EventController eventController;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 rotationLimits;
    private Transform startRotation;
    private float startAngleX;

    protected override void Start()
    {
        base.Start();
        startRotation = transform;
        startAngleX = startRotation.localEulerAngles.x;
        if (startAngleX >= 180)
        {
            startAngleX -= 360;
        }
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        eventController.DoorUnlockAction += OnDoorUnlock;
        eventController.DoorLockAction += OnDoorLock;
    }

    protected override void Update()
    {
        base.Update();
        doorTransform.localEulerAngles = new Vector3(doorTransform.localEulerAngles.x,
            transform.localEulerAngles.y,
            doorTransform.localEulerAngles.z);

        if (isSelected)
        {
            LimitCheck();
        }
    }

    private void LimitCheck()
    {
        float localAngleX = transform.localEulerAngles.x;

        if (localAngleX >= 180)
        {
            localAngleX -= 360;
        }

        if (localAngleX >= startAngleX + rotationLimits.x || localAngleX <= startAngleX - rotationLimits.x)
        {
            ReleaseHinge();
            transform.localEulerAngles = new Vector3(startAngleX, transform.localEulerAngles.y,transform.localEulerAngles.z);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventController.DoorUnlockAction -= OnDoorUnlock;
        eventController.DoorLockAction -= OnDoorLock;
    }

}
