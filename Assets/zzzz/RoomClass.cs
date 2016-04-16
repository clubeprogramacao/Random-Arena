using UnityEngine;
using System.Collections;

public class RoomClass{

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

    public DoorFrame frameNorth = DoorFrame.none;
    public DoorFrame frameSouth = DoorFrame.none;
    public DoorFrame frameEast  = DoorFrame.none;
    public DoorFrame frameWest  = DoorFrame.none;

    public DoorLock lockNorth = DoorLock.none;
    public DoorLock lockSouth = DoorLock.none;
    public DoorLock lockEast  = DoorLock.none;
    public DoorLock lockWest  = DoorLock.none;

    public FloorType floor = FloorType.normal;
    public RoomType type = RoomType.normal;
    public Status status = Status.empty;

    public bool adjacentNorth = false;
    public bool adjacentSouth = false;
    public bool adjacentEast = false;
    public bool adjacentWest = false;

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
        if(newRoomType == RoomType.boss)
        {
            type = RoomType.boss;
            thisframe = DoorFrame.bones;
        }
        if (newRoomType == RoomType.hidden)
        {
            type = RoomType.hidden;
            thisframe = DoorFrame.none;
        }
        if (newRoomType == RoomType.normal)
        {
            type = RoomType.normal;
            thisframe = DoorFrame.wood;
        }
        if (newRoomType == RoomType.shop)
        {
            type = RoomType.shop;
            thisframe = DoorFrame.wood;
        }
        if (newRoomType == RoomType.treasure)
        {
            type = RoomType.treasure;
            thisframe = DoorFrame.gold;
        }

        if (adjacentNorth)
        {
            frameNorth = thisframe;
        }
        if (adjacentSouth)
        {
            frameSouth = thisframe;
        }
        if (adjacentEast)
        {
            frameEast = thisframe;
        }
        if (adjacentWest)
        {
            frameWest = thisframe;
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
