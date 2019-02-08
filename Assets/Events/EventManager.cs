using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{

    [System.Serializable]
    public class OneArgEvent : UnityEvent<System.Object> { }

    private Dictionary<Events, UnityEvent> noArgEventDictionary;
    private Dictionary<Events, OneArgEvent> oneArgEventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (noArgEventDictionary == null)
        {
            noArgEventDictionary = new Dictionary<Events, UnityEvent>();
        }
        if (oneArgEventDictionary == null)
        {
            oneArgEventDictionary = new Dictionary<Events, OneArgEvent>();
        }
    }

    public static void StartListening(Events eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.noArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.noArgEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Events eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.noArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Events eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.noArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void StartListening(Events eventName, UnityAction<System.Object> listener)
    {
        OneArgEvent thisEvent = null;
        if (instance.oneArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new OneArgEvent();
            thisEvent.AddListener(listener);
            instance.oneArgEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Events eventName, UnityAction<System.Object> listener)
    {
        if (eventManager == null) return;
        OneArgEvent thisEvent = null;
        if (instance.oneArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Events eventName, System.Object arg1)
    {
        OneArgEvent thisEvent = null;
        if (instance.oneArgEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(arg1);
        }
    }
}