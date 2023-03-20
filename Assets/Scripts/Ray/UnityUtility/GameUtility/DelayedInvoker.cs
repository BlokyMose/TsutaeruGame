using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tsutaeru
{
    public class DelayedInvoker : MonoBehaviour
    {
        [System.Serializable]
        public class DelayedEvent
        {
            [SerializeField]
            string eventName;

            [SerializeField]
            float delay;

            [SerializeField]
            UnityEvent onInvoke;

            public string EventName { get => eventName; }
            public float Delay { get => delay; }
            public UnityEvent OnInvoke { get => onInvoke; }

            public IEnumerator Invoke()
            {
                yield return new WaitForSeconds(Delay);
                OnInvoke.Invoke();
            }
        }

        [SerializeField]
        List<DelayedEvent> events = new();

        public void Invoke(string eventName)
        {
            var foundEvent = events.Find(e => e.EventName == eventName);
            if (foundEvent != null)
                StartCoroutine(foundEvent.Invoke());
        }

        public void InvokeAll()
        {
            foreach (var delayedEvent in events)
                StartCoroutine(delayedEvent.Invoke());
        }
    }
}
