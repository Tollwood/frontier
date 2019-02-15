
using UnityEngine.Events;
using System.Collections.Generic;

public static class EventManager 
{

    [System.Serializable]
    public class OneArgEvent : UnityEvent<object> { }

    private static Dictionary<Events, UnityEvent> noArgEventDictionary = new Dictionary<Events, UnityEvent>();
    private static Dictionary<Events, OneArgEvent> oneArgEventDictionary = new Dictionary<Events, OneArgEvent>();

      
    public static void StartListening(Events eventName, UnityAction listener)
    {
        if (noArgEventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            noArgEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Events eventName, UnityAction listener)
    {

        if (noArgEventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Events eventName)
    {
        if (noArgEventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void StartListening(Events eventName, UnityAction<object> listener)
    {
        if (oneArgEventDictionary.TryGetValue(eventName, out OneArgEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new OneArgEvent();
            thisEvent.AddListener(listener);
            oneArgEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Events eventName, UnityAction<object> listener)
    {
        if (oneArgEventDictionary.TryGetValue(eventName, out OneArgEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(Events eventName, object arg1)
    {
        if (oneArgEventDictionary.TryGetValue(eventName, out OneArgEvent thisEvent))
        {
            thisEvent.Invoke(arg1);
        }
    }
}