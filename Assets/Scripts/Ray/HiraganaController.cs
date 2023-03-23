using Encore.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{
    [RequireComponent(typeof(Animator))]
    public class HiraganaController : MonoBehaviour
    {
        public float speed = 0;

        public float lifeDuration = 5f;

        private GameObject player;

        public TextMeshProUGUI text;

        public Action OnDie;

        [SerializeField]
        List<string> hiraganas = new();

        Animator animator;
        int boo_show;

        public string GetHiragana => text.text;
        bool hasHiraganaSet = false;
        const float DESTROY_ANIMATION_DURATION = 0.6f;

        void Start()
        {
            animator = GetComponent<Animator>();
            boo_show = Animator.StringToHash(nameof(boo_show));

            player = GameObject.FindGameObjectsWithTag("Player")[0];
            if (!hasHiraganaSet)
                text.text = hiraganas.GetRandom();


            StartCoroutine(Delay(lifeDuration));
            IEnumerator Delay(float delay)
            {
                yield return new WaitForSeconds(delay - DESTROY_ANIMATION_DURATION);
                animator.SetBool(boo_show, false);
                yield return new WaitForSeconds(DESTROY_ANIMATION_DURATION);
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            OnDie?.Invoke();
        }

        public void SetHiragana(string hiragana)
        {
            text.text = hiragana;
            hasHiraganaSet = true;
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
