using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityUtility
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField, OnValueChanged(nameof(OnValueChangedScene))]
        Object scene;
        
        [SerializeField, ReadOnly]
        string sceneName;

        public void Load()
        {
            SceneManager.LoadScene(sceneName);
        }

        void OnValueChangedScene()
        {
            if (scene != null) sceneName = scene.name;
        }
    }
}
