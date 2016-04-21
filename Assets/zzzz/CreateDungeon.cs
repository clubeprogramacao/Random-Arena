using UnityEngine;
using System.Collections;

public class CreateDungeon : MonoBehaviour {


    public GameObject floor;

    

    public int max_x;
    public int max_y;

    public int numberOfDoors, doorsPlaced;

    public bool  boss, treasure, shop, hidden;

    public RoomClass[,] room;

	// Use this for initialization
	void Start ()
    {
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

        // set its base parameters
        first.floor = RoomClass.FloorType.first;
        first.status = RoomClass.Status.exists;
        first.type = RoomClass.RoomType.normal;

        // update adjacent room variables
        setAdjacents(x, y);
        int firstDoors = rand1(2, 4);
        setupRoom(x,y, firstDoors,0);

        CreateAll();
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
    
    /* Vector2[] setupFirstRoom()
    {
        // select the room at the center of the grid
        Vector2 pos = new Vector2((int)max_x/2, (int)max_y/2);
        RoomClass first = room[(int)pos.x, (int)pos.y];

        // set its base parameters
        first.floor = RoomClass.FloorType.first;
        first.status = RoomClass.Status.exists;
        first.type = RoomClass.RoomType.normal;

        // update adjacent room variables
        setAdjacents((int)pos.x, (int)pos.y);

        // Create at least 2 adjacent rooms
        int doorsToOpen = rand1(2, 4);//doorsToOpen = 2;
        Vector2[] createdRooms = new Vector2[doorsToOpen];
        int[] door = { 1,2, 3, 4 };
        for (int i = 0; i < doorsToOpen; i++ )
        {
            if (door == null)
                return new Vector2[0];
            int pick = rand2(door);
            switch (pick)
            {
                case 1: // North
                    if(first.lockNorth == RoomClass.DoorLock.none)
                    {
                        // create the door + frame
                        first.lockNorth = RoomClass.DoorLock.open;
                        first.frameNorth = RoomClass.DoorFrame.wood;
                        // set up adjacent room as existing
                        room[(int)pos.x, (int)pos.y + 1].status = RoomClass.Status.exists;
                        room[(int)pos.x, (int)pos.y + 1].lockSouth = RoomClass.DoorLock.open;
                        room[(int)pos.x, (int)pos.y + 1].frameSouth = RoomClass.DoorFrame.wood;
                        // because they exist, update its surrounding rooms
                        setAdjacents((int)pos.x, (int)pos.y + 1);
                        // place its position on the return array
                        createdRooms[i] = new Vector2((int)pos.x, (int)pos.y + 1);
                        // remove this direction to avoid repeating code
                        door = removeNumber(door, 1);
                        // decrease the number of doors set for this dungeon
                        numberOfDoors--;
                    }
                    break;
                case 2: // South
                    if (first.lockSouth == RoomClass.DoorLock.none)
                    {
                        first.lockSouth = RoomClass.DoorLock.open;
                        first.frameSouth = RoomClass.DoorFrame.wood;
                        room[(int)pos.x, (int)pos.y - 1].status = RoomClass.Status.exists;
                        room[(int)pos.x, (int)pos.y - 1].lockNorth = RoomClass.DoorLock.open;
                        room[(int)pos.x, (int)pos.y - 1].frameNorth = RoomClass.DoorFrame.wood;
                        setAdjacents((int)pos.x, (int)pos.y - 1);
                        createdRooms[i] = new Vector2((int)pos.x, (int)pos.y - 1);
                        door = removeNumber(door, 2);
                        numberOfDoors--;
                    }
                    break;
                case 3: // East
                    if (first.lockEast == RoomClass.DoorLock.none)
                    {
                        first.lockEast = RoomClass.DoorLock.open;
                        first.frameEast = RoomClass.DoorFrame.wood;
                        room[(int)pos.x + 1, (int)pos.y].status = RoomClass.Status.exists;
                        room[(int)pos.x + 1, (int)pos.y].lockWest = RoomClass.DoorLock.open;
                        room[(int)pos.x + 1, (int)pos.y].frameWest = RoomClass.DoorFrame.wood;
                        setAdjacents((int)pos.x + 1, (int)pos.y);
                        createdRooms[i] = new Vector2((int)pos.x + 1, (int)pos.y);
                        door = removeNumber(door, 3);
                        numberOfDoors--;
                    }
                    break;
                case 4: // West
                    if (first.lockWest == RoomClass.DoorLock.none)
                    {
                        first.lockWest = RoomClass.DoorLock.open;
                        first.frameWest = RoomClass.DoorFrame.wood;
                        room[(int)pos.x - 1, (int)pos.y].status = RoomClass.Status.exists;
                        room[(int)pos.x - 1, (int)pos.y].lockEast = RoomClass.DoorLock.open;
                        room[(int)pos.x - 1, (int)pos.y].frameEast = RoomClass.DoorFrame.wood;
                        setAdjacents((int)pos.x - 1, (int)pos.y);
                        createdRooms[i] = new Vector2((int)pos.x - 1, (int)pos.y);
                        door = removeNumber(door, 4);
                        numberOfDoors--;
                    }
                    break;
                default:
                    break;
            }
        }
        // set adjacent rooms where there are no doors as blocked
        for(int i = 0; i < door.Length; i++){
            switch (door[i])
            {
                case 1: // north
                    room[(int)pos.x, (int)pos.y + 1].status = RoomClass.Status.blocked;
                    room[(int)pos.x, (int)pos.y + 1].adjacentSouth = true;
                    break;
                case 2: // south
                    room[(int)pos.x, (int)pos.y - 1].status = RoomClass.Status.blocked;
                    room[(int)pos.x, (int)pos.y - 1].adjacentNorth = true;
                    break;
                case 3: // east
                    room[(int)pos.x + 1, (int)pos.y].status = RoomClass.Status.blocked;
                    room[(int)pos.x + 1, (int)pos.y].adjacentWest = true;
                    break;
                case 4: // west
                    room[(int)pos.x - 1, (int)pos.y].status = RoomClass.Status.blocked;
                    room[(int)pos.x - 1, (int)pos.y].adjacentEast = true;
                    break;
                default: break;
            }
        }
        return createdRooms;
    }
    */


    void setupRoom(int x, int y, int numberOfExtraDoors, int generation)
    {
        generation++;
        Debug.Log("Setting up room in " + new Vector2(x, y) + " with [" + numberOfExtraDoors + "] extra doors  {" + generation + "}" + '\n');
        // This room already has one door. We're adding more
        RoomClass thisRoom = room[x, y];
        switch (numberOfExtraDoors)
        {
            case 0: // Terminal Room
                switch (rand1(0, 2)) // what type of terminal room?
                {
                    case 0: // normal
                        thisRoom.makeType(RoomClass.RoomType.normal);
                        break;
                    case 1: // treasure
                        thisRoom.makeType(RoomClass.RoomType.treasure);
                        changeSurroundingDoors(x, y, thisRoom);
                        break;
                    case 2: // boss
                        thisRoom.makeType(RoomClass.RoomType.boss);
                        changeSurroundingDoors(x, y, thisRoom);
                        break;
                    case 3: // shop
                        thisRoom.makeType(RoomClass.RoomType.shop);
                        break;
                    case 4: // hidden
                        thisRoom.makeType(RoomClass.RoomType.hidden);
                        break;
                    default: break;
                }
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
                foreach (int d in doors1)
                {
                    Debug.Log("before " + new Vector2(x, y) + " direction = " + d + "   {" + generation + "}" + '\n');
                }
                Debug.Log("--------" + '\n');
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
                            Debug.Log("removing " + 0);
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
                            Debug.Log("removing " + 1);

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
                            Debug.Log("removing " + 2);
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
                            Debug.Log("removing " + 3);
                            break;
                        default: break;
                    }

                    doorsPlaced++;
                }
                // pick the directions that successefully preallocated the adjacent rooms
                foreach(int direction in doors1)
                {
                    doors2 = removeNumber(doors2, direction);
                }
                if (doors2.Length == 0)
                {
                    Debug.Log("0 possible directions2 {" + generation + "}" + '\n');
                    return;
                }

                foreach (int d in doors2)
                    Debug.Log("after " + new Vector2(x, y) + " direction = " + d + "   {" + generation + "}" + '\n');

                // start creating the rooms
                bool last = false;
                foreach (int direction in doors2)
                {
                    if (direction == doors2[doors2.Length-1])
                        last = true;
                    // check how many doors it can/should have
                    int min = 0; int max = numberOfDoors-doorsPlaced;
                    if (numberOfDoors - doorsPlaced > 0 && last)
                        min = 1;
                    if (max < 0)
                        max = 0;
                    if (max > 3)
                        max = 3;
                    // now check the surroundings of the adjacent rooms so we know how many doors we can create there
                    int[] doors3 = { 0, 1, 2, 3 };
                    int X = x; int Y = y;
                    switch (direction)
                    {
                        case 0:
                            // we moved up boyz
                            Y = y + 1;
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
                                Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 1:
                            Y = y - 1;
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
                                Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 2:
                            X = x + 1;
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
                                Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        case 3:
                            X = x - 1;
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
                                Debug.Log("FATAL ERROR MIN > MAX");
                            }
                            break;
                        default: break;
                    }
                    setupRoom(X, Y, rand1(min, max), generation);
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
                if (j == (int)max_y / 2 && i == (int)max_x / 2)
                    room[i, j].floor = RoomClass.FloorType.first;
                if(room[i,j].status == RoomClass.Status.exists)
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
