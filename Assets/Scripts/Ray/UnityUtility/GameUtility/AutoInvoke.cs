using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtility
{
    public class AutoInvoke : MonoBehaviour
    {
        [SerializeField]
        UnityInitialMethod invokeIn = UnityInitialMethod.Awake;

        [SerializeField]
        float delay = 0;

        [SerializeField]
        UnityEvent onInvoke;


        protected virtual void Awake()
        {
            if (invokeIn == UnityInitialMethod.Awake)
                Invoke();
        } 

        protected virtual void Start()
        {
            if (invokeIn == UnityInitialMethod.Start)
                Invoke();
        }

        protected virtual void OnEnable()
        {
            if (invokeIn == UnityInitialMethod.OnEnable)
                Invoke();
        }

        void Invoke()
        {
            StartCoroutine(Invoking());

        }

        protected virtual IEnumerator Invoking()
        {
            yield return new WaitForSeconds(delay);
            onInvoke.Invoke();
        }
    }
}
