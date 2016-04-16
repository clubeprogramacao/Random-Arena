using UnityEngine;
using System.Collections;

public class CreateDungeon : MonoBehaviour {


    public GameObject floor;

    

    public int max_x;
    public int max_y;

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

        foreach (Vector2 pos in setupFirstRoom())
        {
            setupRoom(pos, 0/*rand1(0, 3)*/);
        }

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
                if (i == 0 || i==max_x-1 || j == 0 || j == max_y-1)
                    room[i, j].status = RoomClass.Status.blocked;
            }
        }
    }
    
    Vector2[] setupFirstRoom()
    {
        Vector2 pos = new Vector2((int)max_x/2, (int)max_y/2);
        RoomClass first = room[(int)pos.x, (int)pos.y];
        first.floor = RoomClass.FloorType.first;
        first.status = RoomClass.Status.exists;
        first.type = RoomClass.RoomType.normal;
        setAdjacents((int)pos.x, (int)pos.y);
        // open at least 2 doors
        int[] door = { 1, 2, 3, 4 };
        int doorsToOpen = rand1(2, 4);
        Vector2[] createdRooms = new Vector2[doorsToOpen];
        for (int i = 0; i < doorsToOpen; i++ )
        {
            int pick = rand2(door);
            switch (pick)
            {
                case 1: // North
                    if(first.lockNorth == RoomClass.DoorLock.none)
                    {
                        first.lockNorth = RoomClass.DoorLock.open;
                        first.frameNorth = RoomClass.DoorFrame.wood;
                        room[(int)pos.x, (int)pos.y + 1].status = RoomClass.Status.exists;
                        room[(int)pos.x, (int)pos.y + 1].lockSouth = RoomClass.DoorLock.open;
                        setAdjacents((int)pos.x, (int)pos.y + 1);
                        createdRooms[i] = new Vector2((int)pos.x, (int)pos.y + 1);
                        door = removeNumber(door, 1);
                    }
                    break;
                case 2: // South
                    if (first.lockSouth == RoomClass.DoorLock.none)
                    {
                        first.lockSouth = RoomClass.DoorLock.open;
                        first.frameSouth = RoomClass.DoorFrame.wood;
                        room[(int)pos.x, (int)pos.y - 1].status = RoomClass.Status.exists;
                        room[(int)pos.x, (int)pos.y - 1].lockNorth = RoomClass.DoorLock.open;
                        setAdjacents((int)pos.x, (int)pos.y - 1);
                        createdRooms[i] = new Vector2((int)pos.x, (int)pos.y - 1);
                        door = removeNumber(door, 2);
                    }
                    break;
                case 3: // East
                    if (first.lockEast == RoomClass.DoorLock.none)
                    {
                        first.lockEast = RoomClass.DoorLock.open;
                        first.frameEast = RoomClass.DoorFrame.wood;
                        room[(int)pos.x + 1, (int)pos.y].status = RoomClass.Status.exists;
                        room[(int)pos.x + 1, (int)pos.y].lockWest = RoomClass.DoorLock.open;
                        setAdjacents((int)pos.x + 1, (int)pos.y);
                        createdRooms[i] = new Vector2((int)pos.x + 1, (int)pos.y);
                        door = removeNumber(door, 3);
                    }
                    break;
                case 4: // West
                    if (first.lockWest == RoomClass.DoorLock.none)
                    {
                        first.lockWest = RoomClass.DoorLock.open;
                        first.frameWest = RoomClass.DoorFrame.wood;
                        room[(int)pos.x - 1, (int)pos.y].status = RoomClass.Status.exists;
                        room[(int)pos.x - 1, (int)pos.y].lockEast = RoomClass.DoorLock.open;
                        setAdjacents((int)pos.x - 1, (int)pos.y);
                        createdRooms[i] = new Vector2((int)pos.x - 1, (int)pos.y);
                        door = removeNumber(door, 4);
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
    
    Vector2[] setupRoom(Vector2 pos, int numberOfDoors)
    {
        // This room already has one door. We're adding more
        RoomClass thisRoom = room[(int)pos.x, (int)pos.y];
        setAdjacents((int)pos.x, (int)pos.y);
        switch (numberOfDoors)
        {
            case 0: // Terminal Room
                switch (rand1(0, 2)) // what type of terminal room?
                {
                    case 0: // normal
                        thisRoom.makeType(RoomClass.RoomType.normal);
                        break;
                    case 1: // treasure
                        thisRoom.makeType(RoomClass.RoomType.treasure);
                        changeSurroundingDoors((int)pos.x, (int)pos.y, thisRoom);
                        break;
                    case 2: // boss
                        thisRoom.makeType(RoomClass.RoomType.boss);
                        changeSurroundingDoors((int)pos.x, (int)pos.y, thisRoom);
                        break;
                    case 3: // shop
                        thisRoom.makeType(RoomClass.RoomType.shop);
                        break;
                    case 4: // hidden
                        thisRoom.makeType(RoomClass.RoomType.hidden);
                        break;
                    default: break;
                }
                break;
            case 1: // Corridor
                
                break;
            case 2: // 
            case 3:

                break;
            default: break;
        }


        return new Vector2[0];
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
                GetComponent<CreateRoom>().createRoom(i, j, room[i,j]);
            }
        }
    }
    
    void setAdjacents(int x, int y)
    {
        // North
        room[x, y + 1].adjacentSouth = true;
        // South
        room[x, y - 1].adjacentNorth = true;
        // East
        room[x + 1, y].adjacentWest = true;
        // West
        room[x - 1, y].adjacentEast = true;
    }

    int rand1(int min, int max)
    {
        return Random.Range(min, max+1);
    }

    int rand2(int[] number)
    {
        int index = Random.Range(0, (int)number.GetLength(0));
        return number[index];
    }
    int[] removeNumber(int[] array,int numberToRemove)
    {
        if( System.Array.IndexOf(array, numberToRemove) ==-1)
            return null;
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
