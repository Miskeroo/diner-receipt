using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour 
{
    public bool grabbing;
    public Camera camera;
    public LayerMask layerMask;
    public float distance;

    private objectGrabbed heldObject;
    private Rigidbody m_body;
    private GameObject m_item;
    private Item itemPickup;
    

	// Use this for initialization
	void Start () 
    {
        grabbing = false;
       
	}
	
	// Update is called once per frame
	void Update () 
    {

        

	    if(Input.GetKeyDown(KeyCode.F) && !grabbing)
        {
            Grab();
        }
        else if (Input.GetKeyDown(KeyCode.F) && grabbing)
        {
            Release();
        }

        else if(grabbing)
        {
            Carry(m_item.transform.parent.gameObject);
        }
       
	}

    void Grab()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
      

        if (Physics.Raycast(ray, out hit,4,layerMask.value))
        {
            Debug.DrawLine(camera.gameObject.transform.position, hit.collider.gameObject.transform.position, Color.red);
            if(hit.collider.gameObject.tag == "handheld")
            {
                Debug.Log("Grabbing");
                heldObject = hit.collider.gameObject.GetComponentInParent<objectGrabbed>();
                if (heldObject != null)
                {
                    heldObject.Grabbed();
                    grabbing = true;
                    m_item = hit.collider.gameObject;
                    m_body = hit.collider.gameObject.GetComponentInParent<Rigidbody>();
                    m_body.isKinematic = true;

                    Carry(m_item.transform.parent.gameObject);
                }
            }

            else if (hit.collider.gameObject.tag == "itemPickup")
            {
                Debug.Log("Picked up Item");

                itemPickup = hit.collider.gameObject.GetComponent<Item>();
                m_item = hit.collider.gameObject;
               
                
                if(itemPickup.Pickup())
                {
                    Debug.Log("Picked up: " + itemPickup.itemName);
                    Destroy(m_item);
                }
            }

            else
            {
                Debug.Log("Grab failed");
            }

            // Do something with the object that was hit by the raycast.
        }

        //
        else
        {
            Debug.Log("Failed to grab");
        }
    }

    void Release()
    {
        Debug.Log("released");
        heldObject.Released();
        grabbing = false;
    }
    
    void Carry(GameObject m_object)
    {
        Debug.Log("Carry");
       
        m_object.transform.position = camera.transform.position + camera.transform.forward * distance;


    }

}
