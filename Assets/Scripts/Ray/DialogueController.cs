using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Tsutaeru.GameController;

namespace Tsutaeru
{
    public class DialogueController : MonoBehaviour
    {
        [Header("Boy")]
        [SerializeField]
        Animator bubbleBoy;

        [SerializeField]
        TextMeshProUGUI textBoy;

        [SerializeField]
        Image boyImage;

        [Header("Girl")]
        [SerializeField]
        Animator bubbleGirl;

        [SerializeField]
        TextMeshProUGUI textGirl;

        [SerializeField]
        Image girlImage;

        int boo_show;

        public void Init()
        {
            boo_show = Animator.StringToHash(nameof(boo_show));
        }

        public void SetDialogueToBubbleBoy(string text, Sprite sprite)
        {
            textBoy.text = text;
            SetSpriteBoy(sprite);
            bubbleBoy.SetBool(boo_show, true);
            bubbleGirl.SetBool(boo_show, false);
        }        
        
        public void SetDialogueToBubbleGirl(string text, Sprite sprite)
        {
            textGirl.text = text;
            SetSpriteGirl(sprite);
            bubbleGirl.SetBool(boo_show, true);
            bubbleBoy.SetBool(boo_show, false);
        }

        public void SetSpriteBoy(Sprite sprite)
        {
            if (sprite != null)
                boyImage.sprite = sprite;
        }

        public void SetSpriteGirl(Sprite sprite)
        {
            if (sprite != null)
                girlImage.sprite = sprite;
        }

        public void HideAll()
        {
            bubbleBoy.SetBool(boo_show, false);
            bubbleGirl.SetBool(boo_show, false);
        }
    }
}
