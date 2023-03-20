using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityUtility
{
    public static class GameplayUtilityClass
    {
        [Serializable]
        public abstract class AnimatorParameter
        {
            public enum DataType { Float, Int, Bool, Trigger }

            [SerializeField, HorizontalGroup("1")]
            protected string paramName;
            public string ParamName => paramName;

            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f)]
            protected DataType dataType;
            public DataType Type => dataType;

            public abstract int IntValue { get; }

            public abstract float FloatValue { get; }

            public abstract bool BoolValue { get; }

            protected int hash;

            public void Init()
            {
                hash = Animator.StringToHash(paramName);
            }


            public virtual void SetParam(Animator animator)
            {
                switch (dataType)
                {
                    case DataType.Float:
                        animator.SetFloat(hash, FloatValue);
                        break;
                    case DataType.Int:
                        animator.SetInteger(hash, IntValue);
                        break;
                    case DataType.Bool:
                        animator.SetBool(hash, BoolValue);
                        break;
                    case DataType.Trigger:
                        animator.SetTrigger(hash);
                        break;
                    default:
                        break;
                }
            }
        }


        [Serializable]
        public class AnimatorParameterStatic : AnimatorParameter
        {
            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f), SuffixLabel("value", true), ShowIf("@" + nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Int))]
            protected int intValue;
            public override int IntValue => intValue;

            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f), SuffixLabel("value", true), ShowIf("@" + nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Float))]
            protected float floatValue;
            public override float FloatValue => floatValue;

            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f), SuffixLabel("value", true), ShowIf("@" + nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Bool))]
            protected bool boolValue;
            public override bool BoolValue => boolValue;

            public AnimatorParameterStatic(string paramName, int intValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Int;
                this.intValue = intValue;
            }

            public AnimatorParameterStatic(string paramName, float floatValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Float;
                this.floatValue = floatValue;
            }

            public AnimatorParameterStatic(string paramName, bool boolValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Bool;
                this.boolValue = boolValue;
            }

            public AnimatorParameterStatic(string paramName)
            {
                this.paramName = paramName;
                this.dataType = DataType.Trigger;
            }

        }

        [Serializable]
        public class AnimatorParameterRandom : AnimatorParameter
        {
            public enum NumberRandomMode { Between, List }

            [SerializeField, LabelText("Mode"), PropertyOrder(-1)]
            protected NumberRandomMode numberRandomMode;

            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f),  
                ShowIf("@" +    nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Int) + " && " +
                                nameof(numberRandomMode) + "==" + nameof(NumberRandomMode) + "." + nameof(NumberRandomMode.List))]
            protected List<int> intRandomList;
            public List<int> IntRandomList => intRandomList;


            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f), 
                ShowIf("@" +    nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Int) + " && " +
                                nameof(numberRandomMode) + "==" + nameof(NumberRandomMode) + "." + nameof(NumberRandomMode.Between))]

            protected Vector2Int intRandomBetween;
            public Vector2Int IntRandomBetween => intRandomBetween;





            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f), 
                ShowIf("@" +    nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Float) + " && " +
                                nameof(numberRandomMode) + "==" + nameof(NumberRandomMode) + "." + nameof(NumberRandomMode.List))]
            protected List<float> floatRandomList;
            public List<float> FloatRandomList => floatRandomList;


            [SerializeField, HorizontalGroup("2"), LabelWidth(0.1f),
                ShowIf("@" +    nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Float) + " && " +
                                nameof(numberRandomMode) + "==" + nameof(NumberRandomMode) + "." + nameof(NumberRandomMode.Between))]

            protected Vector2 floatRandomBetween;
            public Vector2 FloatRandomBetween => floatRandomBetween;

            [SerializeField, HorizontalGroup("2"), LabelText("True:"), LabelWidth(25f), Range(0f,1f), ShowIf("@" + nameof(dataType) + "==" + nameof(DataType) + "." + nameof(DataType.Bool))]
            protected float boolTrueChance;
            public float BoolTrueChance => boolTrueChance;

            public override float FloatValue
            {
                get
                {
                    switch (numberRandomMode)
                    {
                        case NumberRandomMode.Between:
                            return Random.Range(floatRandomBetween.x, floatRandomBetween.y);
                        case NumberRandomMode.List:
                            var random = Random.Range(0, floatRandomList.Count);
                            return intRandomList[random];
                        default:
                            return 0f;
                    }
                }
            }

            public override int IntValue
            {
                get
                {
                    switch (numberRandomMode)
                    {
                        case NumberRandomMode.Between:
                            return Random.Range(intRandomBetween.x, intRandomBetween.y);
                        case NumberRandomMode.List:
                            var random = Random.Range(0, intRandomList.Count);
                            return intRandomList[random];
                        default:
                            return 0;
                    }
                }
            }

            public override bool BoolValue
            {
                get
                {
                    var random = Random.Range(0, 1f);
                    return random < boolTrueChance;
                }
            }

            public AnimatorParameterRandom(string paramName, List<int> intRandomValueList)
            {
                this.paramName = paramName;
                this.intRandomList = intRandomValueList;
            }

            public AnimatorParameterRandom(string paramName, float floatValue, List<float> floatRandomValueList)
            {
                this.paramName = paramName;
                this.floatRandomList = floatRandomValueList;
            }

            public AnimatorParameterRandom(string paramName, bool boolValue, float boolTrueChance)
            {
                this.paramName = paramName;
                this.boolTrueChance = boolTrueChance;
            }
        }

    }
}
