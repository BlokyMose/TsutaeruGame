using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BGCanvas : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
