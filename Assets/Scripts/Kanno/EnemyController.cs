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

        public float destroyTimer = 0f;

        private GameObject player;

        public GameObject destroyEnemy;

        public TextMeshProUGUI text;

        public string GetHiragana => text.text;


        void Start()
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            Destroy(destroyEnemy, destroyTimer);
        }

    }
}
