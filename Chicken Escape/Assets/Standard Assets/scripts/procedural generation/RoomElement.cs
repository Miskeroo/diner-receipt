using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoomElement
{
    //Is our room element used?
    private bool taken;
    private bool takenLight;


    //used  to  check for connections... if this is true ALL elements must be able to find a way to this location.
    private bool frontDoor;

    //what type of room is our element part of? ex... "Bedroom", "Kitchen"
    private Room type;

    //name of parent for this element *ex: part of first created bedroom... "Bedroom1"
    private string parent;

    //
    private bool connected;

    //information for walls.....
    // "empty", "doorSection1", "doorSection2". Null will indicate not yet set not empty. // EVERY WALL MUST BE SET. 
    private string[] wallInfo;

    //Our neighbors... will need to become 3d when we implement multistory houses...
    //[0] foward [1] right [2] behind [3] left
    //null during CONSTRUCTION time will assume  wall should be built. 
    private RoomElement[] neighbors;

    int positionWidth;
    int positionHeight;


    //Constructor
    public RoomElement(int width, int height)
    {
        taken = false;
        type = Room.None;
        parent = null;
        wallInfo = new string[4];
        neighbors = new RoomElement[4];
        connected = false;
        positionHeight = height;
        positionWidth = width;

        //Used for blueprint mapping....Housebuilder.SetRoomElementsTaken()
        // accessed by.. get/set methods...
        takenLight = false;
        
    }


    /// <summary>
    /// ///////////////////////////////////Data METHODS//////////////////////////////////////////////////////////
    /// </summary>
    public void SetAsFront()
    {
        frontDoor = true;
        connected = true;
        wallInfo[0] = "DoorSectionMain";
    }

    public bool IsFront()
    {
        return frontDoor;
    }

    public bool MakeConnected()
    {
        if (!connected)
        {
            connected = true;
            return true;
        }

        // already connected
        else
            return false;
    }

    public bool IsConnected()
    {
        return connected;
    }

    public Room GetType()
    {
        return type;
    }

    public string GetParent()
    {
        return parent;
    }

    public void SetTaken(int widthPos, int heightPos,string parentIn)
    {
        parent = parentIn;
        positionHeight = heightPos;
        positionWidth = widthPos;
        taken = true;
    }

    public bool IsTaken()
    {
        return taken;
    }

    public void SetTakenLight()
    {
        takenLight = true;
    }

    public void ReleaseTakenLight()
    {
        takenLight = false;
    }

    public bool GetTakenLight()
    {
        return takenLight;
    }
    public RoomElement[] GetNeighbors()
    {
        return neighbors;
    }

    public void SetNeighbors(RoomElement neighbor, int direction)
    {
            
        
            neighbors[direction] = neighbor;
            
            switch (direction)
            {
                case 0:
                    neighbor.AlarmNewNeighbor(this,2);
                    break;
                case 1:
                    neighbor.AlarmNewNeighbor(this,3);
                    break;
                case 2:
                    neighbor.AlarmNewNeighbor(this,0);
                    break;
                case 3:
                    neighbor.AlarmNewNeighbor(this,1);
                    break;
            }
            //Neighbor alarm here


            if(neighbor.IsConnected())
            {
                connected = true;
                AlarmNewConnection();
                //connection alarm here
            }
        //TODO CREATE ALARM TO GET NEIGHBOR TO SET THIS as a neighbor
        //TODO Create alarm to update on new "Connected" neighbors"
        //ToDo ... we set our neighbors... next is to
        
    }

    // WE NEED TO CHECK THAT WALLS ARE NOT BLOCKING CONNECTION. 
    public void AlarmNewConnection()
    {

        for (int count = 0; count < 4; count++)
        {
            if (neighbors[count] != null && neighbors[count].MakeConnected())
            {
                neighbors[count].AlarmNewConnection();
            }

        }
     }
    
    public void AlarmNewNeighbor(RoomElement from ,int fromDirection)
    {
        
            if (neighbors[fromDirection] == null)
            {
                //Debug.Log("Direction is null, adding new neighbor.");
                neighbors[fromDirection] = from;
            }
            else
            {
                Debug.LogError("Direction had already been filled... neighbor Overload.");
            }

        
    }

    public string[] GetWallInfo()
    {
        return wallInfo;
    }

    public void SetWall(int direction, Wall type)
    {
        wallInfo[direction] = type.ToString();
    }

    public void SetType(Room typeInput)
    {
        type = typeInput;
    }


}
