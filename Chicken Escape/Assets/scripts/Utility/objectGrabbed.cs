using UnityEngine;
using System.Collections;

public class objectGrabbed : MonoBehaviour 
{
    public bool isNPC;

    private chickenSight chickenSight;
    private chickenWander chickenWander;
    private NavMeshAgent nav;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	
    public void Grabbed()
    {
        if(isNPC)
        {
            chickenSight = this.gameObject.GetComponent<chickenSight>();
            chickenWander = this.gameObject.GetComponent<chickenWander>();
            nav = this.gameObject.GetComponent<NavMeshAgent>();

            chickenSight.enabled = false;
            chickenWander.enabled = false;
            nav.enabled = false;
            
        }

    }

    public void Released()
    {
        if(isNPC)
        {
            nav.enabled = true;
            chickenSight.enabled = true;
            chickenWander.enabled = true;
        }

    }
}
