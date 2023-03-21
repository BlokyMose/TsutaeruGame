using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class EnemyController : MonoBehaviour
    {
        public float speed = 0;

        public float destroyTimer = 0f;

        public GameObject target;

        public GameObject destroyEnemy;

        void Start()
        {
            
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            Destroy(destroyEnemy, destroyTimer);
        }
    }
}
