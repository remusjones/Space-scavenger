using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Seat : MonoBehaviour,IObjectInteract
{
    public Transform _SeatAnchor;
    public Transform _ExitAnchor;
    public UnityEvent _SeatEvent;
    public GameObject _CurrentPlayer;
    public void Interact(PlayerController player)
    {
        player._Seated = true;
        player.transform.SetPositionAndRotation(_SeatAnchor.position,_SeatAnchor.rotation);
        player.transform.SetParent(transform);
        player.GetComponent<Rigidbody>().isKinematic = true;
        _CurrentPlayer = player.gameObject;
        _SeatEvent.Invoke();
    }
    public void DisablePlayer()
    {
        _CurrentPlayer.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
