using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RoomClass: NetworkBehaviour{
    
	public enum DoorFrame
    {
        wood, gold, bones, spikes, none
    }
    public enum DoorLock
    {
        none, open, combat, key, coin, bomb
    }
    public enum FloorType
    {
        normal, dark, boss, secret, first
    }
    public enum Status
    {
        exists, empty, blocked
    }
    public enum RoomType
    {
        normal, boss, treasure, shop, hidden
    }

    [SyncVar]
    public DoorFrame frameNorth = DoorFrame.none;
    [SyncVar]
    public DoorFrame frameSouth = DoorFrame.none;
    [SyncVar]
    public DoorFrame frameEast  = DoorFrame.none;
    [SyncVar]
    public DoorFrame frameWest  = DoorFrame.none;

    [SyncVar]
    public DoorLock lockNorth = DoorLock.none;
    [SyncVar]
    public DoorLock lockSouth = DoorLock.none;
    [SyncVar]
    public DoorLock lockEast  = DoorLock.none;
    [SyncVar]
    public DoorLock lockWest  = DoorLock.none;

    [SyncVar]
    public FloorType floor = FloorType.normal;
    [SyncVar]
    public RoomType type = RoomType.normal;
    [SyncVar]
    public Status status = Status.empty;

    [SyncVar]
    public bool adjacentNorth = false;
    [SyncVar]
    public bool adjacentSouth = false;
    [SyncVar]
    public bool adjacentEast = false;
    [SyncVar]
    public bool adjacentWest = false;

    [SyncVar]
    public Vector2 pos;
    

    public int adjacentRooms()
    {
        int count = 0;
        if (adjacentNorth)
        {
            count++;
        }
        if (adjacentSouth)
        {
            count++;
        }
        if (adjacentEast)
        {
            count++;
        }
        if (adjacentWest)
        {
            count++;
        }
        return count;
    }
    
    public void makeType(RoomType newRoomType)
    {
        DoorFrame thisframe = DoorFrame.none;
        DoorLock thislock = DoorLock.open;

        if(newRoomType == RoomType.boss)
        {
            type = RoomType.boss;
            floor = FloorType.boss;
            thisframe = DoorFrame.bones;
            thislock = DoorLock.combat;

        }
        if (newRoomType == RoomType.hidden)
        {
            type = RoomType.hidden;
            floor = FloorType.secret;
            thisframe = DoorFrame.none;
            thislock = DoorLock.bomb;
        }
        if (newRoomType == RoomType.normal)
        {
            type = RoomType.normal;
            floor = FloorType.normal;
            thisframe = DoorFrame.wood;
        }
        if (newRoomType == RoomType.shop)
        {
            type = RoomType.shop;
            floor = FloorType.normal;
            thisframe = DoorFrame.wood;
            thislock = DoorLock.key;
            
        }
        if (newRoomType == RoomType.treasure)
        {
            type = RoomType.treasure;
            floor = FloorType.normal;
            thisframe = DoorFrame.gold;
            thislock = DoorLock.open;
        }

        if (adjacentNorth)
        {
            frameNorth = thisframe;
            lockNorth = thislock;
        }
        if (adjacentSouth)
        {
            frameSouth = thisframe;
            lockSouth = thislock;
        }
        if (adjacentEast)
        {
            frameEast = thisframe;
            lockEast = thislock;
        }
        if (adjacentWest)
        {
            frameWest = thisframe;
            lockWest = thislock;
        }
    }

    public void makeFrames(DoorFrame newFrame, int[] door)
    {
        for(int i = 0; i< door.Length; i++) {
            switch(door[i])
            {
                case 0: // North
                    frameNorth = newFrame;
                    break;
                case 1: // South
                    frameSouth = newFrame;
                    break;
                case 2: // East
                    frameEast = newFrame;
                    break;
                case 3: // West
                    frameWest = newFrame;
                    break;
                default: break;
            }
        }
    }
};
