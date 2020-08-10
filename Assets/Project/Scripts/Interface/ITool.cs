using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITool
{
    void Shoot(float damage, float ammoCost, float ammoMultiplier);
    void Reload();
    void DeductAmmo(float deductAmount);
    void AddAmmo(float addAmount);

    void ApplyKnockback(Rigidbody player, float force);
    ToolType GetToolType();

    BTool GetToolBase();

}
