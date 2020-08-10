using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldTool : BTool
{

    [SerializeField]
    protected LineRenderer weaponLineRenderer = null;
    [SerializeField]
    protected ParticleSystem particleToPlayOnCollisionRayHit = null;

    
    // Update is called once per frame
    public override void UpdateTool()
    {
        AngleToolToCamera();
        if (isPrimaryDown)
        {
            ShootTool();
            if (!CanShoot(1f * Time.deltaTime))
            {
                OnPrimaryInputRelease();
            }
        }
    }
    public override string GetPrintable()
    {
        return "Weld Tool";
    }
    public void ShootTool()
    {
        weaponLineRenderer.enabled = true;
        Shoot(damage, 1f, 1f * Time.deltaTime);
        
    }

    public override void OnPrimaryInputRelease()
    {
        base.OnPrimaryInputRelease();
        weaponLineRenderer.enabled = false;
        if (particleToPlayOnCollisionRayHit.isPlaying)
        {
            particleToPlayOnCollisionRayHit.Stop(true);


            foreach (Light light in particleToPlayOnCollisionRayHit.GetComponentsInChildren<Light>())
            {
                light.enabled = false;
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
        if (!CanShoot(ammoCost * ammoMultiplier))
        {
            weaponLineRenderer.enabled = false;
            return;
        }

       
        ApplyKnockback(playerRigidbody, playerKnockback);
        base.Shoot(damage,ammoCost,ammoMultiplier);

        // do projectile/raycast here
        RaycastHit hit;
        if (base.RaycastFromCamera(out hit))
        {

            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, hit.point };
            Vector3 backDir = (hit.point - weaponNozzle.position) * 0.01f;

            particleToPlayOnCollisionRayHit.transform.position = hit.point - backDir;
            particleToPlayOnCollisionRayHit.transform.LookAt(playerRigidbody.transform, Vector3.up);


            if (!particleToPlayOnCollisionRayHit.isPlaying)
            {
                particleToPlayOnCollisionRayHit.Play(true);
            }
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }
        else
        {
            if (particleToPlayOnCollisionRayHit.isPlaying)
            {
                particleToPlayOnCollisionRayHit.Stop(true);
                foreach (Light light in particleToPlayOnCollisionRayHit.GetComponentsInChildren<Light>())
                {
                    light.enabled = false;
                }
            }
            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, playerCamera.transform.position + (playerCamera.transform.forward * range) };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
        }
    }
}
