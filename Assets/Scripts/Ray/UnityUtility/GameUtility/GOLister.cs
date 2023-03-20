using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public class GOLister : MonoBehaviour
    {
        [Serializable]
        public class Pair
        {
            [SerializeField]
            string name;
            [SerializeField]
            GameObject go;

            public string Name { get => name; }
            public GameObject GO { get => go;  }
        }

        [SerializeField]
        List<Pair> list = new();

        public List<Pair> List { get => list;  }

        public bool TryGet(string name, out GameObject foundGO)
        {
            foreach (var pair in list)
                if (pair.Name == name)
                {
                    foundGO = pair.GO;
                    return true;
                }

            foundGO = null;
            return false;
        }

        public GameObject Get(string name)
        {
            foreach (var pair in list)
                if (pair.Name == name)
                    return pair.GO;

            return null;
        }

    }
}
