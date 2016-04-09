using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


// IMPORTANT  --------------------------------- DONT actually have a door prefab made yet.. ran outa steam.  PAst the enumerator value there is no coding backend to support doors....
// If you'd like to play around wtih hwo this works see if u can add the code for the Door prefab. Look where my prefabs are stored and how they are called in code...



//Enumerators....custom variable types that can only hold a predetermined set of Values...
//so wall's value can only be Window, door, blank or it will be set to null.
public enum Wall {Window,Door,Blank};
public enum Room {Living,Dining};



// a Struct is a custom variable type, (sort of like a  class, but passes by value. not refernce - better for storing data.);
public struct HouseNode 
{
    //Direction {foward,right,back,left};
    Wall[] roomWalls;
    Room room;

    public HouseNode (string roomType,string[] walls)
    {


        roomWalls = new Wall[4];
        room = new Room();
        SetRoomType(roomType);

        if(walls.Length == 4)
            for (int i = 0; i <= 3; i++)
            {
                SetWall(i,walls[i]);
            }
        else
        {
            Debug.LogError("string[] walls must be length 4  array");
        }

    }

    public Room GetRoomType()
    {
        return room;
    }

    public Wall[] GetRoomWalls()
    {
        return roomWalls;
    }

    public void SetRoomType(string type)
    {
       // we are making sure our enum type is valid... to prevent errors
        if(Enum.IsDefined(typeof(Room), type))
        room = (Room)Enum.Parse(typeof(Room),type); 
       //Need better error h andling than this.. try/catch, exceptions
        else
        Debug.LogError("Room type not valid. must be Living, Dining");
    }

    public void SetWall(int direction, string type)
    {
        if(Enum.IsDefined(typeof(Wall), type))
        roomWalls[direction] = (Wall)Enum.Parse(typeof(Wall),type); 

        else 
        Debug.LogError("Wall type not valid. must be Window, Door, Blank");
    }

}

public class houseBuilder : MonoBehaviour 
{

    //our giant list of prefabs
    public GameObject livingRoom;
    public GameObject windowWall;
    public GameObject diningRoom;
    public Transform houseBase;
    public  int houseWidth;
    public  int houseHeight;

    private HouseNode[,] house;

	// Use this for initialization
	void Awake () 
    {
        // we want to initizlie our struct//
	    house = new HouseNode[houseHeight, houseWidth];
        BuildBluePrint();
	}
	
	void BuildBluePrint()
    {
        //MIiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiskkkkkkkkkkkkkk////////////////////////////////
        //this will map out the house room wise..

        string[] arrayHold = {"Window","Window","Blank","Blank"};

        //above array and these are currently hard coded... use these to play around with rooms and walls.

        house[0,0] = new HouseNode("Living",arrayHold);
        house[0,1] = new HouseNode("Dining", arrayHold);
        


        //Build up house
        for (int row = 0; row < houseHeight; row++)
        { 
            for (int column = 0; column < houseWidth; column++)
            {
                ConstructRoom(row,column,house[row,column]);
            }
        
        } 
    }

    void ConstructRoom(int row, int column,HouseNode blueprint)
    {

        //Builds the floorplan for the room.... (literally will just be the floor and the type of room it is once i get more assests to use)
        //Furniture will be assigned based on random numbers... they have pre determined slots in a room but if ithey spawn or not
        //will be determined based on a random number

        //we need this to set the room as a child to the house
        GameObject child;

        switch (blueprint.GetRoomType())
        {
            case Room.Dining:
                child = (GameObject)Instantiate(diningRoom, new Vector3(this.transform.position.x + row * 10, this.transform.position.y, this.transform.position.z + column * -10), Quaternion.identity);
                child.transform.SetParent(houseBase);
                ComputeWalls(blueprint, child);
                break;

            case Room.Living:
                child = (GameObject)Instantiate(livingRoom, new Vector3(this.transform.position.x + row * 10, this.transform.position.y, this.transform.position.z + column * -10), Quaternion.identity);
                child.transform.SetParent(houseBase);
                ComputeWalls(blueprint, child);
                break;
            default:
                //
                child = new GameObject();
                break;
        }
       
        
    }

    void ComputeWalls(HouseNode blueprint, GameObject room)
    {

        //Where are our walls going?

        Debug.Log("Called ComputeWalls");

        Wall[] hold = blueprint.GetRoomWalls();

        for (int pos = 0; pos < 4; pos++)
        {
            switch (hold[pos])
            {
                case Wall.Blank:
                    ConstructWalls(room,pos,"Blank");
                    break;
                case Wall.Window:
                    ConstructWalls(room, pos,"Window");
                    break;
                default:
                    break;
            }
        }
    }

    void ConstructWalls(GameObject room,int pos,string type)
    {
        Debug.Log("Called construct walls");
        //where is our wall being built in local space?
        Transform buildLocation;

        //we need this so we can set the walls as children to the room.
        GameObject child;

        switch (pos)
        {
            case 0:

                 buildLocation = room.transform.FindChild("wallSlot1");   
                 if(type == "Window" )
                   {
                      child = (GameObject)Instantiate(windowWall,buildLocation.position, buildLocation.rotation);
                      child.transform.SetParent(room.transform);
                   }
                break;

            case 1:

                buildLocation = room.transform.FindChild("wallSlot2");
                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, new Vector3(buildLocation.transform.position.x, buildLocation.transform.position.y, buildLocation.transform.position.z), buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }
                break;

            case 2:

                buildLocation = transform.FindChild("wallSlot3");
                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, new Vector3(buildLocation.transform.position.x, buildLocation.transform.position.y, buildLocation.transform.position.z), buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }
                break;

            case 3:
                buildLocation = transform.FindChild("wallSlot4");
                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, new Vector3(buildLocation.transform.position.x, buildLocation.transform.position.y, buildLocation.transform.position.z), buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }
                break;
        }
    }


}
