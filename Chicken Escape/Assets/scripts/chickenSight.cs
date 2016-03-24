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

    // navMesh for level - ask me about it and i will explain how to create- may use this to check for distance
    private NavMeshAgent nav;

    // radius of sound that will trigger the chicken
    private SphereCollider chickenHearRadius;

    //the player reference
    private GameObject thePlayer;

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

        nav = GetComponent<NavMeshAgent>();
        chickenHearRadius = GetComponent<SphereCollider>();
        thePlayer = GameObject.FindGameObjectWithTag("Player");

    }

    // Use this for initialization for now...
	void Start ()
    {
        chased = false;
        canSeePlayer = false;
        
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
