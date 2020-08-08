using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldTool : BTool
{

    [SerializeField]
    protected LineRenderer weaponLineRenderer = null;
    [SerializeField]
    protected ParticleSystem particleToPlayOnCollisionRayHit = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weaponLineRenderer.enabled = true;
            Shoot(damage, 1f, 1f * Time.deltaTime);
            ApplyKnockback(playerRigidbody, playerKnockback);
        }
        else
        {
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
        //if (Input.GetMouseButtonDown(1))
        //{
        //    weaponLineRenderer.enabled = true;
        //    Shoot(damage, 5f);
        //    ApplyKnockback(playerRigidbody, playerKnockback);
        //}



        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }


    public override void Shoot(float damage, float ammoCost, float ammoMultiplier = 0.0f)
    {
        if (!CanShoot(ammoCost))
            return;

        base.Shoot();

        // do projectile/raycast here
        RaycastHit hit;
        if (Physics.Raycast(weaponNozzle.position, weaponNozzle.forward, out hit, range))
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
            Vector3[] vecs = new Vector3[2] { weaponNozzle.position, weaponNozzle.position + (weaponNozzle.forward * range) };
            if (weaponLineRenderer)
            {
                weaponLineRenderer.SetPositions(vecs);
            }
        }


    }
}
