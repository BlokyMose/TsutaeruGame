using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityUtility
{
    public static class EventTriggerUtility
    {
        public static void AddEventTrigger(this Image image, Action onEvent, EventTriggerType eventTriggerType = EventTriggerType.PointerClick)
        {
            EventTrigger image_et = image.gameObject.GetComponent<EventTrigger>();
            if (image_et == null)
                image_et = image.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener((data) => onEvent.Invoke());
            image_et.triggers.Add(entry);
        }

        public static void AddEventTriggers(
            this Image image, 
            Action onEnter = null,
            Action onExit = null,
            Action onDown = null,
            Action onUp = null,
            Action onClick = null)
        {
            EventTrigger image_et = image.gameObject.GetComponent<EventTrigger>();
            if (image_et == null)
                image_et = image.gameObject.AddComponent<EventTrigger>();

            if (onEnter != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => onEnter.Invoke());
                image_et.triggers.Add(entry);
            }
            if (onExit != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((data) => onExit.Invoke());
                image_et.triggers.Add(entry);
            }
            if (onDown != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => onDown.Invoke());
                image_et.triggers.Add(entry);
            }
            if (onUp != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((data) => onUp.Invoke());
                image_et.triggers.Add(entry);
            }
            if (onClick != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((data) => onClick.Invoke());
                image_et.triggers.Add(entry);
            }
        }
    }
}
