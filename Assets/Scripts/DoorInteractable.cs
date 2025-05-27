using Assets.Scripts;
using UnityEngine;

public class DoorInteractable : SimpleHindgeInteractable
{
    [SerializeField] private EventController eventController;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 rotationLimits;
    [SerializeField] private Vector3 endRotation;
    [SerializeField] private Collider doorCloseCollider;
    [SerializeField] private Collider doorOpenCollider;

    private bool isClosed;
    private bool isOpen;
    private Vector3 startRotation;
    private float startAngleX;

    protected override void Start()
    {
        base.Start();
        isClosed = true;
        isOpen = false;
        startRotation = transform.localEulerAngles;
        startAngleX = startRotation.x;
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
        isClosed = false;
        isOpen = false;
        float localAngleX = transform.localEulerAngles.x;

        if (localAngleX >= 180)
        {
            localAngleX -= 360;
        }

        if (localAngleX >= startAngleX + rotationLimits.x || localAngleX <= startAngleX - rotationLimits.x)
        {
            ReleaseHinge();
        }
    }
    
    protected override void ResetHinge()
    {
        if (isClosed)
        {
            transform.localEulerAngles = startRotation;
        }
        else if (isOpen) {
            transform.localEulerAngles = endRotation;
        }
        {
            transform.localEulerAngles = new Vector3(startAngleX, transform.localEulerAngles.y,transform.localEulerAngles.z);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventController.DoorUnlockAction -= OnDoorUnlock;
        eventController.DoorLockAction -= OnDoorLock;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == doorCloseCollider)
        {
            isClosed = true;
            ReleaseHinge();
        }
        else if (other == doorOpenCollider)
        {
            isOpen = true;
            ReleaseHinge();
        }
    }
}
