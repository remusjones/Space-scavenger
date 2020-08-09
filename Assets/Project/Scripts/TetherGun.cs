﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherGun : BTool
{

    private Transform anchorPoint = null;
    private Rigidbody grabbedRigidbody = null;
    private bool holdItem = false;
    [SerializeField]
    private float grabModifier = 1f;
    [SerializeField]
    private bool pullItem = false;

    [SerializeField]
    private float moveForce = 200f;
    [SerializeField]
    private float retractForce = 0.5f;
    [SerializeField]
    private LineRenderer lineRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        anchorPoint = new GameObject("AnchorPoint").transform;
        anchorPoint.parent = this.transform;
        anchorPoint.position = this.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetMouseButton(0))
        {


            if (holdItem == false)
                Shoot();

            holdItem = true;
            // OnInputToggle();

        }
        else
        {
            holdItem = false;
            grabbedRigidbody = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (grabbedRigidbody)
                pullItem = true;
            else
                pullItem = false;
        }
        else
            pullItem = false;
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
            anchorPoint.position = grabbedRigidbody.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (holdItem && grabbedRigidbody)
        {
            grabbedRigidbody.AddForce(((((anchorPoint.position - grabbedRigidbody.transform.position)) * Vector3.Distance(anchorPoint.position, grabbedRigidbody.transform.position)) * moveForce) * Time.fixedDeltaTime, ForceMode.Force);
            if (pullItem)
            {
                anchorPoint.position = Vector3.MoveTowards(weaponNozzle.position, anchorPoint.position, Time.fixedDeltaTime * retractForce);
            }

            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            List<Vector3> positions = new List<Vector3>();
            positions.Add(this.weaponNozzle.position);
            positions.Add(this.grabbedRigidbody.position);
            lineRenderer.SetPositions(positions.ToArray());
        }
        else
            lineRenderer.enabled = false;
    }

}
