using UnityEngine;
using System.Collections;
using System;

public class TargetableGeneric : Targetable {
    public override void processHex(Hex h) {
        // Deal damage.
        ApplyDamage(h.Damage);
        // Destroy the hex.
        h.Destroy();
        // Process if the Aura Rune health is low enough.
        if (this.IsDead()) {
            Destroy(this.gameObject);
        }
    }
}
