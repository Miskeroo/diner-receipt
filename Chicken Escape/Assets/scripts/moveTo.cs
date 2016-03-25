using UnityEngine;
using System.Collections;

public class moveTo : MonoBehaviour

{
    public NavMeshPath path;
    public Transform goal;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () 
    {
        //  basic path on start
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        path = new NavMeshPath();
	}
	
    public bool MoveObjectTo(Vector3 destination)
    {
        // create path for chicken, true if path is walkable
        if(NavMesh.CalculatePath(gameObject.transform.position, destination, NavMesh.AllAreas,path))
        {
            Debug.Log("Path calculated and routed");
            agent.destination = destination;
            return true;
        }

        else
        {
            return false;
        }
        
    }

	// Update is called once per frame
	void Update () 
    {


	
	}
}
