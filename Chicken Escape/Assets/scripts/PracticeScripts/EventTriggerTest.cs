using UnityEngine;
using System.Collections;

public class EventTriggerTest : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown("q"))
        {
            EventManager.TriggerEvent("test");
        }
        if (Input.GetKeyDown("e"))
        {
            EventManager.TriggerEvent("NightTime");
        }

        if(Input.GetKeyDown("t"))
        {
            EventManager.TriggerEvent("DayTime");
        }
	}
}
