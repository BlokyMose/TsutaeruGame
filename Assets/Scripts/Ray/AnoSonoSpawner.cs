using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{
    public class AnoSonoSpawner : MonoBehaviour
    {
        [SerializeField]
        GameObject anoSonoPrefab;

        [SerializeField]
        List<string> texts = new();

        [SerializeField]
        float spawnEvery = 1f;

        [SerializeField]
        float killAfter = 3;

        bool isPaused = true;

        public void Init(bool isPaused = true)
        {
            this.isPaused = isPaused;
            StartCoroutine(Update());
            IEnumerator Update()
            {
                var time = 0f;
                while (true)
                {
                    if (!this.isPaused && time > spawnEvery)
                    {
                        time = 0f;
                        Spawn();
                    }

                    time += Time.deltaTime * Time.timeScale;
                    yield return null;
                }
            }
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Spawn()
        {
            var go = Instantiate(anoSonoPrefab, transform);
            go.transform.localPosition = Vector2.zero;
            go.GetComponentInChildren<TextMeshProUGUI>().text = texts.GetRandom();
            Destroy(go, killAfter);
        }
    }
}
