using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : BTool
{
    [SerializeField]
    private ParticleSystem projectileEffect = null;
    [SerializeField]
    private ParticleSystem muzzleFlash = null;
    public override IEnumerator ShootCoroutine(float fireRate)
    {

        if (isShootCoroutineRunning || !CanShoot(1))
            yield return null;


        isShootCoroutineRunning = true;
        OnShootEvent();
        Shoot(damage, 1f, 0);
        yield return new WaitForSeconds(fireRate);
        isShootCoroutineRunning = false;
        yield return null;
    }
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Use this in input events
    /// </summary>
    public override void OnPrimaryInputDown()
    {
        if (this.CanShoot(1f) && this.enabled)
        {
            if (isShootCoroutineRunning)
                return;
            else
            {
                onShootCoroutine = ShootCoroutine(fireRate);
                StartCoroutine(onShootCoroutine);
            }
        }
    }
    public override void OnReloadInput()
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
