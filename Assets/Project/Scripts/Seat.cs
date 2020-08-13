using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Seat : MonoBehaviour,IObjectInteract
{
    public Transform _SeatAnchor;
    public Transform _ExitAnchor;
    public UnityEvent _SeatEnterEvent;
    public UnityEvent _SeatExitEvent;
    public GameObject _CurrentPlayer;
    public PlayerController _Player;

    private float exitCooldown = 0.2f;
    private bool isReady = true;
    IEnumerator WaitAfterEvent(float t)
    {
        isReady = false;
        yield return new WaitForSeconds(t);
        isReady = true;
    }

    public void Interact(PlayerController player)
    {
        if (isReady)
            EnterSeat(player);
    }
    public void EnterSeat(PlayerController player)
    {
        _Player = player;
        _Player._Seated = true;
        _Player._Seat = this;
        _Player._Canvas.SetActive(false);
        _Player.transform.SetPositionAndRotation(_SeatAnchor.position, _SeatAnchor.rotation);
        _Player.transform.SetParent(transform);
        _Player.rb.isKinematic = true;
        _CurrentPlayer = player.gameObject;
        StartCoroutine(WaitAfterEvent(exitCooldown));
        _SeatEnterEvent.Invoke();
    }
    public void ExitSeat(PlayerController player)
    {

        _Player = player;
        _Player._Canvas.SetActive(true);
        _Player.transform.SetPositionAndRotation(_ExitAnchor.position, _ExitAnchor.rotation);
        _Player._Seated = false;
        _Player._Seat = null;
        _Player.rb.isKinematic = false;
        _Player.transform.SetParent(null);
        StartCoroutine(WaitAfterEvent(exitCooldown));
        _SeatExitEvent.Invoke();
        MatchVelocity(FindObjectOfType<Ship>().GetComponent<Rigidbody>());
    }
    public void EnablePlayer(bool enabled){ _CurrentPlayer.SetActive(enabled); }
    public void MatchVelocity(Rigidbody VelocityToMatch) 
    {
        //Debug.Log("PLAYER BEFORE VELOCITY" + _Player.rb.velocity);
        
        _Player.rb.velocity = VelocityToMatch.velocity;
        _Player.rb.angularVelocity = VelocityToMatch.angularVelocity;
        Debug.Log("PLAYER After VELOCITY" + _Player.rb.velocity);
        Debug.Log("SHIP Velocity" + VelocityToMatch.velocity);

    }

}
