using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
    public string itemName;

    [SerializeField]
    private bool consumable;



	// Use this for initialization
	void Start () 
    {
	    
	}

    public bool Pickup()
    {
        return true;
    }

    string GetItemName()
    {
        return itemName;
    }
	
	

}
