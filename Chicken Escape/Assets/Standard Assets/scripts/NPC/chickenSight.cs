using UnityEngine;
using System.Collections;

public class chickenSight : MonoBehaviour 
{

    //TIPS FOR MISK AND FLIGHTY----------------------------------------
    //always declare your class wide variables at the top of the script..
    //just for organization do publics first then privates.
    // public variables can be changed even at RUNTIME in the UNITY EDITOR which means you can
    // play with variables to test what will happen.  - these will show up in the INSEPCTOR


    // from what degree can chicken see infront of him
    public float sightFieldOfView;

    // how far in a given direction... 

    public float sightRange;

    // can it currently see the player?
    public bool canSeePlayer;

    // is the chicken running for its LIFE
    public bool chased;

    // how long will a chicken run for before it goes back to wandering
    public float alarmedTime;

    public AudioSource alarmAlert;

    // navMesh for level - ask me about it and i will explain how to create- may use this to check for distance
    private NavMeshAgent nav;

    // radius of sound that will trigger the chicken
    private SphereCollider chickenHearRadius;

    //the player reference
    private GameObject thePlayer;

    // how long the chicken has been alarmed for since last event;
    private float currAlarmTime;

    // how long to wait before checking for player again;
    private float alarmCooldown;

    //TIPS FOR MISK AND FLIGHTY-----------------------------------------------------
    // AWAKE AND START ARE BOTH FUNCTIONS USED MAINLY FOR INITIALIZATIONSZSSS
    //Awake is called when an object the script is attached to is created.
    //Start is called when the script compenent of the function is enabled. 
    // this means awake will always be run upon creation, while disabled scripts will not have
    // their Start() function called until enabled. 


	
    // use this for references... for now
    void Awake()
    {
        // we use GetComponent<>() to find other things attached to THIS game component;

        // multiple audio sources appear in an array when called by get components. Ordered as they appear in INSPECTOR in the EDITOR
        AudioSource[] sources = GetComponents<AudioSource>();
        alarmAlert = sources[1];
        nav = GetComponent<NavMeshAgent>();
        chickenHearRadius = GetComponent<SphereCollider>();
        thePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    // Use this for initialization for now...
	void Start ()
    {
        currAlarmTime = 0f;
        alarmCooldown = .5f;
        chased = false;
        canSeePlayer = false;
        
	
	}
	
	// Update is called once per frame 
	void Update () 
    {
        if(chased)
        {
            currAlarmTime += Time.deltaTime;
        }
        
	
	}


    // THIS WILL BE OUR FUNCTION FOR SIGHT AND HEARING...
    // HEARING HAS NOT BEEN IMPLEMENTED  YET.
    void OnTriggerStay(Collider other)
    {

        // we are being chased and the alarm time cooldown has passed, or we are not being chased
        if ((chased && (currAlarmTime > alarmCooldown)) || !chased)
        {

            if (other.gameObject == thePlayer)
            {
                // set it false for now.  the player is colliding with the view range of our object,
                // but we dont know if it is in the field of view yet.
                canSeePlayer = false;


                // get the direction of the collision from the object this script is attached to. 
                Vector3 direction = other.transform.position - transform.position;

                // we are calculating hte ANGLE form the FRONT of the object this script is attahced to.
                float angle = Vector3.Angle(direction, transform.forward);


                // WE CUT THE ANGLE IN HALF RIGHT FLIGHTY?
                if (angle < sightFieldOfView * .5f)
                {
                    // RAYCASTS...
                    // a raycast is a line that GOES DIRECTLY OUT... SHOOTING GAMES USE THIS FOR BULLETS.
                    RaycastHit hit;


                    //ok so this is waht this line means..

                    // we are invoking raycast, AT THE POSITION of this objects transform. but we are moving the raycast
                    //UP halfway the height of the object.
                    //going out as far as CHICKEN HEAR RADIUS is ( the fartherst are chicken can see/ hear)
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, chickenHearRadius.radius))
                    {
                        // if our raycast hits the player
                        if (hit.collider.gameObject == thePlayer)
                        {
                            canSeePlayer = true;
                            // reset time since last sight check;
                            currAlarmTime = 0;
                            chased = true;
                            
                            // play the alert bell
                            alarmAlert.Play();

                        }
                    }
                }

            }
        }
    }


}
