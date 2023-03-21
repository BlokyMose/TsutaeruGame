using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{

    [RequireComponent(typeof(Canvas))]
    public class BGCanvas : MonoBehaviour
    {
        public static BGCanvas Instance;

        void Awake()
        {
            if (BGCanvas.Instance == null)
            {
                BGCanvas.Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnLoadScene()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }

        private void OnLevelWasLoaded(int level)
        {
            OnLoadScene();
        }

    }

}