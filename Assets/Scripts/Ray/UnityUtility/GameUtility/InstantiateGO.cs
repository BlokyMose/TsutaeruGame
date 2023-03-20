using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityUtility
{
    public class InstantiateGO : MonoBehaviour
    {
        [SerializeField]
        GameObject prefab;

        [HorizontalGroup("scale", 10), SerializeField, LabelWidth(0.1f)]
        bool isOverrideScale = false;

        [HorizontalGroup("scale"), SerializeField, EnableIf(nameof(isOverrideScale))]
        Vector2 scaleOverride = Vector2.one;

        [HorizontalGroup("rotation", 10), SerializeField, LabelWidth(0.1f)]
        bool isOverrideRotation = false;

        [HorizontalGroup("rotation"), SerializeField, EnableIf(nameof(isOverrideRotation))]
        Vector3 rotationOverride = Vector3.zero;

        public void Invoke()
        {
            var go = Instantiate(prefab, null);
            go.SetActive(true);
            go.transform.position = transform.position;

            if (isOverrideRotation)
                go.transform.localEulerAngles = rotationOverride;
            else
                go.transform.localEulerAngles = transform.localEulerAngles;

            if (isOverrideScale) 
                go.transform.localScale = scaleOverride;

        }
    }
}
