using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tsutaeru
{
    public class HiraganaSpawner : MonoBehaviour
    {
        [SerializeField] HiraganaController hiraganaPrefab; 
        [SerializeField] Transform posTopLeft;                 
        [SerializeField] Transform posBottomRight;                

        float time = 0f;
        [SerializeField] 
        float spawnEvery = 1f;

        public bool IsPaused = false;

        public Action OnSpawn;


        void Update()
        {
            if (IsPaused) return;

            time += Time.deltaTime;

            if (time > spawnEvery)
            {
                time = 0;
                Spawn();
            }
        }

        public void Spawn(string hiraganaText = "")
        {
            float posX, posY;
            bool isSpawnOnXAxis = Random.Range(0, 2) == 0;
            if (isSpawnOnXAxis)
            {
                bool isSpawnOnTop = Random.Range(0, 2) == 0;
                posY = isSpawnOnTop ? posTopLeft.position.y : posBottomRight.position.y;
                posX = Random.Range(posTopLeft.position.x, posBottomRight.position.x);
            }
            else
            {
                bool isSpawnOnLeft = Random.Range(0, 2) == 0;
                posX = isSpawnOnLeft ? posTopLeft.position.x : posBottomRight.position.x;
                posY = Random.Range(posTopLeft.position.y, posBottomRight.position.y);

            }

            var hiragana = Instantiate(hiraganaPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
            hiragana.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(hiraganaText))
                hiragana.SetHiragana(hiraganaText);

            OnSpawn?.Invoke();
        }
    }
}
