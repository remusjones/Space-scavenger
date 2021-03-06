﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BDock : MonoBehaviour, IDock
{
    [SerializeField]
    float m_magnitizeValue = 1f;
    [SerializeField]
    float m_dockingDistance = 10f;

    [SerializeField]
    bool ignoreMagnitise = false;
    private BDock m_dockTarget = null;
    IEnumerator dockingRoutine = null;
    [SerializeField]
    Rigidbody rb = null;

   

    public bool CanDock(Vector3 location, float checkDistance, Transform closestDock)
    {
        if (Vector3.Distance(location, closestDock.position) > checkDistance)
            return false;
        return true;
    }
    public void HandleMagnatise(Rigidbody rigidbody, Transform target)
    {
        if (CanDock(rigidbody.transform.position, m_dockingDistance, target))
        {
            // get direction of push
            Vector3 dir = target.position - this.transform.position;
            rigidbody.AddForce((dir * m_magnitizeValue) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    IEnumerator DockingLoop(Transform otherObject, Rigidbody rigidbody)
    {
        while(m_dockTarget)
        {
            HandleMagnatise(rigidbody, otherObject);

            // exits, and waits.. 
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine(dockingRoutine);
        yield return null;
    }
    public void HandleMagnatiseOther(Rigidbody otherRigidbody, Transform ourTransform)
    {

        Vector3 dir = ourTransform.position - otherRigidbody.transform.position;
        Vector3 otherForward = otherRigidbody.transform.forward;
        otherRigidbody.AddRelativeTorque(Vector3.Cross(this.transform.forward, otherForward) * Time.deltaTime, ForceMode.Acceleration);
        otherRigidbody.AddForce((dir * m_magnitizeValue) * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
    IEnumerator DockingLoopOther(Transform thisObject, Rigidbody otherRigidbody)
    {
        while (m_dockTarget)
        {
            HandleMagnatiseOther(otherRigidbody, thisObject);

            // exits, and waits.. 
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine(dockingRoutine);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        BDock tOtherDock = other.GetComponent<BDock>();
        if (tOtherDock != null)
        {
            m_dockTarget = tOtherDock;
            if (ignoreMagnitise == true)
            {
                dockingRoutine = DockingLoopOther(transform, tOtherDock.rb);
                StartCoroutine(dockingRoutine);
                return;
            }      
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BDock tOtherDock = other.GetComponent<BDock>();
        if (tOtherDock != null)
        {
            StopAllCoroutines();
            m_dockTarget = null;
        }
    }


}
