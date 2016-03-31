using UnityEngine;
using System.Collections;

public class SunLight : MonoBehaviour 
{
    double sunDegree;
    Light sunSource;
	// Use this for initialization
	void Start () 
    {
        sunSource = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        SetSunAngle();

        if( DayNight.GetHour() < 9 && DayNight.GetHour() >= 4 )
        {
            Sunrise();
        }

        else if (DayNight.GetHour() <= 20 && DayNight.GetHour() > 17 )
        {   
            Sunset();
        }
	}

    void SetSunAngle()
    {
        //-77 Degrees so sunrise is at 6 am;

        double hour = (double)DayNight.GetHour() + ((double)DayNight.GetMinute() / 60f);


        //Morning
        if(5.66< hour && hour <= 13.66)
        {
            sunDegree = (hour - 5.66)*11.25;
        }

       //Afternoon
        else  if(13.66 < hour && hour <= 21.66)
        {
            sunDegree = (90 + (hour - 13.66) * 11.25);
        }

      //Night

        else  if(21.66< hour && hour <= 24)
        {
            sunDegree =   (hour - 21.66) * 22.5d + 180;
        }

        else if(hour <= 1.66)
        {
            sunDegree = (22.5 * hour + 52.65) + 180;
        }

        else
        {
            sunDegree = -1 * (90 - (hour - 1.66) * 22.5);
        }





        //sunDegree = (float)((float)DayNight.GetHour() + ((float)DayNight.GetMinute() / 60)) * 15 - 85;
        transform.rotation = Quaternion.Euler((float)sunDegree,0 , 0);
    }

    void Sunrise()
    {
        sunSource.intensity = ((float) ((float)(DayNight.GetHour() * 60) + (float)DayNight.GetMinute() - 240f  ) / 300f); 
    }

    void Sunset()
    {
        sunSource.intensity = (float)180f/((float)(DayNight.GetHour() * 60) + (float)DayNight.GetMinute() -900); 
    }
}
