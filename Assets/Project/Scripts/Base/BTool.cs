using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTool : MonoBehaviour, ITool
{

    [Header("Tool References")]
    [SerializeField]
    protected Transform weaponNozzle = null;
    [SerializeField]
    private LineRenderer weaponLineRenderer = null;

    [SerializeField]
    protected Rigidbody playerRigidbody = null;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="ammoCost"></param>
    /// <param name="ammoMultiplier">Leave as 0.0f if you don't want to use it</param>
    public virtual void Shoot(float damage, float ammoCost, float ammoMultiplier = 0.0f)
    {
        if (currentMagazineAmount <= 0 || (currentMagazineAmount - ammoCost ) < 0f)
            return;

        if (ammoMultiplier == 0.0f)
        {
            currentMagazineAmount -= ammoCost;
        }else
        {
            currentMagazineAmount -= (ammoCost * ammoMultiplier);
        }
        // do projectile/raycast here
        RaycastHit hit;
        if (Physics.Raycast(weaponNozzle.position,Vector3.forward, out hit, range))
        {
            Vector3 point = hit.point;
            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, (weaponNozzle.forward * range) };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }else
        {
            
            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, weaponNozzle.position + (weaponNozzle.forward * range) };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
        }


    }

    protected virtual void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weaponLineRenderer.enabled = true;
            Shoot(damage, 1f, 1f * Time.deltaTime);
            ApplyKnockback(playerRigidbody, 1f);
        }
        //if (Input.GetMouseButtonDown(1))
        //{
        //    weaponLineRenderer.enabled = true;
        //    Shoot(damage, 5f);
        //    ApplyKnockback(playerRigidbody, playerKnockback);
        //}
        else
            weaponLineRenderer.enabled = false;


        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void ApplyKnockback(Rigidbody player, float force)
    {
        Vector3 torqueDir = player.transform.position - weaponNozzle.position;
        player.AddForce((-player.transform.forward * force) * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
