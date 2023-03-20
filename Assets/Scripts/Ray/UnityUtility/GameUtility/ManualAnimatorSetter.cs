using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
    public class ManualAnimatorSetter : MonoBehaviour
    {
        [SerializeField]
        List<GameplayUtilityClass.AnimatorParameterManual> parameterList = new ();

        [SerializeField]
        Animator animator;

        private void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            foreach (var param in parameterList)
                param.Init();
        }

        public void SetParam(string setterName)
        {
            foreach (var param in parameterList)
                if (param.SetterName == setterName)
                    param.SetParam(animator);
        }

        public void SetAllParams()
        {
            foreach (var param in parameterList)
                param.SetParam(animator);
        }
    }
}
