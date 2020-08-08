using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityGun : BTool
{

    [SerializeField]
    Transform anchorPoint = null;
    private Rigidbody grabbedRigidbody = null;
    private bool holdItem = false;
    [SerializeField]
    private float grabModifier = 1f;
    [SerializeField]
    private bool throwItem = false;

    [SerializeField]
    private float throwForce = 200f;

    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        if (anchorPoint == null)
        {
            Debug.LogError(this + " needs an anchor point");
            this.gameObject.SetActive(false);
        }
        camera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("test");
            OnInputToggle();

        }
        if (grabbedRigidbody != null)
            weaponLineRenderer.enabled = true;
        else
            weaponLineRenderer.enabled = false;

        if (Input.GetMouseButtonDown(1))
        {
            if (grabbedRigidbody)
                throwItem = true;
        }
    }

    public void OnInputToggle()
    {
        holdItem = !holdItem;

        if (holdItem)
        {
            Shoot(0f, 0f, 0f);
        }
    }
    public override void Shoot(float damage, float ammoCost, float ammoMultiplier = 0)
    {
        //base.Shoot(damage, ammoCost, ammoMultiplier);
        RaycastHit hit;
        if (Physics.Raycast(weaponNozzle.position, weaponNozzle.forward, out hit, range))
        {
            IGrabbable grabbable = hit.collider.GetComponent<IGrabbable>();
            if (grabbable == null)
                return;

            grabbedRigidbody = grabbable.GetRigidbody();
            //Rigidbody otherRigidbody = 

            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, hit.point };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
            particleToPlayOnCollisionRayHit.transform.position = hit.transform.position;


           // if (!particleToPlayOnCollisionRayHit.isPlaying)
           // {
           //     particleToPlayOnCollisionRayHit.Play(true);
           // }
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
        }
        else
        {
            weaponLineRenderer.enabled = true;
            grabbedRigidbody = null;
            if (particleToPlayOnCollisionRayHit.isPlaying)
            {
                particleToPlayOnCollisionRayHit.Stop(true);
                foreach (Light light in particleToPlayOnCollisionRayHit.GetComponentsInChildren<Light>())
                {
                    light.enabled = false;
                }
            }
            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, weaponNozzle.position + (weaponNozzle.forward * range) };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
        }
    }

    private void FixedUpdate()
    {
        if (holdItem && grabbedRigidbody)
        {
            weaponLineRenderer.SetPosition(0, weaponNozzle.position);
            weaponLineRenderer.SetPosition(1, grabbedRigidbody.position);
            if (Vector3.Distance(grabbedRigidbody.position, anchorPoint.position) > 1f)
            {
                grabbedRigidbody.AddForce(((anchorPoint.position - grabbedRigidbody.transform.position) * Time.fixedDeltaTime) * grabModifier, ForceMode.Impulse);
            }
            else
            {
                grabbedRigidbody.MovePosition(anchorPoint.position);
                grabbedRigidbody.velocity = Vector3.zero;
            }

            if (throwItem)
            {
                grabbedRigidbody.AddForce(weaponNozzle.forward * throwForce, ForceMode.Impulse);
                grabbedRigidbody = null;
                throwItem = false;
            }
        }
    }

}
