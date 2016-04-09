using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
    public string itemName;

    [SerializeField]
    private bool consumable;
    [SerializeField]
    private string itemType;



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
	
	public string GetItemType()
    {
        return itemType;
    }

}
