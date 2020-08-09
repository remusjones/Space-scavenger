using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : BTool
{
    [SerializeField]
    private ParticleSystem projectileEffect = null;
    [SerializeField]
    private ParticleSystem muzzleFlash = null;
    protected override void Start()
    {
        onShootCoroutine = ShootCoroutine(fireRate);
        onReloadCoroutine = ReloadWeaponCoroutine(reloadSpeed);
    }

    public override IEnumerator ShootCoroutine(float fireRate)
    {

        Debug.Log("Coroutine Entered.. ");
        if (isShootCoroutineRunning || !CanShoot(1))
            yield return null;


        Debug.Log("Coroutine Starting.. ");
        isShootCoroutineRunning = true;
        OnShootEvent();
        Shoot(damage, 1f, 0);
        yield return new WaitForSeconds(fireRate);
        isShootCoroutineRunning = false;
        Debug.Log("Coroutine Ending.. ");
        yield return null;
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            ShootWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }
    }

    /// <summary>
    /// Use this in input events
    /// </summary>
    public void ShootWeapon()
    {
        if (isShootCoroutineRunning)
            return;
        else
        {
            onShootCoroutine = ShootCoroutine(fireRate);
            StartCoroutine(onShootCoroutine);
        }
    }
    public void ReloadWeapon()
    {
        if (isReloadCoroutineRunning)
            return;
        else
        {
            onReloadCoroutine = ReloadWeaponCoroutine(reloadSpeed);
            StartCoroutine(onReloadCoroutine);
        }
    }
    public override void Shoot(float damage, float ammoCost, float ammoMultiplier = 0.0f)
    {

        ApplyKnockback(playerRigidbody, playerKnockback);
        base.Shoot(damage, ammoCost, ammoMultiplier);
        muzzleFlash.Play();
        projectileEffect.Play();
        // do projectile/raycast here
        RaycastHit hit;
        if (base.RaycastFromCamera(out hit))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }
    }

}
