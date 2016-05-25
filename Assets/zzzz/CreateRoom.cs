using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CreateRoom : NetworkBehaviour {

    public GameObject Dungeon;
    public GameObject RoomHolder;
    public GameObject[] floor;
    public GameObject spawn;

    // walls
    public GameObject wallHorizontalWithDoor;
    public GameObject wallVerticalWithDoor;

    public GameObject wallHorizontal;
    public GameObject wallVertical;

    public GameObject[] doorFrame;
    public GameObject doorHole;

    public GameObject thisDungeon;

	// Use this for initialization
	void createDungeon () {
        thisDungeon = (GameObject)Instantiate(Dungeon, Vector3.zero, Quaternion.identity);
        thisDungeon.name = "Dungeon";
        NetworkServer.Spawn(thisDungeon);
    }


    public void createRoom(int x, int y, RoomClass room)
    {
        x *= 60;
        y *= 36;
        thisDungeon = GameObject.Find("Dungeon");
        if(thisDungeon == null)
        {
            createDungeon();
        }

        // Create GameObject Holder
        GameObject thisRoom = (GameObject)Instantiate(RoomHolder, new Vector3(x, y, 0), Quaternion.identity);
        thisRoom.name = "Room (" + x / 60 + ", " + y / 36 + ")";
        thisRoom.transform.SetParent(thisDungeon.transform);

        NetworkServer.Spawn(thisRoom);
        
        // Create the floor
        GameObject thisFloor;
        if (room.floor == RoomClass.FloorType.normal)
        {
            thisFloor = (GameObject)Instantiate(floor[0], new Vector3(x,y,0),Quaternion.identity);
        }
        else if (room.floor == RoomClass.FloorType.first)
        {
            thisFloor = (GameObject)Instantiate(floor[1], new Vector3(x, y, 0), Quaternion.identity);
        }
        else if (room.floor == RoomClass.FloorType.boss)
        {
            thisFloor = (GameObject)Instantiate(floor[2], new Vector3(x, y, 0), Quaternion.identity);
        }
        else if (room.floor == RoomClass.FloorType.dark)
        {
            thisFloor = (GameObject)Instantiate(floor[3], new Vector3(x, y, 0), Quaternion.identity);
        }
        else if (room.floor == RoomClass.FloorType.secret)
        {
            thisFloor = (GameObject)Instantiate(floor[4], new Vector3(x, y, 0), Quaternion.identity);
        }
        else
        {
            thisFloor = (GameObject)Instantiate(floor[5], new Vector3(x, y, 0), Quaternion.identity);
            Debug.Log("floor Exception -> " + room.floor.ToString());
        }
        showValues(room, thisFloor.GetComponent<Text>());
        thisFloor.name = room.floor.ToString();
        thisFloor.transform.SetParent(thisRoom.transform);
        NetworkServer.Spawn(thisFloor);

       

        if (room.status == RoomClass.Status.empty || room.status == RoomClass.Status.blocked)
            return;

        // Create the walls, the doors and the frames
        GameObject thisNorthWall, thisNorthBorder;
        GameObject thisSouthWall, thisSouthBorder;
        GameObject thisEastWall, thisEastBorder;
        GameObject thisWestWall, thisWestBorder;

        // North
        if (room.adjacentNorth)
        {
            thisNorthWall = (GameObject)Instantiate(wallHorizontalWithDoor, new Vector3(x, y + 16, 0), Quaternion.identity);
            if (room.frameNorth == RoomClass.DoorFrame.wood)
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[0], new Vector3(x, y + 16, 0), Quaternion.identity);
            }
            else if (room.frameNorth == RoomClass.DoorFrame.bones)
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[1], new Vector3(x, y + 16, 0), Quaternion.identity);
            }
            else if (room.frameNorth == RoomClass.DoorFrame.gold)
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[2], new Vector3(x, y + 16, 0), Quaternion.identity);
            }
            else if (room.frameNorth == RoomClass.DoorFrame.none)
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[3], new Vector3(x, y + 16, 0), Quaternion.identity);
            }
            else if (room.frameNorth == RoomClass.DoorFrame.spikes)
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[4], new Vector3(x, y + 16, 0), Quaternion.identity);
            }
            else
            {
                thisNorthBorder = (GameObject)Instantiate(doorFrame[5], new Vector3(x, y + 16, 0), Quaternion.identity);
                Debug.Log("northBorder Exception -> " + room.frameNorth.ToString());
            }
                thisNorthBorder.transform.SetParent(thisRoom.transform);
                NetworkServer.Spawn(thisNorthBorder);
        }
        else {
            thisNorthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y + 16, 0), Quaternion.identity);
        }

        // South
        if (room.adjacentSouth)
        {
            thisSouthWall = (GameObject)Instantiate(wallHorizontalWithDoor, new Vector3(x, y - 16, 0), Quaternion.identity);
            if (room.frameSouth == RoomClass.DoorFrame.wood)
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[0], new Vector3(x, y - 16, 0), Quaternion.identity);
            }
            else if (room.frameSouth == RoomClass.DoorFrame.bones)
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[1], new Vector3(x, y - 16, 0), Quaternion.identity);
            }
            else if (room.frameSouth == RoomClass.DoorFrame.gold)
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[2], new Vector3(x, y - 16, 0), Quaternion.identity);
            }
            else if (room.frameSouth == RoomClass.DoorFrame.none)
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[3], new Vector3(x, y - 16, 0), Quaternion.identity);
            }
            else if (room.frameSouth == RoomClass.DoorFrame.spikes)
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[4], new Vector3(x, y - 16, 0), Quaternion.identity);
            }
            else
            {
                thisSouthBorder = (GameObject)Instantiate(doorFrame[5], new Vector3(x, y - 16, 0), Quaternion.identity);
                Debug.Log("southBorder Exception -> " + room.frameSouth.ToString());
            }
            thisSouthBorder.transform.SetParent(thisRoom.transform);
            NetworkServer.Spawn(thisSouthBorder);
        }
        else {
            thisSouthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y - 16, 0), Quaternion.identity);
        }

        // East
        if (room.adjacentEast)
        {
            thisEastWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x + 28, y, 0), Quaternion.identity);
            if (room.frameEast == RoomClass.DoorFrame.wood)
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[0], new Vector3(x + 28, y, 0), Quaternion.identity);
            }
            else if (room.frameEast == RoomClass.DoorFrame.bones)
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[1], new Vector3(x + 28, y, 0), Quaternion.identity);
            }
            else if (room.frameEast == RoomClass.DoorFrame.gold)
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[2], new Vector3(x + 28, y, 0), Quaternion.identity);
            }
            else if (room.frameEast == RoomClass.DoorFrame.none)
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[3], new Vector3(x + 28, y, 0), Quaternion.identity);
            }
            else if (room.frameEast == RoomClass.DoorFrame.spikes)
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[4], new Vector3(x + 28, y, 0), Quaternion.identity);
            }
            else
            {
                thisEastBorder = (GameObject)Instantiate(doorFrame[5], new Vector3(x + 28, y, 0), Quaternion.identity);
                Debug.Log("southBorder Exception -> " + room.frameEast.ToString());
            }
            thisEastBorder.transform.SetParent(thisRoom.transform);
            NetworkServer.Spawn(thisEastBorder);
        }
        else {
            thisEastWall = (GameObject)Instantiate(wallVertical, new Vector3(x + 28, y, 0), Quaternion.identity);
        }
        // West
        if (room.adjacentWest)
        {
            thisWestWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x - 28, y, 0), Quaternion.identity);
            if (room.frameWest == RoomClass.DoorFrame.wood)
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[0], new Vector3(x - 28, y, 0), Quaternion.identity);
            }
            else if (room.frameWest == RoomClass.DoorFrame.bones)
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[1], new Vector3(x - 28, y, 0), Quaternion.identity);
            }
            else if (room.frameWest == RoomClass.DoorFrame.gold)
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[2], new Vector3(x - 28, y, 0), Quaternion.identity);
            }
            else if (room.frameWest == RoomClass.DoorFrame.none)
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[3], new Vector3(x - 28, y, 0), Quaternion.identity);
            }
            else if (room.frameWest == RoomClass.DoorFrame.spikes)
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[4], new Vector3(x - 28, y, 0), Quaternion.identity);
            }
            else
            {
                thisWestBorder = (GameObject)Instantiate(doorFrame[5], new Vector3(x - 28, y, 0), Quaternion.identity);
                Debug.Log("southBorder Exception -> " + room.frameWest.ToString());
            }
            thisWestBorder.transform.SetParent(thisRoom.transform);
            NetworkServer.Spawn(thisWestBorder);
        }
        else {
            thisWestWall = (GameObject)Instantiate(wallVertical, new Vector3(x - 28, y, 0), Quaternion.identity);
        }

        // Place them inside the Holder
        // walls
        thisNorthWall.transform.SetParent(thisRoom.transform);
        thisSouthWall.transform.SetParent(thisRoom.transform);
        thisEastWall.transform.SetParent(thisRoom.transform);
        thisWestWall.transform.SetParent(thisRoom.transform);
        // doors

        // frames



        //NetworkServer.Spawn(thisNorthWall);
        //NetworkServer.Spawn(thisSouthWall);
        //NetworkServer.Spawn(thisEastWall);
        //NetworkServer.Spawn(thisWestWall);
    }

    void showValues(RoomClass room, Text theText)
    {
        theText.text = "status: " + room.status.ToString() + '\n' +
                                                "type: " + room.type.ToString() + '\n' +
                                                "floor: " + room.floor.ToString() + '\n' +
                                                "Numbre of adj rooms: " + room.adjacentRooms().ToString() + '\n' + '\n' +

                                                "lockNorth: " + room.lockNorth.ToString() + '\n' +
                                                "lockSouth: " + room.lockSouth.ToString() + '\n' +
                                                "lockEast: " + room.lockEast.ToString() + '\n' +
                                                "lockWest: " + room.lockWest.ToString() + '\n' + '\n' +

                                                "frameNorth: " + room.frameNorth.ToString() + '\n' +
                                                "frameSouth: " + room.frameSouth.ToString() + '\n' +
                                                "frameEast: " + room.frameEast.ToString() + '\n' +
                                                "frameWest: " + room.frameWest.ToString() + '\n' + '\n' +

                                                "adjacentNorth: " + room.adjacentNorth.ToString() + '\n' +
                                                "adjacentSouth: " + room.adjacentSouth.ToString() + '\n' +
                                                "adjacentEast: " + room.adjacentEast.ToString() + '\n' +
                                                "adjacentWest: " + room.adjacentWest.ToString() + '\n';

    }
}
