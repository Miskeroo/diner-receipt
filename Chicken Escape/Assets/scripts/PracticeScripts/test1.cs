using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class test1 : MonoBehaviour 
{
    // IF THE GAME OBJECT THIS LISTENER IS FROM(THIS) ISNT ALIVE THIS LISTENER WILL STILL BE ACTIVE..
    //SO WE HAVE TO MAKE SURE WE CLEAN IT UP....
    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    //LISTEN ON ENABLE
    void OnEnable()
    {
        EventManager.StartListening("test", someListener);
    }

    //STOP LISTEN
    void OnDisable()
    {
        EventManager.StopListening("test", someListener);
    }

    void SomeFunction()
    {
        Debug.Log("Some function was called");
    }

	
}
