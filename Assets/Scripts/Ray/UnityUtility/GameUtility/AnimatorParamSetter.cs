using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UnityUtility
{
    public class AnimatorParamSetter : MonoBehaviour
    {
        [Serializable]
        public class AnimatorParameterEvent : GameplayUtilityClass.AnimatorParameterStatic
        {
            [SerializeField, HorizontalGroup("1"), LabelWidth(0.1f)]
            EventTriggerType triggerType;
            public EventTriggerType TriggerType => triggerType;

            public AnimatorParameterEvent(EventTriggerType triggerType, string paramName, int intValue) : base(paramName, intValue)
            {
                this.triggerType = triggerType;
            }

            public AnimatorParameterEvent(EventTriggerType triggerType, string paramName, float floatValue) : base(paramName, floatValue)
            {
                this.triggerType = triggerType;
            }

            public AnimatorParameterEvent(EventTriggerType triggerType, string paramName, bool boolValue) : base(paramName, boolValue)
            {
                this.triggerType = triggerType;
            }

            public AnimatorParameterEvent(EventTriggerType triggerType, string paramName) : base(paramName)
            {
                this.triggerType = triggerType;
            }

        }

        [SerializeField]
        Animator animator;

        [SerializeField]
        List<AnimatorParameterEvent> events = new();

        void Awake()
        {
            if (!enabled) return;
            if (animator == null)
                animator = GetComponent<Animator>();

            EventTrigger _et = gameObject.GetComponent<EventTrigger>();
            if (_et == null) _et = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry _entry_enter = new EventTrigger.Entry();
            _entry_enter.eventID = EventTriggerType.PointerEnter;
            _entry_enter.callback.AddListener((data) =>
            {
                OnPointerEnter(data as PointerEventData);
            });
            _et.triggers.Add(_entry_enter);

            EventTrigger.Entry _entry_exit = new EventTrigger.Entry();
            _entry_exit.eventID = EventTriggerType.PointerExit;
            _entry_exit.callback.AddListener((data) =>
            {
                OnPointerExit(data as PointerEventData);
            });
            _et.triggers.Add(_entry_exit);

            EventTrigger.Entry _entry_click = new EventTrigger.Entry();
            _entry_click.eventID = EventTriggerType.PointerClick;
            _entry_click.callback.AddListener((data) =>
            {
                OnPointerClick(data as PointerEventData);
            });
            _et.triggers.Add(_entry_click);

            EventTrigger.Entry _entry_Down = new EventTrigger.Entry();
            _entry_Down.eventID = EventTriggerType.PointerDown;
            _entry_Down.callback.AddListener((data) =>
            {
                OnPointerDown(data as PointerEventData);
            });
            _et.triggers.Add(_entry_Down);

            EventTrigger.Entry _entry_Up = new EventTrigger.Entry();
            _entry_Up.eventID = EventTriggerType.PointerUp;
            _entry_Up.callback.AddListener((data) =>
            {
                OnPointerUp(data as PointerEventData);
            });
            _et.triggers.Add(_entry_Up);
        }

        void Start()
        {
            foreach (var animEvent in events)
                animEvent.Init();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            foreach (var animEvent in events)
            {
                if(animEvent.TriggerType == EventTriggerType.PointerClick)
                    animEvent.SetParam(animator);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var animEvent in events)
            {
                if (animEvent.TriggerType == EventTriggerType.PointerEnter)
                    animEvent.SetParam(animator);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var animEvent in events)
            {
                if (animEvent.TriggerType == EventTriggerType.PointerExit)
                    animEvent.SetParam(animator);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            foreach (var animEvent in events)
            {
                if (animEvent.TriggerType == EventTriggerType.PointerDown)
                    animEvent.SetParam(animator);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            foreach (var animEvent in events)
            {
                if (animEvent.TriggerType == EventTriggerType.PointerUp)
                    animEvent.SetParam(animator);
            }
        }

        // ===

        public void OnPointerClick()
        {
            OnPointerClick(new PointerEventData(null));
        }

        public void OnPointerEnter()
        {
            OnPointerEnter(new PointerEventData(null));
        }

        public void OnPointerExit()
        {
            OnPointerExit(new PointerEventData(null));
        }

        public void OnPointerDown()
        {
            OnPointerDown(new PointerEventData(null));
        }

        public void OnPointerUp()
        {
            OnPointerUp(new PointerEventData(null));
        }
    }
}
