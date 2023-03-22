using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{
    public class HiraganaController : MonoBehaviour
    {
        public float speed = 0;

        public float lifeDuration = 5f;

        private GameObject player;

        public TextMeshProUGUI text;

        [SerializeField]
        List<string> hiraganas = new();

        public string GetHiragana => text.text;
        bool hasHiraganaSet = false;

        void Start()
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
            if (!hasHiraganaSet)
                text.text = hiraganas.GetRandom();
        }

        public void SetHiragana(string hiragana)
        {
            text.text = hiragana;
            hasHiraganaSet = true;
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Destroy(gameObject, 5f);
        }
    }
}
