using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoor
{
    bool isDoorOpen();
    List<Room> getConnectedRooms();
}
