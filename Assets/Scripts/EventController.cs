using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EventController : MonoBehaviour
    {
        public UnityAction DoorUnlockAction;
        public UnityAction DoorLockAction;

        public void UnlockDoor() => DoorUnlockAction?.Invoke();
        public void LockDoor() => DoorLockAction?.Invoke();
    }
}