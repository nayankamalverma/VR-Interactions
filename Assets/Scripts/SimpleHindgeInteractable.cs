using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class SimpleHindgeInteractable : XRSimpleInteractable
{
    [SerializeField] private Vector3 positionLimits;

    private Collider hingeCollider;
    private Vector3 hingePostion;
    private Rigidbody rb;
    private Transform grabHand;
    private bool isLocked;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";

    protected virtual void Start()
    {
        hingeCollider = GetComponent<Collider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isLocked = true;
    }

    protected virtual void Update()
    {
        if (grabHand != null)
        {
            TrackHand();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("enter");
        if (!isLocked)
        {
            base.OnSelectEntered(args);
            grabHand = args.interactorObject.transform;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabHand = null;
        ChangeLayerMask(grabLayer);
        ResetHinge();

    }

    private void TrackHand()
    {
        transform.LookAt(grabHand, transform.forward);
        hingePostion = hingeCollider.bounds.center;
        if (grabHand.position.x >= hingePostion.x + positionLimits.x ||
            grabHand.position.x <= hingePostion.x - positionLimits.x ||
            grabHand.position.y >= hingePostion.y + positionLimits.y ||
            grabHand.position.y <= hingePostion.y - positionLimits.y ||
            grabHand.position.z >= hingePostion.z + positionLimits.z ||
            grabHand.position.z <= hingePostion.z - positionLimits.z)
        {
            ReleaseHinge();
        }
    }

    protected void ReleaseHinge()
    {
        ChangeLayerMask(defaultLayer);
    }

    protected abstract void ResetHinge();

    protected void OnDoorUnlock()
    {
        isLocked = false;
    }
    protected void OnDoorLock()
    {
        isLocked = true;
    }
    private void ChangeLayerMask(string layerMask)
    {
        interactionLayers = InteractionLayerMask.GetMask(layerMask);
    }
}
