using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    [CreateAssetMenu(fileName ="CharSL_", menuName ="SO/Character Sprite Library")]

    public class CharacterSpriteLib : ScriptableObject
    {
        [SerializeField]
        Sprite normal;

        [SerializeField]
        Sprite happy;
        
        [SerializeField]
        Sprite sad;

        [SerializeField]
        Sprite nervous;

        [SerializeField]
        Sprite nervousMuch;

        [SerializeField]
        Sprite happyNervous;

        public Sprite Normal { get => normal; }
        public Sprite Happy { get => happy; }
        public Sprite Sad { get => sad; }
        public Sprite Nervous { get => nervous; }
        public Sprite NervousMuch { get => nervousMuch; }
        public Sprite HappyNervous { get => happyNervous; }

        public Sprite Get(Emotion emotion)
        {
            return emotion switch
            {
                Emotion.Normal => normal,
                Emotion.Happy => happy,
                Emotion.Sad => sad,
                Emotion.Nervous => nervous,
                Emotion.NervousMuch => nervousMuch,
                Emotion.HappyNervous => happyNervous,
                _ => normal
            };
        }
    }
}
