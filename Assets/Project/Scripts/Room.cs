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
    [SerializeField]
    float rigidBodyEjectionForce = 3f;
    int layer = (1 << 0);

    IEnumerator breach;
    public List<Room> GetConnectedRooms(Room room, Door ignoreDoor)
    {
        List<Door> openDoors = GetOpenDoors(room);
        List<Room> connectedRooms = new List<Room>();
        foreach(Door door in openDoors)
        {
            if (door == ignoreDoor)
            {
                continue;
            }

            foreach(Room otherRoom in door.m_connectedRooms)
            {
                if (otherRoom != room)
                {
                    if (connectedRooms.Contains(otherRoom))
                        continue;

                    connectedRooms.Add(otherRoom);
                    connectedRooms.AddRange(otherRoom.GetConnectedRooms(otherRoom,door));
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
                    connectedRooms.Add(otherRoom);
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
        int size = 0;
        List<Room> otherRooms = GetConnectedRooms(room);
        size = otherRooms.Count + 1;
        foreach (Room otherRoom in otherRooms)
        {
            if (otherRoom == room)
                continue;
            sharedOxygen += otherRoom.currentOxygen;
        }
        return sharedOxygen / size;
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
                    if (rooms.isSpace) // check for damage here
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
        if (room.isSpace)
            return;

        List<Room> rooms = GetConnectedRooms(room);
        int roomCount = rooms.Count + 1;
        float total = room.currentOxygen;
        bool foundBreach = false;
        Room breachedRoom = null;
        if (foundBreach)
        {
            breachedRoom = room;
        }
        
        foreach(Room otherRoom in rooms)
        {
            total += otherRoom.currentOxygen;
     

            if (foundBreach == false)
            {
                if (otherRoom.isRoomBreached(otherRoom))
                {
                    Debug.Log("Breach found: " + otherRoom.name);
                    foundBreach = true;
                    breachedRoom = otherRoom;
                }
            }
        }

        if (foundBreach)
        {
            // start oxygen evacuation measures . .
            breach = HandleOxygenBreach(breachedRoom, rooms);
            StartCoroutine(breach);
        }
        else
        {
            if (breach != null)
            {
                StopCoroutine(breach);
                breach = null;
            }
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

        Debug.Log("Oxygen: " + oxygenRemaining);
        while(oxygenRemaining > 0)
        {  
            // seep oxygen
            oxygenRemaining -= Time.fixedDeltaTime;
            int otherRoomsCount = otherRooms.Count + 1;
            foreach(Room room in otherRooms)
            {
      
                room.currentOxygen = oxygenRemaining;

                if (room.currentOxygen < 0f)
                    room.currentOxygen = 0f;
            }
            // get nearby objects .. 
            // pull player, pull other objects, etc to the breach .. 
            Collider[] col = Physics.OverlapSphere(targetRoom.transform.position, 20, layer);
            
            foreach(Collider otherCol in col)
            {
                Rigidbody rb = otherCol.GetComponent<Rigidbody>();
                if (rb)
                {
                    Debug.Log("Breached room is: " + targetRoom.name);
                    rb.AddForce((((targetRoom.transform.position  - rb.transform.position)) * rigidBodyEjectionForce) * Time.fixedDeltaTime, ForceMode.Impulse);
                }
            }
            
            this.currentOxygen = oxygenRemaining;
            if (currentOxygen <= 0f)
                currentOxygen = 0f;


            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    private void OnDrawGizmos()
    {
        foreach(Door door in m_doors)
        {

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, door.transform.position);

        }
        if (this.isSpace)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(this.transform.position, Vector3.one / 4f);
            return;
        }
        foreach (Room room in m_connectedRooms)
        {
            
            if (!room.isSpace)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, room.transform.position);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(this.transform.position, Vector3.one / 4f);

        UnityEditor.Handles.Label(this.transform.position, "Oxygen: " + currentOxygen);

    }
}
