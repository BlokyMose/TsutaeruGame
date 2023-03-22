using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{

    [RequireComponent(typeof(Animator))]
    public class WordPanel : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI text;

        Animator animator;
        int boo_show;

        public void Init()
        {
            animator = GetComponent<Animator>();
            boo_show = Animator.StringToHash(nameof(boo_show));
        }

        public void SetText(string text)
        {
            animator.SetBool(boo_show, true);
            this.text.text = text;
        }

        public void SetTextColored(string text, int length)
        {
            animator.SetBool(boo_show, true);
            if (length > text.Length) length = text.Length;
            this.text.text = $"<color=#E8574A>{text[0..length]}</color>{text[length..]}";
        }

        public void Hide()
        {
            animator.SetBool(boo_show, false);
        }
    }

}