using Encore.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoSwitchSprite : AutoInvoke
    {
        [SerializeField]
        List<Sprite> sprites = new List<Sprite>();

        SpriteRenderer sr;

        protected override void Awake()
        {
            base.Awake();
            sr = GetComponent<SpriteRenderer>();
        }

        protected override IEnumerator Invoking()
        {
            yield return base.Invoking();
            sr.sprite = sprites.GetRandom();
        }
    }
}
