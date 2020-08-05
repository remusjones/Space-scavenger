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
    public void TriggerDoor()
    {
        m_isDoorOpen = !m_isDoorOpen;

        if (!m_isDoorOpen)
            return;
        foreach (Room room in m_connectedRooms)
        {
            room.MoveOxygen(room);
        }
    }
    private void OnDrawGizmos()
    {
        if (m_isDoorOpen)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(this.transform.position, 0.1f);

    }
}
