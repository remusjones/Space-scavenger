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
    public override void UpdateTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnInputToggle();

        }

        if (Input.GetMouseButtonDown(1))
        {
            if (grabbedRigidbody)
                throwItem = true;
        }
    }
    public override string GetPrintable()
    {
        return "Gravity Gun";
    }
    public void OnInputToggle()
    {
        holdItem = !holdItem;

        if (holdItem)
        {
            Shoot();
        }
    }
    public override void Shoot()
    {
        base.Shoot();
        //base.Shoot(damage, ammoCost, ammoMultiplier);
        RaycastHit hit;
        if (base.RaycastFromCamera(out hit))
        {
            IGrabbable grabbable = hit.collider.GetComponent<IGrabbable>();
            if (grabbable == null)
                return;

            grabbedRigidbody = grabbable.GetRigidbody();
        }
    }

    private void FixedUpdate()
    {
        if (holdItem && grabbedRigidbody)
        {
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
