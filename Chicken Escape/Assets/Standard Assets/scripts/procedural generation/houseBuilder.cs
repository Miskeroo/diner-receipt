using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


// IMPORTANT  --------------------------------- DONT actually have a door prefab made yet.. ran outa steam.  PAst the enumerator value there is no coding backend to support doors....
// If you'd like to play around wtih hwo this works see if u can add the code for the Door prefab. Look where my prefabs are stored and how they are called in code...



//Enumerators....custom variable types that can only hold a predetermined set of Values...
//so wall's value can only be Window, door, blank or it will be set to null.


//ORDER OF EVENTS. 
/*1. Blueprint stage...
    a. Livable.
 *      I.Figure out front door spawn, Place 2x4 hallway going into house as start point.
 *      II. Create bathroom,bedroom,kitchen;
 *      III. 
 *  b. Add Extra Rooms...
 *      I. Livingroom, hallways, garage, ***stairs.
 *      
 *  c. Check for connections....  (make sure each room can reach ->FRONT DOOR;
 *       I. Once  A room type / variation is chosen....
 *              1. Make sure all required spaces are open.
 *              2. Is it connected to the front?
 *       
 *       
 *  d. Check for 
 *
 */


public enum Wall {Window,Door,Blank,StandardSection};
public enum Room {None,Living,Dining,Hallway,HouseEntrance,Family,Bedroom,Bathroom,Kitchen};



// a Struct is a custom variable type, (sort of like a  class, but passes by value. not refernce - better for storing data.);

//house node is from the first way to build houses... we now use RoomNode and RoomElement
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

    struct Variation
    {
        public int width;
        public int height;
        public Room Type;
        public int[,] map;

    }

    struct Coordinates
    {
        public int w;
        public int h;
    }

    //our giant list of prefabs

    public GameObject floor;
    public GameObject DoorSectionMain;
    public GameObject StandardWall;


    public GameObject livingRoom;
    public GameObject windowWall;
    public GameObject diningRoom;
    public GameObject doorWall;
    public Transform houseBase;
    public  int houseWidth;
    public  int houseHeight;
    public int houseWidthElements;
    public int houseHeightElements;
    bool built;

    private HouseNode[,] house;

    Dictionary<string, RoomNode> rooms;
    Dictionary<string, int> variationCount;

    //private RoomNode[,] houseTestRooms;
    private RoomElement[,] houseTestElements;
    

	// Use this for initialization
	void Awake () 
    {

        //Initialize the variation Count dictionary.
        variationCount = new Dictionary<string, int>(); 


        // we want to initizlie our struct//
	    house = new HouseNode[houseHeight, houseWidth];
        houseTestElements = new RoomElement[houseWidthElements, houseHeightElements];

        for (int w = 0; w < houseWidthElements; w++)
        {
            for (int h = 0; h < houseHeightElements; h++)
            {
                houseTestElements[w, h] = new RoomElement(w,h);
            }
        }


        rooms = new Dictionary<string, RoomNode>();
        //TODO this
        BuildBluePrint();
	}

  
	
     


	void BuildBluePrint()
    {
        //MIiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiskkkkkkkkkkkkkk////////////////////////////////
        //this will map out the house room wise..

        //string[] arrayHold = {"Window","Window","Door","Door"};


        bool livable = false;
        bool roomConnected = false;
        built = false;
        ///////////////////////////////new way entry point
        
        /*
        while(!built)
        {
            //Step 1 create front door hall way.. 
            CreateFrontDoor();
            
            while(!livable)
            {
                
                // this is next... STep 2

            }


        }
         * 
         */
        while(true)
        {
           
            CreateFrontDoor();
            if (MakeLivable())
            {
                break;
            }
            ReleaseAll();
        }

        ComputeWallsFinal(houseTestElements);
        
        ConstructFloor(houseTestElements);
        ConstructWalls(houseTestElements);
        //Next... furniture?

        //above array and these are currently hard coded... use these to play around with rooms and walls.
       
        
        
        //house[0,0] = new HouseNode("Living",arrayHold);
        //house[0,1] = new HouseNode("Dining", arrayHold);
        


        //Build up house ////// OLD WAY
        /*
        for (int widthPos = 0; widthPos < houseHeight; widthPos++)
        {
            for (int heightPos = 0; heightPos < houseWidth; heightPos++)
            {
                ConstructRoom(widthPos, heightPos, house[widthPos, heightPos]);
            }
        
        } */
        //////////////////////////////////////////////////////////////////////////////////////////////////////
    }



    ///Construct room, compute walls, construct walls, ALL OUTDATED. 

    /*
     * void ConstructRoom(int row, int column,HouseNode blueprint)
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
                case Wall.Door:
                    ConstructWalls(room, pos, "Door");
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
                 if (buildLocation == null)
                 {
                     Debug.LogError("Wall slot not found");
                 }
                 if(type == "Window" )
                   {
                      child = (GameObject)Instantiate(windowWall,buildLocation.position, buildLocation.rotation);
                      child.transform.SetParent(room.transform);
                   }

                 else if(type == "Door")
                 {
                     child = (GameObject)Instantiate(doorWall, buildLocation.position, buildLocation.rotation);
                     child.transform.SetParent(room.transform);
                 }

                break;

            case 1:

                buildLocation = room.transform.FindChild("wallSlot2");

                if(buildLocation == null)
                {
                    Debug.LogError("Wall slot not found");
                }

                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                else if (type == "Door")
                {
                    child = (GameObject)Instantiate(doorWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                break;

            case 2:

                buildLocation = room.transform.FindChild("wallSlot3");
                if (buildLocation == null)
                {
                    Debug.LogError("Wall slot not found");
                }
                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                else if (type == "Door")
                {
                    child = (GameObject)Instantiate(doorWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                break;

            case 3:
                buildLocation = room.transform.FindChild("wallSlot4");
                if (buildLocation == null)
                {
                    Debug.LogError("Wall slot not found");
                }
                if (type == "Window")
                {
                    child = (GameObject)Instantiate(windowWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                else if (type == "Door")
                {
                    child = (GameObject)Instantiate(doorWall, buildLocation.position, buildLocation.rotation);
                    child.transform.SetParent(room.transform);
                }

                break;
        }
    } */

    void CreateFrontDoor()
    {
        int chosenSeed = UnityEngine.Random.Range(0, 0);

        rooms.Add("Entrance1", new RoomNode(1, 2,houseWidthElements,houseHeightElements));
        rooms["Entrance1"].SetType((Room)Enum.Parse(typeof(Room), "HouseEntrance"));
        rooms["Entrance1"].AddRoomElementFront((Room)Enum.Parse(typeof(Room), "HouseEntrance"), chosenSeed, 0, houseTestElements);
        rooms["Entrance1"].AddRoomElement((Room)Enum.Parse(typeof(Room), "Hallway"), chosenSeed, 1, houseTestElements,"Hallway1");

        /*
        houseTestRooms[0, chosenSeed] = new RoomNode(1, 2,houseTestElements);
        houseTestRooms[0, chosenSeed].SetType((Room)Enum.Parse(typeof(Room),"Hallway"));
        houseTestRooms[0, chosenSeed].AddRoomElement((Room)Enum.Parse(typeof(Room), "Hallway"),0,chosenSeed);
         */
    }

    void AddToFloorPlan(Coordinates start, Variation variationIn, RoomElement[,] elementsIn)
    {
        //CODE VERY DIRTY.. RUSH JOB
        Debug.Log("FLOOR PLAN CALLED");

        //check if variation type has been added y et

        if(!variationCount.ContainsKey(variationIn.Type.ToString()))
        {
            variationCount.Add(variationIn.Type.ToString(), 1);
        }
        // if so icrement it
        else
        {
            variationCount[variationIn.Type.ToString()]++;
        }

        // add a new room
        rooms.Add(variationIn.Type.ToString() + variationCount[variationIn.Type.ToString()], new RoomNode(variationIn.width, variationIn.height, houseWidthElements, houseHeightElements));
        //set type of hte new room..
        rooms[variationIn.Type.ToString() + variationCount[variationIn.Type.ToString()]].SetType(variationIn.Type);
        //
       

        for(int currentW = 0; currentW < variationIn.width; currentW++)
        {

            for(int currentH = 0; currentH < variationIn.height; currentH++)
            {
                if(variationIn.map[currentW,currentH] == 1)
                {
                    //Debug.Log("ADDING ELEMENT TO " + (start.w + currentW) + "," + (start.h + currentH));
                    rooms[(string)(variationIn.Type.ToString() + variationCount[variationIn.Type.ToString()])].AddRoomElement(variationIn.Type, start.w + currentW, start.h + currentH, houseTestElements, variationIn.Type.ToString() +variationCount[variationIn.Type.ToString()]);
                }
            }

        }

    }

    void ComputeWalls(Coordinates start, Variation variationIn, RoomElement[,] elementsIn)
    {

        for (int currentW = 0; currentW < variationIn.width; currentW++)
        {

            for (int currentH = 0; currentH < variationIn.height; currentH++)
            {
                if (variationIn.map[currentW, currentH] == 1)
                {
                    for(int direction = 0; direction < 3; direction++)
                    {
                        //Top Most element...
                        if((start.h + currentH == 0) && direction == 0)
                        {
                            elementsIn[(start.w + currentW), (start.h + currentH)].SetWall(direction, Wall.StandardSection);
                            //Debug.Log("Setting wall because " + (start.w + currentW) + ", " + (start.h + currentH) + "is the foward cell");
                        }

                        //Right most elements
                        if(((start.w + currentW) == (houseWidthElements - 1)) && (direction == 1))
                        {
                            elementsIn[(start.w + currentW), (start.h + currentH)].SetWall(direction, Wall.StandardSection);
                            //Debug.Log("Setting wall because " + (start.w + currentW) + ", " + (start.h + currentH) + "is the right cell");
                        }

                        //bottom most elements
                        if (((start.h + currentH) == (houseHeightElements - 1)) && (direction == 2))
                        {
                            elementsIn[(start.w + currentW), (start.h + currentH)].SetWall(direction, Wall.StandardSection);
                            //Debug.Log("Setting wall because " + (start.w + currentW) + ", " + (start.h + currentH) + "is the back cell");
                        }

                        //left most elements.
                        if ((start.w + currentW == 0) && direction == 3)
                        {
                            elementsIn[(start.w + currentW), (start.h + currentH)].SetWall(direction, Wall.StandardSection);
                           // Debug.Log("Setting wall because " + (start.w + currentW) + ", " + (start.h + currentH) + "is the left cell");
                        }

                    }
                }
            }

        }


    }

    void ComputeWallsFinal(RoomElement[,] elementsIn)
    {
        for (int currentW = 0; currentW < houseWidthElements; currentW++)
        {

            for (int currentH = 0; currentH < houseHeightElements; currentH++)
            {
                if(elementsIn[currentW,currentH].IsTaken())
                {

                    RoomElement[] neighbors = elementsIn[currentW, currentH].GetNeighbors();
                    string[] wallInfo = elementsIn[currentW,currentH].GetWallInfo();

                    for(int direction = 0; direction < 4; direction++)
                    {      
                        if(wallInfo[direction] == null)
                        {
                            if(neighbors[direction] == null)
                            {
                                elementsIn[currentW, currentH].SetWall(direction, Wall.StandardSection);
                                switch (direction)
                                {
                                    case 0:
                                        Debug.Log("Setting Wall because the direction of above has no neighbor for element "+currentW +"," +currentH);
                                        break;
                                    case 1:
                                        Debug.Log("Setting Wall because the direction of right has no neighbor for element " + currentW + "," + currentH);
                                        break;
                                    case 2:
                                        Debug.Log("Setting Wall because the direction of below has no neighbor for element " + currentW + "," + currentH);
                                        break;
                                    case 3:
                                        Debug.Log("Setting Wall because the direction of left has no neighbor for element " + currentW + "," + currentH);
                                        break;
                                }
                            }
                        }

                    }
                }
                
            }

        }
    }

    bool CreateRoom(Room type, Variation variationIn, RoomElement[,] elementsIn)
    {
        Coordinates chosenSpot = FindOpenSpot(variationIn);

        if(chosenSpot.w == -1 || chosenSpot.h == -1)
        {
            Debug.Log("Error CreatesROOM");
            return false;
        }

        else
        {
            AddToFloorPlan(chosenSpot, variationIn, elementsIn);
            ComputeWalls(chosenSpot, variationIn, elementsIn);
            //do wall generation here...
        }

        return true;
    }

    void SetRoomElementsTaken(Variation variationIn, Coordinates start, RoomElement[,] elementsIn)
    {
        int currentW = 0;

        while(currentW < variationIn.width)
        {
            int currentH = 0;
            while(currentH < variationIn.height)
            {
                if(variationIn.map[currentW,currentH] == 1)
                {
                    elementsIn[start.w + currentW, start.h + currentH].SetTakenLight();
                }

                currentH++;
            }

            currentW++;
        }
        
    }

    Variation GetVariation(Room type, int variation)
    {
        //Get Variation information from text file.

        Variation variationIn = new Variation();
        variationIn.Type = type;

        try
        {
            using (StreamReader sr = new StreamReader("Assets\\Variations\\RoomVariations\\"+type.ToString() + ".txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line;
                    line = sr.ReadLine();
                    if(line.Contains("variation"+variation))
                    { 
                        // we are now where we need to be. 
                        //line 1 are the dimensions.

                        line = sr.ReadLine();

                        // set our demensions
                        
                        variationIn.width = (int)Char.GetNumericValue(line[0]);
                        variationIn.height = (int)Char.GetNumericValue(line[2]);
                        Debug.Log("The Dimensions of this Variation are : " + variationIn.width + " by " + variationIn.height);
                        // setup floor map.
                        variationIn.map = new int[variationIn.width, variationIn.height];

                        for (int h = 0; h < variationIn.height; h++ )
                        {

                            line = sr.ReadLine();

                            for(int w = 0; w < variationIn.width; w++)
                            {
                                variationIn.map[w, h] = (int)Char.GetNumericValue(line[w]);
                            }
                        }



                        return variationIn;
                    }           
                }

                Debug.Log("Variation Not found");
                return variationIn;

            }
        }

        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }


        return variationIn;
    }

    int AssignVariation(Room type)
    {
        string line;
        line = null;
        try
        {
            using (StreamReader sr = new StreamReader("Assets\\Variations\\RoomVariations\\" + type.ToString() + ".txt"))
            {

                
                line = sr.ReadLine();
            }
        }

        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }

        int variationRange;

        variationRange = Int32.Parse(line);

        int variation = UnityEngine.Random.Range(1, variationRange);
        

        return variation;
    }

    bool MakeLivable()
    {
        Variation variation = new Variation();
        bool hasBedroom;
        bool hasDiningRoom;
        bool hasLivingRoom;

        Room roomType = Room.Living;
        bool test = true;
        

        //Create the "core of the house"
       
        roomType = Room.Living;
        variation = GetVariation(roomType, AssignVariation(roomType));
        if(!CreateRoom(roomType,variation,houseTestElements))
        {
            return false;
        }
        roomType = Room.Family;
        variation = GetVariation(roomType, AssignVariation(roomType));
        if(!CreateRoom(roomType, variation, houseTestElements))
        {
             return false;
        }

        roomType = Room.Bedroom;
        variation = GetVariation(roomType, AssignVariation(roomType));
        if (!CreateRoom(roomType, variation, houseTestElements))
        {
            return false;
        }

        roomType = Room.Bathroom;
        variation = GetVariation(roomType, AssignVariation(roomType));
        if (!CreateRoom(roomType, variation, houseTestElements))
        {
            return false;
        }

        roomType = Room.Kitchen;
        variation = GetVariation(roomType, AssignVariation(roomType));
        if (!CreateRoom(roomType, variation, houseTestElements))
        {
            return false;
        }


        return true;

    }

    Coordinates FindOpenSpot(Variation variationIn)
    {
        //Here we check elements randomly within the house, to see if they are open spots.

        var intCoords = new List<Coordinates>();
        Coordinates tempCoord = new Coordinates();
        int tempW;
        int tempH;

        // while we havent checked every option.
        while (intCoords.Count < houseWidthElements * houseHeightElements)
        {
            tempW = UnityEngine.Random.Range(0,houseWidthElements);
            tempH = UnityEngine.Random.Range(0,houseHeightElements);

            tempCoord.h = tempH;
            tempCoord.w = tempW;

            if(!intCoords.Contains(tempCoord))
            {
                if(CheckOpenSpot(tempCoord.w, tempCoord.h,variationIn))
                {
                    //Debug.Log("Variation can fit on " + tempCoord.w + "," + tempCoord.h);
                    return tempCoord;
                }
                else
                {
                    //Debug.Log("Location " + tempCoord.w + ","+ tempCoord.h+" does not fit.");
                    intCoords.Add(tempCoord);
                }
            }

        }
            tempCoord.h = -1;
            tempCoord.w = -1;
            return tempCoord;   
    }

    bool CheckOpenSpot(int posW, int posH,Variation variationIn)
    {
       
        int counterW = 0;

        while (counterW < variationIn.width)
        {
            int counterH = 0;

            //if this position takes us past our max value
            if(counterW + posW >= houseWidthElements)
            {
                ClearTakenLight(posW, posH, variationIn, counterH, counterW);
                return false;
            }
                
            while(counterH < variationIn.height)
            {
                //if this position takes us past our max value
                if(counterH + posH >= houseHeightElements)
                {
                    ClearTakenLight(posW, posH, variationIn, counterH, counterW);
                    return false;
                }

                // if this position takes us to an already taken spot.
                if ((houseTestElements[counterW + posW, counterH + posH].IsTaken() && variationIn.map[counterW, counterH] == 1) || (houseTestElements[counterW + posW, counterH + posH].GetTakenLight() && variationIn.map[counterW, counterH] == 1))
                {
                    ClearTakenLight(posW, posH, variationIn, counterH, counterW);
                    return false;
                }

                else if (variationIn.map[counterW,counterH] == 1)
                {
                    houseTestElements[counterW + posW, counterH + posH].SetTakenLight();
                }
                counterH++;
            }
            counterW++;

        }
        return true;
            
    }

    void ConstructFloor(RoomElement[,] elementsIn)
    {

        //Debug.Log("COnstruct floor Called");
        GameObject child;
        Transform buildLocation;

        buildLocation = houseBase.transform;

        for(int w = 0; w < houseWidthElements; w++)
        {
            for(int h = 0; h < houseHeightElements; h++)
            {
                
                if(elementsIn[w,h].IsTaken())
                {
                    
                    child = (GameObject)Instantiate(floor, new Vector3(buildLocation.position.x + (2 * w), buildLocation.position.y, buildLocation.position.z + (2 * h * -1)), buildLocation.rotation);
                    child.transform.SetParent(houseBase.transform);
                    //Debug.Log("FLOOR BUILT FOR: "+  w + "," + h);
                }

                else
                {
                    //Debug.Log(w + "," + h + " Is not taken.");
                }
            }
        }
    }

    void ConstructWalls(RoomElement[,] elementsIn)
    {
        //Debug.Log("COnstruct Wall Called");
        GameObject child;
        Transform buildLocation;

        buildLocation = houseBase.transform;

        for (int w = 0; w < houseWidthElements; w++)
        {
            for (int h = 0; h < houseHeightElements; h++)
            {
                if (elementsIn[w, h].IsTaken())
                {
                    string[] elementWalls = elementsIn[w,h].GetWallInfo();

                    for(int wall = 0; wall < 4; wall++)
                    {
                        if(elementWalls[wall] != null)
                        {
                            
                           switch (wall)
                            {
                                case 0:
                                    child = (GameObject)Instantiate(Resources.Load(elementWalls[wall], typeof(GameObject)), new Vector3(buildLocation.position.x + (2 * w + 1), buildLocation.position.y+ 2.15f, buildLocation.position.z + (-2 * h + 0.9f)), buildLocation.rotation);
                                    child.transform.SetParent(houseBase.transform);
                                    //Debug.Log("Wall BUILT TYPE: " + elementWalls[wall] + " inside of room : " + elementsIn[w,h].GetParent());
                                    break;
                                case 1:
                                    child = (GameObject)Instantiate(Resources.Load(elementWalls[wall], typeof(GameObject)), new Vector3(buildLocation.position.x + (2 * w + 0.9f), buildLocation.position.y + 2.15f, buildLocation.position.z + (-2 * h - 0.9f )), buildLocation.rotation);
                                    child.transform.Rotate(0f,90f,0f);
                                    child.transform.SetParent(houseBase.transform);
                                    //Debug.Log("Wall BUILT TYPE: " + elementWalls[wall] + " inside of room : " + elementsIn[w, h].GetParent());
                                    break;
                                case 2:
                                    child = (GameObject)Instantiate(Resources.Load(elementWalls[wall], typeof(GameObject)), new Vector3(buildLocation.position.x + (2 * w + 0.9f), buildLocation.position.y + 2.15f, buildLocation.position.z + (-2 * h - 0.9f)), buildLocation.rotation);
                                    child.transform.SetParent(houseBase.transform);
                                    //Debug.Log("Wall BUILT TYPE: " + elementWalls[wall] + " inside of room : " + elementsIn[w, h].GetParent());
                                    break;
                                case 3:
                                    child = (GameObject)Instantiate(Resources.Load(elementWalls[wall], typeof(GameObject)), new Vector3(buildLocation.position.x + (2 * w - 0.9f), buildLocation.position.y + 2.15f, buildLocation.position.z + (-2 * h - 0.9f)), buildLocation.rotation);
                                    child.transform.SetParent(houseBase.transform);
                                    child.transform.Rotate(0f, 90f, 0f);
                                    //Debug.Log("Wall BUILT TYPE: " + elementWalls[wall] + " inside of room : " + elementsIn[w, h].GetParent());
                                    break;
                            }
                            
                        }
                    }


                    
                    
                }
            }
        }
    }


    void ClearTakenLight(int posW, int posH, Variation variationIn, int maxW, int maxH)
    {

        // work on this..
         int counterW = 0;

         while (counterW <= maxW)
         {
             int counterH = 0;

             if (counterW + posW >= houseWidthElements)
             {

                 return;
             }

             while (counterH <=maxH)
             {

                 if (counterH + posH >= houseHeightElements)
                 {
                     
                     return;
                 }

                 houseTestElements[posW + counterW,posH+ counterH].ReleaseTakenLight();
                 counterH++;
             }

             counterW++;
         }
    }
    void ReleaseAll()
    {
        variationCount = new Dictionary<string, int>();
        // we want to initizlie our struct//
        house = new HouseNode[houseHeight, houseWidth];
        houseTestElements = new RoomElement[houseWidthElements, houseHeightElements];

        for (int w = 0; w < houseWidthElements; w++)
        {
            for (int h = 0; h < houseHeightElements; h++)
            {
                houseTestElements[w, h] = new RoomElement(w, h);
            }
        }
        rooms = new Dictionary<string, RoomNode>();
    }

}
