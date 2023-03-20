using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityUtility
{
    [RequireComponent(typeof(Animator))]
    public class AutoAnimatorParamSetter : AutoInvoke
    {
        [SerializeField]
        List<GameplayUtilityClass.AnimatorParameterStatic> parameters = new List<GameplayUtilityClass.AnimatorParameterStatic>();

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
