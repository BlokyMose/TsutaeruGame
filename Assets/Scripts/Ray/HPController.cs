using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityUtility;

namespace Tsutaeru
{
    [RequireComponent(typeof(Animator))]
    public class HPController : MonoBehaviour
    {
        [SerializeField]
        Sprite fullHeartSprite;

        [SerializeField]
        Sprite emptyHeartSprite;

        [SerializeField]
        Image heartPrefab;

        [SerializeField]
        Transform heartsParent;

        [SerializeField]
        int heartCount = 3;

        [SerializeField]
        UnityEvent onDie;

        Animator animator;
        List<Image> hearts = new();
        int currentHP;
        int boo_show;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            boo_show = Animator.StringToHash(nameof(boo_show));
        }

        void Start()
        {
            currentHP = heartCount;
            heartsParent.DestroyChildren();
            hearts.Clear();
            for (int i = 0; i < heartCount; i++)
            {
                var heart = Instantiate(heartPrefab, heartsParent);
                heart.sprite = fullHeartSprite;
                hearts.Add(heart);
            }
        }

        public void Show()
        {
            animator.SetBool(boo_show, true);
        }

        public void Hide()
        {
            animator.SetBool(boo_show, false);
        }

        public void ReduceHealth()
        {
            currentHP--;
            hearts[currentHP].sprite = emptyHeartSprite;

            if (currentHP == 0)
            {
                onDie.Invoke();
            }
        }

        public void RestoreAllHealth()
        {
            foreach (var heart in hearts)
                heart.sprite = fullHeartSprite;

            currentHP = heartCount;
        }
    }

}