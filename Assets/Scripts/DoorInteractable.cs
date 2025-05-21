using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class DoorInteractable : SimpleHindgeInteractable
{
    [SerializeField] private Transform dooTransform;

    protected override void Update()
    {
        base.Update();
        dooTransform.localEulerAngles = new Vector3(dooTransform.localEulerAngles.x,
            transform.localEulerAngles.y,
            dooTransform.localEulerAngles.z);
    }


}
