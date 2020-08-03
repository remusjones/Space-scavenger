using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour, IRoom
{

    [SerializeField]
    List<Door> m_doors = new List<Door>();
    [SerializeField]
    List<Room> m_connectedRooms = new List<Room>();
    public bool isSpace = false;
    [SerializeField]
    float currentOxygen = 1f;
    float oxygenLossRate = 10f;

    bool hasChangedBreached = false;

    public List<Room> GetConnectedRooms(Room room, Door ignoreDoor)
    {
        List<Door> openDoors = GetOpenDoors(room);
        List<Room> connectedRooms = new List<Room>();
        foreach(Door door in openDoors)
        {
            if (door == ignoreDoor)
                continue;

            foreach(Room otherRoom in door.m_connectedRooms)
            {
                if (otherRoom != room)
                {
                    connectedRooms.AddRange(room.GetConnectedRooms(otherRoom,door));
                }
            }
        }
        return connectedRooms;
    }
    public List<Room> GetConnectedRooms(Room room)
    {
        List<Door> openDoors = GetOpenDoors(room);
        List<Room> connectedRooms = new List<Room>();
        foreach (Door door in openDoors)
        {
            foreach (Room otherRoom in door.m_connectedRooms)
            {
                if (otherRoom != room)
                {
                    connectedRooms.AddRange(room.GetConnectedRooms(otherRoom, door));
                }
            }
        }
        return connectedRooms;
    }


    public List<Door> GetOpenDoors(Room room)
    {
        List<Door> openDoors = new List<Door>();
        foreach(Door door in room.m_doors)
        {
            if (door.isDoorOpen())
                openDoors.Add(door);
        }
        return openDoors;
    }




    public float GetOxygenVolume(Room room)
    {
        float sharedOxygen = currentOxygen;
        foreach(Room otherRoom in GetConnectedRooms(room))
        {
            if (otherRoom == room)
                continue;
            sharedOxygen += otherRoom.currentOxygen;
        }
        return sharedOxygen;
    }
    

    public bool isRoomBreached(Room room)
    {
        if (isSpace)
        {
            return true;
        }
       
        List<Door> doors = GetOpenDoors(room);
        foreach(Door door in doors)
        {
            
           if (door.m_connectedRooms.Count > 1)
            {
                foreach(Room rooms in door.m_connectedRooms)
                {
                    if (rooms == room)
                        continue;
                    if (rooms.isSpace || rooms.isRoomBreached(rooms))
                        return true;

                }
            }
        }
        return false;
    }

    public bool isRoomOxygenated(Room room)
    {
        if (room.currentOxygen > 0)
            return true;
        else
            return false;
    }

    public bool isRoomSpace(Room room)
    {
        return room.isSpace;
    }

    public void MoveOxygen(Room room)
    {
        List<Room> rooms = GetConnectedRooms(room);
        int roomCount = rooms.Count;

        float total = room.currentOxygen;
        bool foundBreach = isRoomBreached(room);

        Room breachedRoom = null;
        if (foundBreach)
        {
            breachedRoom = room;
        }
        
        foreach(Room otherRoom in rooms)
        {
            if (otherRoom == room)
                continue;
            total += otherRoom.currentOxygen;

            if (foundBreach == false)
            {
                if (otherRoom.isRoomBreached(otherRoom))
                {
                    foundBreach = true;
                    breachedRoom = otherRoom;
                }
            }
        }

        if (foundBreach)
        {
            // start oxygen evacuation measures . .
            IEnumerator breach = HandleOxygenBreach(breachedRoom, rooms);
            StartCoroutine(breach);
            Debug.LogError("der were breach");
        }
        else
        {
            // distribute -- 
            foreach (Room otherRoom in rooms)
            {
                if (otherRoom == room)
                    continue;
                otherRoom.currentOxygen = total / roomCount;
            }
            room.currentOxygen = total / roomCount;
        }
    }

    IEnumerator HandleOxygenBreach(Room targetRoom, List<Room> otherRooms)
    {
        float oxygenRemaining = GetOxygenVolume(this);
        
        while(oxygenRemaining >= 0)
        {  
            // seep oxygen
            oxygenRemaining -= oxygenLossRate * Time.fixedDeltaTime;
            int otherRoomsCount = otherRooms.Count + 1;
            foreach(Room room in otherRooms)
            {
      
                room.currentOxygen = oxygenRemaining / otherRooms.Count;

                if (room.currentOxygen < 0f)
                    room.currentOxygen = 0f;
            }
            // get nearby objects .. 
            // pull player, pull other objects, etc to the breach .. 
            Collider[] col = Physics.OverlapSphere(targetRoom.transform.position, 20);
            foreach(Collider otherCol in col)
            {
                Rigidbody rb = otherCol.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForce((rb.transform.position - targetRoom.transform.position) * oxygenRemaining, ForceMode.Impulse);
                }
            }
            
            this.currentOxygen = oxygenRemaining / otherRoomsCount;


            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }
}
