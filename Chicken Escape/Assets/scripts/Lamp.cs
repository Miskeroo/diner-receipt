using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Lamp : MonoBehaviour 
{

    private UnityAction dayListener;
    private UnityAction nightListener;

    private bool lightOn;

	// Use this for initialization


    void Awake()
    {

        lightOn = DayNight.GetTimeOfDay();
        dayListener = new UnityAction(ToDay);
        nightListener = new UnityAction(ToNight);
    }

    //LISTEN ON ENABLE
    void OnEnable()
    {
        EventManager.StartListening("DayTime", dayListener);
        EventManager.StartListening("NightTime", nightListener);
    }

    //STOP LISTEN
    void OnDisable()
    {
        EventManager.StopListening("DayTime", dayListener);
        EventManager.StopListening("NightTime", nightListener);
    }

	
    void ToDay()
    {
        Debug.Log("Daytime Found!");
        Light temp = this.gameObject.GetComponentInChildren<Light>();
        temp.enabled = false;
    }

    void ToNight()
    {
        Debug.Log("Night Found!");
        Light temp = this.gameObject.GetComponentInChildren<Light>();
        temp.enabled = true;


    }

	// Update is called once per frame
	
}
