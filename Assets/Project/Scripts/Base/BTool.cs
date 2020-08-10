using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField]
    protected float reloadSpeed = 0f;
    [SerializeField]
    protected float fireRate = 0f;


    [Header("Cooldown Values")]
    [SerializeField]
    protected float shootCooldown;
    [SerializeField]
    protected float reloadCooldown;

    [SerializeField]
    protected UnityEvent onShootEvent = null;

    [SerializeField]
    protected UnityEvent onReloadEvent = null;

    protected bool isReloadCoroutineRunning = false;
    protected bool isShootCoroutineRunning = false;

    protected IEnumerator onShootCoroutine = null;
    protected IEnumerator onReloadCoroutine = null;

    protected bool isPrimaryDown = false;
    protected bool isSecondaryDown = false;
    protected virtual void Start()
    {
        onShootCoroutine = ShootCoroutine(fireRate);
        onReloadCoroutine = ReloadWeaponCoroutine(reloadSpeed);
    }

    protected virtual void OnShootEvent()
    {
        onShootEvent?.Invoke();
    }
    protected virtual void OnReloadEvent()
    {
        onReloadEvent?.Invoke();
    }
    public virtual void AddAmmo(float addAmount)
    {
        storedAmmo += addAmount;
    }

    public virtual void DeductAmmo(float deductAmount)
    {
        storedAmmo -= deductAmount;
    }

    public virtual IEnumerator ReloadWeaponCoroutine(float delay)
    {
        if (isReloadCoroutineRunning)
            yield return null;

        isReloadCoroutineRunning = true;
        OnReloadEvent();
        yield return new WaitForSeconds(delay);
        Reload();
        isReloadCoroutineRunning = false;

        yield return null;
    }
    public virtual IEnumerator ShootCoroutine(float fireRate)
    {
        if (isShootCoroutineRunning)
            yield return null;

        isShootCoroutineRunning = true;
        yield return new WaitForSeconds(fireRate);
        OnShootEvent();
        isShootCoroutineRunning = false;

        yield return null;
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

    public virtual void AngleToolToCamera() 
    {
        Vector3 pos = playerCamera.transform.position + (playerCamera.transform.forward * range);
        this.weaponNozzle.parent.LookAt(pos, playerRigidbody.transform.up);
    }
    public virtual bool RaycastFromCamera(out RaycastHit raycastHit)
    {
        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, range, layermask))
        {
            raycastHit = hit;
            return true;
        }
        raycastHit = hit;
        return false;
    }
    public virtual bool RaycastFromToolToCamera(Transform toolNozzle, out RaycastHit raycastHit)
    {
        RaycastHit hit;
        Ray ray = new Ray(toolNozzle.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, range, layermask))
        {
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

        OnShootEvent();
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
        OnShootEvent();
    }

    public virtual void OnPrimaryInputDown() { isPrimaryDown = true; }
    public virtual void OnSecondaryInputDown() { isSecondaryDown = true; }
    public virtual void OnPrimaryInputRelease() { isPrimaryDown = false; }
    public virtual void OnSecondaryInputRelease() { isSecondaryDown = false; }
    public virtual void OnReloadInput() { }
    protected virtual void Update()
    {
    }

    public void ApplyKnockback(Rigidbody player, float force)
    {
        Vector3 torqueDir = player.transform.position - weaponNozzle.position;
        player.AddForce((-player.transform.forward * force) * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
