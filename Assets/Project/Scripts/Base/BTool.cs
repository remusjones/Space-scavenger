using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTool : MonoBehaviour, ITool
{

    [Header("Tool References")]
    [SerializeField]
    protected Transform weaponNozzle = null;

    [SerializeField]
    protected Rigidbody playerRigidbody = null;

    [SerializeField]
    protected Camera playerCamera = null;

    [Header("Tool Values")]
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float playerKnockback;
    [SerializeField]
    protected float magazineAmount;
    [SerializeField]
    protected float currentMagazineAmount;
    [SerializeField]
    protected float storedAmmo;
    [SerializeField]
    protected LayerMask layermask;

    [Header("Cooldown Values")]
    [SerializeField]
    protected float shootCooldown;
    [SerializeField]
    protected float reloadCooldown;



    public virtual void AddAmmo(float addAmount)
    {
        storedAmmo += addAmount;
    }

    public virtual void DeductAmmo(float deductAmount)
    {
        storedAmmo -= deductAmount;
    }

    public virtual void Reload()
    {
        float diff = storedAmmo - magazineAmount;
        if (diff >= 0)
        {
            storedAmmo -= magazineAmount;
            currentMagazineAmount = magazineAmount;
        }else
        {
            if (Mathf.Abs(diff) > magazineAmount)
                return;
            else
            {
                float actual = magazineAmount + diff;
                currentMagazineAmount = actual;
                storedAmmo = 0f;
            }
        }
    }

    public virtual bool RaycastFromCamera(out RaycastHit raycastHit)
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * range);
        if (Physics.Raycast(ray, out hit, range, layermask))
        {
            Debug.Log(hit.collider.gameObject.name);
            raycastHit = hit;
            return true;
        }
        raycastHit = hit;
        return false;
    }
    public bool CanShoot(float ammoCost)
    {
        if (currentMagazineAmount <= 0 || (currentMagazineAmount - ammoCost) < 0f)
            return false;
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="ammoCost"></param>
    /// <param name="ammoMultiplier">Leave as 0.0f if you don't want to use it</param>
    public virtual void Shoot(float damage, float ammoCost, float ammoMultiplier = 0.0f)
    {
        if (ammoMultiplier == 0.0f)
        {
            currentMagazineAmount -= ammoCost;
        }
        else
        {
            currentMagazineAmount -= (ammoCost * ammoMultiplier);
        }
    }
    public virtual void Shoot()
    { 
    }

    protected virtual void Update()
    {
    }

    public void ApplyKnockback(Rigidbody player, float force)
    {
        Vector3 torqueDir = player.transform.position - weaponNozzle.position;
        player.AddForce((-player.transform.forward * force) * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
