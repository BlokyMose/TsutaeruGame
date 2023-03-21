using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class EnemyController : MonoBehaviour
    {
        public float speed = 0;

        public float destroyTimer = 0f;

        private GameObject player;

        public GameObject destroyEnemy;

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
