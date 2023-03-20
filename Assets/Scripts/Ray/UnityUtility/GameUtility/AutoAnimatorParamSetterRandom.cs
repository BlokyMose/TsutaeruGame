using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public class AutoAnimatorParamSetterRandom : AutoInvoke
    {
        [SerializeField]
        List<GameplayUtilityClass.AnimatorParameterRandom> parameters = new List<GameplayUtilityClass.AnimatorParameterRandom>();

        Animator animator;

        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            foreach (var param in parameters)
                param.Init();

            base.Awake();
        }

        protected override IEnumerator Invoking()
        {
            yield return base.Invoking();

            foreach (var param in parameters)
                param.SetParam(animator);
        }
    }
}
