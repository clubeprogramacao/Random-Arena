using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    public GameObject Dungeon;
    public GameObject RoomHolder;
    public GameObject floor;

    // walls
    public GameObject wallHorizontalWithDoor;
    public GameObject wallVerticalWithDoor;

    public GameObject wallHorizontal;
    public GameObject wallVertical;

    public GameObject doorFrame;
    public GameObject doorHole;

    private GameObject thisDungeon;

	// Use this for initialization
	void Start () {
        thisDungeon = (GameObject)Instantiate(Dungeon, Vector3.zero, Quaternion.identity);
        thisDungeon.name = "Dungeon";
    }


    public void createRoom(int x, int y, RoomClass room)
    {
        x *= 60;
        y *= 36;

        // Create GameObject Holder
        GameObject thisRoom = (GameObject)Instantiate(RoomHolder, new Vector3(x, y, 0), Quaternion.identity);
        thisRoom.transform.SetParent(thisDungeon.transform);
        thisRoom.name = "Room (" + x / 60 + ", " + y / 36 + ")";

        // Create the floor

        GameObject thisFloor = (GameObject)Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity);
        thisFloor.transform.SetParent(thisRoom.transform);

        if (room.type == RoomClass.RoomType.normal)
            thisFloor.GetComponent<SpriteRenderer>().color = new Color(0xCD/255f, 0x71/255f, 0x5D/255f, 0xFF/255f);

        if (room.type == RoomClass.RoomType.treasure)
            thisFloor.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 0, 255);
        if(room.type == RoomClass.RoomType.boss)
            thisFloor.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f, 0.5f, 1);
        if (room.status == RoomClass.Status.blocked)
        {
            thisFloor.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        }

        if (room.status == RoomClass.Status.empty)
        {
            thisFloor.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
        if (room.floor == RoomClass.FloorType.first)
        {
            thisFloor.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        thisFloor.name = room.status.ToString();

        showValues(room, thisFloor.GetComponent<Text>());
        

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
            thisNorthBorder = (GameObject)Instantiate(doorFrame, new Vector3(x, y + 16, 0),Quaternion.identity);
            if (room.frameNorth == RoomClass.DoorFrame.gold)
                thisNorthBorder.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            if (room.frameNorth == RoomClass.DoorFrame.bones)
                thisNorthBorder.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            if (room.frameNorth == RoomClass.DoorFrame.wood)
                thisNorthBorder.GetComponent<SpriteRenderer>().color = new Color(0xCD / 255f, 0x71 / 255f, 0x5D / 255f, 0xFF / 255f);
            thisNorthBorder.transform.SetParent(thisRoom.transform);
        }
        else
            thisNorthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y + 16, 0), Quaternion.identity);

        // South
        if (room.adjacentSouth)
        {
            thisSouthWall = (GameObject)Instantiate(wallHorizontalWithDoor, new Vector3(x, y - 16, 0), Quaternion.identity);
            thisSouthBorder = (GameObject)Instantiate(doorFrame, new Vector3(x, y - 16, 0), Quaternion.identity);
            if (room.frameSouth == RoomClass.DoorFrame.gold)
                thisSouthBorder.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            if (room.frameSouth == RoomClass.DoorFrame.bones)
                thisSouthBorder.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            if (room.frameSouth == RoomClass.DoorFrame.wood)
                thisSouthBorder.GetComponent<SpriteRenderer>().color = new Color(0xCD / 255f, 0x71 / 255f, 0x5D / 255f, 0xFF / 255f);
            thisSouthBorder.transform.SetParent(thisRoom.transform);
        }
        else
            thisSouthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y - 16, 0), Quaternion.identity);

        // East
        if (room.adjacentEast)
        {
            thisEastWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x + 28, y, 0), Quaternion.identity);
            thisEastBorder = (GameObject)Instantiate(doorFrame, new Vector3(x + 28, y, 0), Quaternion.identity);
            if (room.frameEast == RoomClass.DoorFrame.gold)
                thisEastBorder.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            if (room.frameEast == RoomClass.DoorFrame.bones)
                thisEastBorder.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            if (room.frameEast == RoomClass.DoorFrame.wood)
                thisEastBorder.GetComponent<SpriteRenderer>().color = new Color(0xCD / 255f, 0x71 / 255f, 0x5D / 255f, 0xFF / 255f);
            thisEastBorder.transform.SetParent(thisRoom.transform);
        }
        else
            thisEastWall = (GameObject)Instantiate(wallVertical, new Vector3(x + 28, y, 0), Quaternion.identity);

        // West
        if (room.adjacentWest)
        {
            thisWestWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x - 28, y, 0), Quaternion.identity);
            thisWestBorder = (GameObject)Instantiate(doorFrame, new Vector3(x - 28, y, 0), Quaternion.identity);
            if (room.frameWest == RoomClass.DoorFrame.gold)
                thisWestBorder.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            if (room.frameWest == RoomClass.DoorFrame.bones)
                thisWestBorder.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            if (room.frameWest == RoomClass.DoorFrame.wood)
                thisWestBorder.GetComponent<SpriteRenderer>().color = new Color(0xCD / 255f, 0x71 / 255f, 0x5D / 255f, 0xFF / 255f);
            thisWestBorder.transform.SetParent(thisRoom.transform);
        }
        else
            thisWestWall = (GameObject)Instantiate(wallVertical, new Vector3(x - 28, y, 0), Quaternion.identity);

        // Place them inside the Holder
        // walls
        thisNorthWall.transform.SetParent(thisRoom.transform);
        thisSouthWall.transform.SetParent(thisRoom.transform);
        thisEastWall.transform.SetParent(thisRoom.transform);
        thisWestWall.transform.SetParent(thisRoom.transform);
        // doors

        // frames
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
