using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityObjectEvent : UnityEvent<object> { }

public class EventManager : Singleton<EventManager>
{
    private static Dictionary<EEventsName, UnityObjectEvent> eventDictionary = new Dictionary<EEventsName, UnityObjectEvent>();

    public static void Subscribe(EEventsName name, UnityAction<object> action)
    {
        UnityObjectEvent currentEvent = null;
        if (eventDictionary == null) eventDictionary = new Dictionary<EEventsName, UnityObjectEvent>();

        if (eventDictionary.TryGetValue(name, out currentEvent))
        {
            currentEvent.AddListener(action);
        }
        else
        {
            currentEvent = new UnityObjectEvent();
            currentEvent.AddListener(action);
            eventDictionary.Add(name, currentEvent);
        }

    }

    public static void Unsubscribe(EEventsName name, UnityAction<object> action)
    {
        UnityObjectEvent currentEvent = null;
        if (eventDictionary.TryGetValue(name, out currentEvent))
        {
            currentEvent.RemoveListener(action);
        }
    }

    public static void OnEvent(EEventsName name, object param = null)
    {
        UnityObjectEvent currentEvent = null;

        if (eventDictionary.TryGetValue(name, out currentEvent))
        {
            currentEvent.Invoke(param);
        }
    }
}
