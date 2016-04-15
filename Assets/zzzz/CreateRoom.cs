using UnityEngine;
using System.Collections;

public class CreateRoom : MonoBehaviour {

    public GameObject Dungeon;
    public GameObject RoomHolder;
    public GameObject floor;

    // walls
    public GameObject wallHorizontalWithDoor;
    public GameObject wallVerticalWithDoor;

    public GameObject wallHorizontal;
    public GameObject wallVertical;

    private GameObject thisDungeon;

	// Use this for initialization
	void Start () {
        thisDungeon = (GameObject)Instantiate(Dungeon, Vector3.zero, Quaternion.identity);
        thisDungeon.name = "Dungeon";
    }
	

    public void createRoom(int x, int y, RoomClass room)
    {
        if (room.status == RoomClass.Status.empty)
            return;
        x *= 60;
        y *= 36;

        // Create GameObject Holder
        GameObject thisRoom = (GameObject)Instantiate(RoomHolder, new Vector3(x, y, 0), Quaternion.identity);
        thisRoom.transform.parent = thisDungeon.transform;
        thisRoom.name = "Room (" + x + ", " + y + ")";

        // Create the floor
        if (room.status == RoomClass.Status.empty)
            return;
        GameObject thisFloor = (GameObject)Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity);
        thisFloor.transform.parent = thisRoom.transform;
        if (room.floor == RoomClass.FloorType.normal)
        {
            thisFloor.GetComponent<SpriteRenderer>().color = new Color32(0xCD, 0x71, 0x5D, 0xFF);
            thisFloor.name = room.floor.ToString();
        }



        // Create the walls
        GameObject thisNorthWall;
        GameObject thisSouthWall;
        GameObject thisEastWall;
        GameObject thisWestWall;
        
        // North
        if (room.lockNorth == RoomClass.DoorLock.open)
            thisNorthWall = (GameObject)Instantiate(wallHorizontalWithDoor, new Vector3(x, y + 16, 0), Quaternion.identity);
        else
            thisNorthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y + 16, 0), Quaternion.identity);
        
        // South
        if (room.lockSouth == RoomClass.DoorLock.open)
            thisSouthWall = (GameObject)Instantiate(wallHorizontalWithDoor, new Vector3(x, y - 16, 0), Quaternion.identity);
        else
            thisSouthWall = (GameObject)Instantiate(wallHorizontal, new Vector3(x, y - 16, 0), Quaternion.identity);

        // East
        if (room.lockEast == RoomClass.DoorLock.open)
            thisEastWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x + 28, y, 0), Quaternion.identity);
        else
            thisEastWall = (GameObject)Instantiate(wallVertical, new Vector3(x + 28, y, 0), Quaternion.identity);

        // West
        if (room.lockWest == RoomClass.DoorLock.open)
            thisWestWall = (GameObject)Instantiate(wallVerticalWithDoor, new Vector3(x - 28, y, 0), Quaternion.identity);
        else
            thisWestWall = (GameObject)Instantiate(wallVertical, new Vector3(x - 28, y, 0), Quaternion.identity);

        // Place them inside the Holder
        thisNorthWall.transform.parent = thisRoom.transform;
        thisSouthWall.transform.parent = thisRoom.transform;
        thisEastWall.transform.parent = thisRoom.transform;
        thisWestWall.transform.parent = thisRoom.transform;
    }
    

}
