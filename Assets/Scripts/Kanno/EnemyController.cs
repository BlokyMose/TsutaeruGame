using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class EnemyController : MonoBehaviour
    {
        public float speed = 0;

        public float lifeDuration = 5f;

        private GameObject player;

        public GameObject destroyEnemy;

        public TextMeshProUGUI text;

        [SerializeField]
        List<string> hiraganas = new();

        public string GetHiragana => text.text;


        void Start()
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
            text.text = hiraganas.GetRandom();
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Destroy(gameObject, 5f);
        }

    }
}
