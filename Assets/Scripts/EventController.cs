using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class EventController : MonoBehaviour
    {
        public UnityAction OnDoorUnlock;
        public UnityAction OnDoorLock;
    }
}