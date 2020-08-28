using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIRunner : MonoBehaviour,IDamageable
{
    public Transform _Target;
    public NavMeshAgent _Nav;
    public GameObject _Mesh;
    public float _NormalSpeed;
    public float _NormalAngularSpeed;
    public float _MeshLinkSpeed;
    public float _MeshLinkAngularSpeed;
    public Animator _Anim;

    public Rigidbody _Rb;

    
    private bool jumpCooldown = false;
    public void Damage(float damage)
    {
        _Rb.isKinematic = false;
        Destroy(_Nav);
        Destroy(_Anim);
        _Rb.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator JumpAtTarget()
    {
        jumpCooldown = true;

        _Rb.isKinematic = false;
        _Rb.AddForce((_Target.transform.position - this.transform.position), ForceMode.Impulse);
        _Nav.isStopped = true;
        yield return new WaitForSeconds(3f);

        jumpCooldown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (_Target == null)
        {
            if(FindObjectOfType<PlayerController>() != null)
            {
                _Target = FindObjectOfType<PlayerController>().transform;
            }
            else
            {
                return;
            }
        }
        if (!jumpCooldown)
        {
            if (_Nav.isOnNavMesh)
            {
                _Nav.destination = _Target.position;
                _Nav.isStopped = false;
                _Rb.isKinematic = true;
                NavMeshPath path = new NavMeshPath();
                bool success = _Nav.CalculatePath(_Target.position, path);
                if (_Nav.pathStatus == NavMeshPathStatus.PathInvalid || !success)
                {
                    if (!jumpCooldown)
                        StartCoroutine(JumpAtTarget());
                }
            }
        }

        if (_Nav.isOnOffMeshLink)
        {
            _Nav.speed = _MeshLinkSpeed;
            _Nav.angularSpeed = _MeshLinkAngularSpeed;
        }
        else
        {
            _Nav.speed = _NormalSpeed;
            _Nav.angularSpeed = _NormalAngularSpeed;
        }

   

        //_Nav.autoTraverseOffMeshLink = false;

        //if (_Nav.isOnOffMeshLink)
        //{
        //    if(_Mesh.transform.position != _Nav.currentOffMeshLinkData.endPos)
        //    {
        //        _Mesh.transform.position = Vector3.Lerp(_Mesh.transform.position, _Nav.currentOffMeshLinkData.endPos, 1);
        //    }
        //    else
        //    {
        //        _Mesh.transform.localPosition = Vector3.zero;
        //        _Nav.CompleteOffMeshLink();
        //    }
        //}


    }

}
