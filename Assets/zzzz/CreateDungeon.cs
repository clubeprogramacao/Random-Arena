using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CreateDungeon : NetworkBehaviour {


    public GameObject floor;
    public Camera camera;
    

    public int max_x;
    public int max_y;

    public int numberOfDoors, doorsPlaced;

    public bool showFirst, showGrid, showLogs;
    private bool  boss, treasure, shop, hidden;

    public RoomClass[,] room;

	// Use this for initialization
	void Start ()
    {
        if (!isServer)
            return;
        boss     = false;
        treasure = false;
        shop     = false;
        hidden   = false;

        setupRoomsArray();
        /*
        Vector2[] toSetUp = setupFirstRoom();
        foreach (Vector2 pos in toSetUp)
        {
            Debug.Log("----------------------------   " + pos + "    {" + numberOfDoors + "}" + '\n');
            int min = 0; int max = 1;
            // in case this is the last "branch", and not enough rooms were created
            if (pos == toSetUp[toSetUp.Length-1] && numberOfDoors > 0)
                min = 1;
            setupRoom(pos, rand1(min, max));
        }*/
        int x = (int)max_x / 2;int y = (int)max_y / 2;
        RoomClass first = room[x, y];
        camera.transform.position = new Vector3(x*60, y*36,-10);
        // set its base parameters
        first.floor = RoomClass.FloorType.normal;
        first.status = RoomClass.Status.exists;
        first.type = RoomClass.RoomType.normal;

        // update adjacent room variables
        setAdjacents(x, y);
        int firstDoors = rand1(2, 4);
        setupRoom(x,y, firstDoors,0, true);

        CreateAll();

        Transform dungeon = GameObject.Find("Dungeon").transform;
        foreach(Transform roomlist in dungeon)
        {
            foreach (Transform component in roomlist)
            {
                NetworkServer.Spawn(component.gameObject);
                if (component.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    //Debug.Log("name: " + component.gameObject.name + "    color: " + component.gameObject.GetComponent<SpriteRenderer>().color);
                }
            }
        }
    }

    void setupRoomsArray()
    {
        room = new RoomClass[max_x, max_y];
        for (int i = 0; i< max_x; i++)
        {
            for(int j = 0; j < max_y; j++)
            {
                room[i,j] = new RoomClass();
                room[i, j].status = RoomClass.Status.empty;
                if (i == 0 || i == max_x - 1 || j == 0 || j == max_y - 1)
                {
                    room[i, j].status = RoomClass.Status.blocked;
                    room[i, j].adjacentEast = true;
                    room[i, j].adjacentNorth = true;
                }
            }
        }
    }
    

    void setupRoom(int x, int y, int numberOfExtraDoors, int generation, bool last)
    {
        generation++;
        if(showLogs)
            Debug.Log("Setting up room in " + new Vector2(x, y) + " with [" + numberOfExtraDoors + "] extra doors  {" + generation + "}" + '\n');
        // This room already has one door. We're adding more
        RoomClass thisRoom = room[x, y];
        switch (numberOfExtraDoors)
        {
            case 0: // Terminal Room
                int[] type = {1, 2, 3, 4 };
                if (treasure)
                    type = removeNumber(type, 1);
                if (boss)
                    type = removeNumber(type, 2);
                if (shop)
                    type = removeNumber(type, 3);
                if (hidden)
                    type = removeNumber(type, 4);
                switch (rand2(type)) // what type of terminal room?
                {
                    case 0: // normal
                        thisRoom.makeType(RoomClass.RoomType.normal);
                        break;
                    case 1: // treasure
                        thisRoom.makeType(RoomClass.RoomType.treasure);
                        changeSurroundingDoors(x, y, thisRoom);
                        treasure = true;
                        break;
                    case 2: // boss
                        thisRoom.makeType(RoomClass.RoomType.boss);
                        changeSurroundingDoors(x, y, thisRoom);
                        boss = true;
                        break;
                    case 3: // shop
                        thisRoom.makeType(RoomClass.RoomType.shop);
                        changeSurroundingDoors(x, y, thisRoom);
                        shop = true;
                        break;
                    case 4: // hidden
                        thisRoom.makeType(RoomClass.RoomType.hidden);
                        changeSurroundingDoors(x, y, thisRoom);
                        hidden = true;
                        break;
                    default: break;
                }
                if(showLogs)
                    Debug.Log("It is a " + thisRoom.type.ToString()+ "    {" + generation + "}" + '\n');
                return;
            default:
                // start with all directions as available
                int[] doors1 = { 0, 1, 2, 3 };
                // remove the impossible ones
                if (room[x, y + 1].adjacentRooms() > 1)
                    doors1 = removeNumber(doors1, 0);
                if (room[x, y - 1].adjacentRooms() > 1)
                    doors1 = removeNumber(doors1, 1);
                if (room[x + 1, y].adjacentRooms() > 1)
                    doors1 = removeNumber(doors1, 2);
                if (room[x - 1, y].adjacentRooms() > 1)
                    doors1 = removeNumber(doors1, 3);
                // copy it to another variable, because the 1st will be modified
                int[] doors2 = doors1;
                // preallocate the rooms. This will prevent the case where outer "layers" of rooms get placed in positions where previous "layers" forbid
                for (int i = 0; i < numberOfExtraDoors; ) {
                    i++;
                    if (doors1.Length == 0)
                        return;
                    int direction = rand2(doors1);

                    switch (direction)
                    {
                        
                        case 0: // north
                            // create a pointer to the next room on the direction chosen
                            RoomClass roomNorth = room[x, y + 1];
                            
                            // create door + frame
                            thisRoom.lockNorth = RoomClass.DoorLock.open;
                            thisRoom.frameNorth = RoomClass.DoorFrame.wood;

                            // setup the room information
                            roomNorth.status = RoomClass.Status.exists;
                            roomNorth.lockSouth = RoomClass.DoorLock.open;
                            roomNorth.frameSouth = RoomClass.DoorFrame.wood;

                            // update surrounding information
                            setAdjacents(x, y + 1);

                            // goes on to create the next rooms
                            //setupRoom(x, y + 1, rand1(min, max));

                            // never try this direction again
                            if (showLogs)
                                Debug.Log("Preallocated " + 0);
                            doors1 = removeNumber(doors1, 0);

                            break;
                        case 1: // south
                            RoomClass roomSouth = room[x, y - 1];
                            thisRoom.lockSouth = RoomClass.DoorLock.open;
                            thisRoom.frameSouth = RoomClass.DoorFrame.wood;
                            roomSouth.status = RoomClass.Status.exists;
                            roomSouth.lockNorth = RoomClass.DoorLock.open;
                            roomSouth.frameNorth = RoomClass.DoorFrame.wood;
                            setAdjacents(x, y - 1);
                            //setupRoom(x, y - 1, rand1(min, max));
                            doors1 = removeNumber(doors1, 1);
                            if (showLogs)
                                Debug.Log("Preallocated " + 1);

                            break;
                        case 2: // east
                            RoomClass roomEast = room[x + 1, y];
                            thisRoom.lockEast = RoomClass.DoorLock.open;
                            thisRoom.frameEast = RoomClass.DoorFrame.wood;
                            roomEast.status = RoomClass.Status.exists;
                            roomEast.lockWest = RoomClass.DoorLock.open;
                            roomEast.frameWest = RoomClass.DoorFrame.wood;
                            setAdjacents(x + 1, y);
                            //setupRoom(x + 1, y, rand1(min, max));
                            doors1 = removeNumber(doors1, 2);
                            if (showLogs)
                                Debug.Log("Preallocated " + 2);
                            break;
                        case 3: // west
                            RoomClass roomWest = room[x - 1, y];
                            thisRoom.lockWest = RoomClass.DoorLock.open;
                            thisRoom.frameWest = RoomClass.DoorFrame.wood;
                            roomWest.status = RoomClass.Status.exists;
                            roomWest.lockEast = RoomClass.DoorLock.open;
                            roomWest.frameEast = RoomClass.DoorFrame.wood;
                            setAdjacents(x - 1, y);
                            //setupRoom(x - 1, y, rand1(min, max));
                            doors1 = removeNumber(doors1, 3);
                            if (showLogs)
                                Debug.Log("Preallocated " + 3);
                            break;
                        default: break;
                    }

                    doorsPlaced++;
                }
                
                // pick the directions that successefully preallocated the adjacent rooms
                foreach (int direction in doors1)
                {
                    doors2 = removeNumber(doors2, direction);
                }
                if (doors2.Length == 0)
                {
                    if (showLogs)
                        Debug.Log("0 possible directions2 {" + generation + "}" + '\n');
                    return;
                }
                

                // start creating the rooms
                if(numberOfExtraDoors>1)
                    last = false;
                foreach (int direction in doors2)
                {
                    if (direction == doors2[doors2.Length-1] && doors2.Length > 1)
                        last = true;
                    // check how many doors it can/should have
                    int min = 0; int max = numberOfDoors-doorsPlaced;
                    if (numberOfDoors - doorsPlaced > 0 && last)
                        min = 1;
                    if (max < 0)
                        max = 0;
                    if (max > 3)
                        max = 3;
                    if (showLogs)
                        Debug.Log("Direction: " + direction + "    min: " + min + "   max: " + max + "   #doors: " + numberOfDoors + "    #placed: " + doorsPlaced + "   last: " + last.ToString() + '\n');
                    // now check the surroundings of the adjacent rooms so we know how many doors we can create there
                    int[] doors3 = { 0, 1, 2, 3 };
                    int X = x; int Y = y;
                    switch (direction)
                    {
                        case 0:
                            // we moved up boyz
                            Y = y + 1;
                            if (showLogs)
                                Debug.Log("trying direction " + direction + "    {" + generation + "}" + '\n');
                            // check the surroundings of the next room
                            if (room[X, Y + 1].adjacentRooms() > 1) // north
                                doors3 = removeNumber(doors3, 0);
                            if (room[X, Y - 1].adjacentRooms() > 1) // south
                                doors3 = removeNumber(doors3, 1);
                            if (room[X + 1, Y].adjacentRooms() > 1) // east
                                doors3 = removeNumber(doors3, 2);
                            if (room[X - 1, Y].adjacentRooms() > 1) // west
                                doors3 = removeNumber(doors3, 3);

                            if (max > doors3.Length)
                                max = doors3.Length;
                            if(min > max)
                            {
                                if (showLogs)
                                    Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 1:
                            Y = y - 1;
                            if (showLogs)
                                Debug.Log("trying direction " + direction + "    {" + generation + "}" + '\n');
                            // check the surroundings of the next room
                            if (room[X, Y + 1].adjacentRooms() > 1) // north
                                doors3 = removeNumber(doors3, 0);
                            if (room[X, Y - 1].adjacentRooms() > 1) // south
                                doors3 = removeNumber(doors3, 1);
                            if (room[X + 1, Y].adjacentRooms() > 1) // east
                                doors3 = removeNumber(doors3, 2);
                            if (room[X - 1, Y].adjacentRooms() > 1) // west
                                doors3 = removeNumber(doors3, 3);

                            if (max > doors3.Length)
                                max = doors3.Length;
                            if (min > max)
                            {
                                if (showLogs)
                                    Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 2:
                            X = x + 1;
                            if (showLogs)
                                Debug.Log("trying direction " + direction + "    {" + generation + "}" + '\n');
                            // check the surroundings of the next room
                            if (room[X, Y + 1].adjacentRooms() > 1) // north
                                doors3 = removeNumber(doors3, 0);
                            if (room[X, Y - 1].adjacentRooms() > 1) // south
                                doors3 = removeNumber(doors3, 1);
                            if (room[X + 1, Y].adjacentRooms() > 1) // east
                                doors3 = removeNumber(doors3, 2);
                            if (room[X - 1, Y].adjacentRooms() > 1) // west
                                doors3 = removeNumber(doors3, 3);

                            if (max > doors3.Length)
                                max = doors3.Length;
                            if (min > max)
                            {
                                if (showLogs)
                                    Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 3:
                            X = x - 1;
                            if (showLogs)
                                Debug.Log("trying direction " + direction + "    {" + generation + "}" + '\n');
                            // check the surroundings of the next room
                            if (room[X, Y + 1].adjacentRooms() > 1) // north
                                doors3 = removeNumber(doors3, 0);
                            if (room[X, Y - 1].adjacentRooms() > 1) // south
                                doors3 = removeNumber(doors3, 1);
                            if (room[X + 1, Y].adjacentRooms() > 1) // east
                                doors3 = removeNumber(doors3, 2);
                            if (room[X - 1, Y].adjacentRooms() > 1) // west
                                doors3 = removeNumber(doors3, 3);

                            if (max > doors3.Length)
                                max = doors3.Length;
                            if (min > max)
                            {
                                if (showLogs)
                                    Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        default: break;
                    }
                    if (showLogs)
                        Debug.Log("setupRoom(" + X + ", " + Y + ", rand1(" + min + ", " + max + "), " + generation + ", " + last + ");" + '\n');
                    setupRoom(X, Y, rand1(min, max), generation, last);
                }
                break;
        }


        return;
    }

    // makes the doors leading to this room a certain type (incomplete: missing lock)
    void changeSurroundingDoors(int x, int y, RoomClass thisroom)
    {
        if (thisroom.adjacentNorth)
        {
            room[x, y + 1].frameSouth = thisroom.frameNorth;
        }
        if (thisroom.adjacentSouth)
        {
            room[x, y - 1].frameNorth = thisroom.frameSouth;
        }
        if(thisroom.adjacentEast)
        {
            room[x + 1, y].frameWest = thisroom.frameEast;
        }
        if (thisroom.adjacentWest)
        {
            room[x - 1, y].frameEast = thisroom.frameWest;
        }
    }



    void CreateAll()
    {
        for (int i = 0; i < max_x; i++)
        {
            for (int j = 0; j < max_y; j++)
            {
                if (j == (int)max_y / 2 && i == (int)max_x / 2 && showFirst)
                    room[i, j].floor = RoomClass.FloorType.first;
                if(room[i,j].status == RoomClass.Status.exists || showGrid)
                    GetComponent<CreateRoom>().createRoom(i, j, room[i,j]);
            }
        }
    }
    
    void setAdjacents(int x, int y)
    {
        RoomClass thisRoom = room[x, y];
        // North
        if (room[x, y + 1].status == RoomClass.Status.empty)
            room[x, y + 1].status = RoomClass.Status.blocked;
        room[x, y + 1].adjacentSouth = true;

        // South
        if (room[x, y - 1].status == RoomClass.Status.empty)
            room[x, y - 1].status = RoomClass.Status.blocked;
        room[x, y - 1].adjacentNorth = true;

        // East
        if (room[x + 1, y].status == RoomClass.Status.empty)
            room[x + 1, y].status = RoomClass.Status.blocked;
        room[x + 1, y].adjacentWest = true;

        // West
        if (room[x - 1, y].status == RoomClass.Status.empty)
            room[x - 1, y].status = RoomClass.Status.blocked;
        room[x - 1, y].adjacentEast = true;
    }

    int rand1(int min, int max)
    {
        return Random.Range(min, max+1);
    }

    int rand2(int[] number)
    {
        if (number.GetLength(0) == 0)
            return -1;
        int index = Random.Range(0, (int)number.GetLength(0));
        return number[index];
    }


    int[] removeNumber(int[] array,int numberToRemove)
    {
        int[] ret = {};
        if( System.Array.IndexOf(array, numberToRemove) ==-1)
            return ret;
        if (array.Length == 1)
            return ret;
        int[] newArray = new int[array.Length - 1];
        for (int i = 0, j=0; i<newArray.Length;i++,j++)
        {
            if (array[i] == numberToRemove)
            {
                j++;
            }
            newArray[i] = array[j];
        }
        return newArray;
    }

    
}
