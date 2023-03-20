using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtility
{
    public class AnimationEventBroadcaster : MonoBehaviour
    {
        [Serializable]
        public class RelayEvent
        {
            public string eventName;
            public UnityEvent onInvoked;
        }

        [SerializeField]
        List<RelayEvent> events = new();

        public void InvokeEvent(string eventName)
        {
            foreach (var relayEvent in events)
            {
                if (relayEvent.eventName == eventName)
                {
                    relayEvent.onInvoked.Invoke();
                    break;
                }
            }
        }
    }
}
