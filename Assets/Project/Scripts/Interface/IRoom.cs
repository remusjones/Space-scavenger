using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom
{
    List<Door> GetOpenDoors(Room room);

    List<Room> GetConnectedRooms(Room room, Door ignoreDoor);
    List<Room> GetConnectedRooms(Room room);
    float GetOxygenVolume(Room room);

    bool isRoomBreached(Room room);
    bool isRoomOxygenated(Room room);

    bool isRoomSpace(Room room);

    void MoveOxygen(Room room);

}
