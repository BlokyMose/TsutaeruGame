using UnityUtility;
using System.Collections;
using UnityEngine;

namespace UnityUtility
{
    public static class ComponentUtility
    {
        public static T GetComponentInFamily<T>(this Component thisComponent) where T : Component
        {
            var targetComponent = thisComponent.GetComponent<T>();
            targetComponent ??= thisComponent.GetComponentInParent<T>();
            targetComponent ??= thisComponent.GetComponentInChildren<T>();

            return targetComponent;
        }

        public static bool TryGetComponentInFamily<T>(this Component thisComponent, out T targetComponent) where T : Component
        {
            targetComponent = thisComponent.GetComponentInFamily<T>();
            return targetComponent != null;
        }

        public static T GetComponentInFamily<T>(this GameObject thisComponent) where T : Component
        {
            var targetComponent = thisComponent.GetComponent<T>();
            targetComponent ??= thisComponent.GetComponentInParent<T>();
            targetComponent ??= thisComponent.GetComponentInChildren<T>();

            return targetComponent;
        }

        public static GameObject Find(string goName)
        {
            var allGOs = GameObject.FindObjectsOfType<GameObject>(true);
            foreach (var go in allGOs)
                if (go.name == goName)
                    return go;

            return null;
        }

        public static bool TryInstantiate<T>(this Object context, T original, out T instantiatedObject, Transform parent = null) where T : Object
        {
            if (original != null) 
            {
                instantiatedObject = Object.Instantiate(original,parent); ;
                return true;
            }
            else
            {
                instantiatedObject=null;
                return false;
            }
        }

        public static void DestroyChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                 Object.Destroy(parent.GetChild(i).gameObject);
        }

    }
}