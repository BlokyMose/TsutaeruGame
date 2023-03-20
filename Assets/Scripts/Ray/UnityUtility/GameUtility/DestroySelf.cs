using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public class DestroySelf : MonoBehaviour
    {
        [SerializeField]
        float delay = 0f;

        public void Init(float delay)
        {
            this.delay = delay;
        }

        public void Invoke()
        {
            Destroy(gameObject, delay);
        }
    }
}
