using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tsutaeru
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField]
        Animator bubbleBoy;

        [SerializeField]
        TextMeshProUGUI textBoy;

        [SerializeField]
        Animator bubbleGirl;

        [SerializeField]
        TextMeshProUGUI textGirl;

        int boo_show;

        private void Awake()
        {
            boo_show = Animator.StringToHash(nameof(boo_show));
        }

        public void SetTextToBubbleBoy(string text)
        {
            textBoy.text = text;
            bubbleBoy.SetBool(boo_show, true);
            bubbleGirl.SetBool(boo_show, false);
        }        
        
        public void SetTextToBubbleGirl(string text)
        {
            textGirl.text = text;
            bubbleGirl.SetBool(boo_show, true);
            bubbleBoy.SetBool(boo_show, false);
        }

        public void HideAll()
        {
            bubbleBoy.SetBool(boo_show, false);
            bubbleGirl.SetBool(boo_show, false);
        }
    }
}
