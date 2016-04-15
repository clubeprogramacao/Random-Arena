using UnityEngine;
using System.Collections;

public class CreateDungeon : MonoBehaviour {


    public GameObject floor;

    public enum RoomType
    {
        normal, boss, treasure, shop, hidden, empty, forcedEmpty
    }

    public int max_x;
    public int max_y;

    public RoomClass[,] room;

	// Use this for initialization
	void Start ()
    {
        setupRoomsArray();

        setupFirstRoom();

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
            }
        }
    }
    
    void setupFirstRoom()
    {
        // room[4, 2] = RoomType.normal;
        RoomClass first = room[4, 2];
        first.floor = RoomClass.FloorType.first;
        first.status = RoomClass.Status.exists;

        // open at least 2 doors
        int[] door = { 1, 2, 3, 4 };
        int doorsToOpen = rand1(2, 4);
        for (int i = 0; i < doorsToOpen; i++ )
        {
            int pick = rand2(door);
            switch (pick)
            {
                case 1: // North
                    if(first.lockNorth == RoomClass.DoorLock.none)
                    {
                        first.lockNorth = RoomClass.DoorLock.open;
                        door = removeNumber(door, 1);
                    }
                    break;
                case 2: // South
                    if (first.lockSouth == RoomClass.DoorLock.none)
                    {
                        first.lockSouth = RoomClass.DoorLock.open;
                        door = removeNumber(door, 2);
                    }
                    break;
                case 3: // East
                    if (first.lockEast == RoomClass.DoorLock.none)
                    {
                        first.lockEast = RoomClass.DoorLock.open;
                        door = removeNumber(door, 3);
                    }
                    break;
                case 4: // West
                    if (first.lockWest == RoomClass.DoorLock.none)
                    {
                        first.lockWest = RoomClass.DoorLock.open;
                        door = removeNumber(door, 4);
                    }
                    break;
                default:
                    break;
            }
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
