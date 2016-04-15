using UnityEngine;
using System.Collections;

public class RoomClass{

	public enum DoorFrame
    {
        wood, gold, bones, spikes, none
    }
    public enum DoorLock
    {
        open, combat, key, coin, bomb, none
    }
    public enum FloorType
    {
        normal, dark, boss, secret, first
    }
    public enum Status
    {
        exists, empty
    }

    public DoorFrame frameNorth;
    public DoorFrame frameSouth;
    public DoorFrame frameEast;
    public DoorFrame frameWest;

    public DoorLock lockNorth;
    public DoorLock lockSouth;
    public DoorLock lockEast;
    public DoorLock lockWest;

    public FloorType floor;

    public Status status;

    public bool adjacentNorth;
    public bool adjacentSouth;
    public bool adjacentEast;
    public bool adjacentWest;

    public Vector2 pos;
    
    public RoomClass()
    {
        lockNorth = DoorLock.none;
        lockSouth = DoorLock.none;
        lockEast = DoorLock.none;
        lockWest = DoorLock.none;
    }
    
};
