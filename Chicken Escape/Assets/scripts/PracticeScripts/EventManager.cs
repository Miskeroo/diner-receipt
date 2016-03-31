using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour 
{

    private Dictionary<string, UnityEvent> eventDictionary;




    private static EventManager m_eventManager;


    private static EventManager instance
    {
        get
        {
            Debug.Log("Initialize 2");
            if(!m_eventManager)
            {
                m_eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if(!m_eventManager)
                {
                    Debug.Log("Missing ACTIVE Event manager script on a GameObject in scene");
                }

                else
                {
                   m_eventManager.Init();
                }

            }

            return m_eventManager;
        }
    }

    void Init()
    {
        Debug.Log("Init Called Event Manager");
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }


    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        //try get value is faster as contains key / as efficient or more;
        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }

        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if(m_eventManager == null)
        {
            return;
        }

        UnityEvent thisEvent = null;
        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent ))
        {
            thisEvent.RemoveListener(listener);
        }
    }
	
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;

        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
	
}
