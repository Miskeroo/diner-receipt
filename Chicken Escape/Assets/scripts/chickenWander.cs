using UnityEngine;
using System.Collections;

public class chickenWander : MonoBehaviour
{
    // not used right now.
    public float chickenWanderSpeed;
    public float chickenChasedSpeed;

    // state of chicken
    public bool wandering;
    public bool running;

    // control timers
    private float currWanderTime;
    private float wanderUpdateTime;

    //moveTo script
    private moveTo moveControl;

	// Use this for initialization

    void Awake()
    {

    }

	void Start () 
    {
        // we want the chicken to initially be wandering
        wandering = true;
        running = false;
        wanderUpdateTime = 2;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        // is our chicken wandering
	    if(wandering)
        {
            // increase the time it has been wandering
            currWanderTime += Time.deltaTime * 1;
            Wander();
        }

        
	}

    void Wander()
    {
        if(wandering)
        {
            // has the chicken been wandering for longer than the cooldown for a new path?
            if(currWanderTime > wanderUpdateTime)
            {
                Debug.Log("Updating wander goal");
                // Create a new destination/ path
                Vector3 position = new Vector3(gameObject.transform.position.x + Random.Range(-7.0F, 7.0F), 0, gameObject.transform.position.z + Random.Range(-7.0F, 7.0F));
                moveControl = GetComponent<moveTo>();
                
                //MoveObjectTo - bool that returns if assignment was succesful and path is walkable
                if(moveControl.MoveObjectTo(position))
                {
                    currWanderTime = 0;
                }

                //if it path fails will update next frame
            }

        }


    }
}
