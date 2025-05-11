using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : XRGrabInteractable
{
    [SerializeField] private Transform drawerTransform;
    [SerializeField] private XRSocketInteractor keySocket;
    [SerializeField] private bool isLocked;
    [SerializeField] private float drawerLimitZ = .9f;
    [SerializeField] private Vector3 limitDistance = new Vector3(.02f, .02f, 0);

    private Vector3 limitPos;
    private Transform parentTransform;
    private bool isGrabbed;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";
    

    private void Start()
    {
        isGrabbed = false;
        isLocked = true;
        if (keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }

        parentTransform = transform.parent.transform;
        limitPos = drawerTransform.localPosition;
    }

    private void OnDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log("Drawer locked ...");
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs arg0)
    {
        isLocked = false;
        Debug.Log("***Drawer unlocked ...");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isLocked)
        {
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }
        else
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        isGrabbed = false;
        transform.localPosition = drawerTransform.localPosition;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x,
                drawerTransform.localPosition.y, transform.localPosition.z);
            CheckLimits();
        }
    }

    private void CheckLimits()
    {
        //limit in x coordinate
        if (transform.localPosition.x >= limitPos.x + limitDistance.x 
            || transform.localPosition.x <= limitPos.x - limitDistance.x 
            || transform.localPosition.y >= limitPos.y + limitDistance.y 
            || transform.localPosition.y <= limitPos.y - limitDistance.y)
        {
            ChangeLayerMask(defaultLayer);
        }
        //limit in z coordinate
        else if (transform.localPosition.z <= limitPos.z - limitDistance.z)
        {
            isGrabbed = false;
            drawerTransform.localPosition = limitPos;
            ChangeLayerMask(defaultLayer);
        } else if (transform.localPosition.z >= drawerLimitZ)
        {
            isGrabbed = false;
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x, drawerTransform.localPosition.y, drawerLimitZ);
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string layerMask)
    {
        interactionLayers = InteractionLayerMask.GetMask(layerMask);
    }
}
