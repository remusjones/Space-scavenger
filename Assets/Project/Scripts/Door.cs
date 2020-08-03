using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{

    [SerializeField]
    bool m_isDoorOpen = false;
    private bool lastVal = false;
    public List<Room> m_connectedRooms = new List<Room>();

    public List<Room> getConnectedRooms()
    {
        return m_connectedRooms;
    }

    public bool isDoorOpen()
    {
        return m_isDoorOpen;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDoorOpen != lastVal)
        {
            foreach(Room room in m_connectedRooms)
            {
                room.MoveOxygen(room);
            }
            lastVal = m_isDoorOpen;
        }
    }
}
