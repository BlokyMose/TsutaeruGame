using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    [RequireComponent(typeof(Animator))]
    public class CharactersPanel : MonoBehaviour
    {
        Animator animator;
        int int_mode;

        public void Init()
        {
            animator = GetComponent<Animator>();
            int_mode = Animator.StringToHash(nameof(int_mode));
        }

        public void Enlarge()
        {
            animator.SetInteger(int_mode, (int)GameState.OutGame);
        }

        public void Shrink()
        {
            animator.SetInteger(int_mode, (int)GameState.InGame);
        }
    }
}
