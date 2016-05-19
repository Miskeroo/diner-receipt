using UnityEngine;
using System.Collections;

public class PocketPickup : MonoBehaviour 
{
    // From now on member variables will be marked a wtih a m_ for identification.
    private bool m_leftPocketEmpty;
    private bool m_rightPocketEmpty;
    private string m_leftPocket;
    private string m_rightPocket;

    private bool m_leftHandEmpty;
    private bool m_rightHandEmpty;
    private string m_leftHand;
    private string m_rightHand;


	// Use this for initialization
	void Start () 
    {
        m_leftHandEmpty = true;
        m_leftPocketEmpty = true;

        m_rightHandEmpty = true;
        m_rightPocketEmpty = true;
	}
    void putInPocket(string side, string item)
    {
        if (GetPocketStatus(side))
        {
            if(side == "right")
            {
                m_rightPocket = item;
                m_rightPocketEmpty = false;
            }

            else if(side == "left")
            {
                m_leftPocket = item;
                m_leftPocketEmpty = false;
            }

        }
        else
        {
            Debug.Log("Pocket Full");
        }
    }

    void Pickup(string side,string item)
    {
        if(GetHandStatus(side))
        {
            if (side == "right")
            {
                m_rightHand = item;
                m_rightHandEmpty = false;
            }

            else if (side == "left")
            {
                m_leftHand = item;
                m_leftHandEmpty = false;
            }
        }
        else
        {
            Debug.Log("Hand Full");
        }
    }

   

    bool GetPocketStatus(string side)
    {
        if (side == "right")
        {
            return m_rightPocketEmpty;
        }

        else if (side == "Left")
        {
            return m_leftPocketEmpty;
        }

        else
        {
            Debug.LogError("side must be either left or right.");
            return false;
        }
    }
	
    //Returns True if Empty, false if occupied
    bool GetHandStatus(string side)
    {
        
        if(side == "right")
        {
            return m_rightHandEmpty;
        }

        else if (side == "Left")
        {
            return m_leftHandEmpty;
        }

        else
        {
            Debug.LogError("side must be either left or right.");
            return false;
        }

    }

    

}
