using UnityEngine;
using System.Collections;

public class RoomNode
{
    // reference to all the elements in house
   // RoomElement[,] roomSections;
    //what roomElements this room needs.
    int[,] elementsNeeded;
    //width of house elements
    int widthHouseElements;
    int roomWidth;
    // height of house elements
    int heightHouseElements;
    int roomHeight;
    string name;
 
    Room type;

    public RoomNode(int width, int height,int houseWidthElements,int houseHeightElements)
    {
        Debug.Log("RoomNode Constructor");
        roomWidth = width;
        roomHeight = height;
        widthHouseElements = houseWidthElements;
        heightHouseElements = houseHeightElements;
        //roomSections = sectionInput;

        

    }

    public RoomNode()
    {

    }

    public Room GetType()
    {
        return type;
    }

    public void SetType(Room input)
    {
        type = input;
    }

    public void AddRoomElement(Room type, int widthPos, int heightPos,RoomElement[,]roomSections,string nameIn)
    {
        if(roomSections[widthPos,heightPos] == null)
        {
            Debug.Log("uhm");
        }

        //Debug.Log("Adding room Element " + widthPos + "," + heightPos);

        roomSections[widthPos, heightPos].SetType(type);
        roomSections[widthPos, heightPos].SetTaken(widthPos,heightPos,nameIn);
        SetNeighbors(widthPos, heightPos,roomSections);
        //Once neighbors set... we need to get connection info..
    }

    public void AddRoomElementFront(Room type, int widthPos, int heightPos, RoomElement[,] roomSections)
    {
        name = "Entrance1";

        if (roomSections[widthPos, heightPos] == null)
        {
            Debug.Log("uhm");
        }

        //Debug.Log("Adding room Element " + widthPos + "," + heightPos);
        roomSections[widthPos, heightPos].SetType(type);
        roomSections[widthPos, heightPos].SetTaken(widthPos, heightPos, "Entrance1");
        SetNeighbors(widthPos, heightPos, roomSections);
        roomSections[widthPos, heightPos].SetAsFront();
        //Once neighbors set... we need to get connection info..
        //ADD A SET AS FRONT CALL HERE....
    }

    public void SetNeighbors(int widthPos, int heightPos,RoomElement[,]roomSections)
    {
        //foward neighbor
        if( heightPos > 0  &&  roomSections[widthPos,heightPos - 1].IsTaken())
        {
            roomSections[widthPos, heightPos].SetNeighbors(roomSections[widthPos, heightPos - 1],0);
            //TODO THIS
        }

        //right neighbor
        if(widthPos < (widthHouseElements - 1) && roomSections[widthPos + 1, heightPos].IsTaken())
        {
            roomSections[widthPos, heightPos].SetNeighbors(roomSections[widthPos + 1, heightPos],1);
        }

        //behind neighbor
        if(heightPos < (heightHouseElements - 1) && roomSections[widthPos,heightPos+1].IsTaken())
        {
            roomSections[widthPos, heightPos].SetNeighbors(roomSections[widthPos, heightPos + 1], 2);
        }

        //left neighbor
        if(widthPos > 0 && roomSections[widthPos - 1, heightPos].IsTaken())
        {
            roomSections[widthPos,heightPos].SetNeighbors(roomSections[widthPos - 1,heightPos],3);
        }

    }

}

