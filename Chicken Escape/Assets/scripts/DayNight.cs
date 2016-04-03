using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class DayNight : MonoBehaviour 
{
    private static bool day = true;
    private static bool night;
    private static float timeModifier;
    private static float minutesExact;
    private static int minutesRound;
    private static int hours;
    private static int dayNum;
    public static int minutesInDay;

    private static DayNight m_dayNight;


    private static DayNight instance
    {
        get
        {
            Debug.Log("Initialize 1");
            if(!m_dayNight)
            {
                m_dayNight = FindObjectOfType(typeof(DayNight)) as DayNight;

                if (!m_dayNight)
                {
                    Debug.Log("Missing ACTIVE DAY NIGHT script on a GameObject in scene");
                }

                else
                {
                    m_dayNight.Init();
                }

            }

            return m_dayNight;
        }
    }


    void Init()
    {
        Debug.Log("Init Called Day Night");
        minutesInDay = 3;

        timeModifier = 1 / ((float)(60 * minutesInDay) / 1440);

        minutesExact = 0;
        hours = 6;
        dayNum = 0;
    }
	
	
    void Awake()
    {
        Init();
    }
	
	// Update is called once per frame


    //TODO CUT THIS UPDATE UP INTO SMALLLER FUNCTIONS. 
	void Update () 
    {
        //Debug.Log("It is now " + hours + ":"+minutesRound);
        

        minutesExact = minutesExact + (Time.deltaTime * (timeModifier));
        
        while(minutesExact >= 1f)
        {

            minutesExact--;
            minutesRound++;

            if(minutesRound >= 60)
            {
                minutesRound -= 60;
                hours++;
                if(day && ((hours >= 20) || (hours < 6)))
                    {
                        day = false;
                        night = true;
                        EventManager.TriggerEvent("NightTime");
                    }
                else if(night && (hours >= 6) && (hours <20))
                {
                    day = true;
                    night = false;
                    EventManager.TriggerEvent("DayTime");
                }

                if(hours >= 24 )
                {
                    hours -= 24;
                    dayNum++;
                }
            }
        }
	
	}

    public static int GetHour()
    {
        return  hours;
    }

    public static int GetMinute()
    {
        return minutesRound;
    }

    public static  bool GetTimeOfDay()
    {
        return day;
    }


}
