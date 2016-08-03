using UnityEngine;
using System.Collections;
using System;


// Targetable Aura Rune that floats in the play space. 
public class TargetableAura : Targetable {

    public GameObject AuraTemplate;

    // Called when a spell collides with the aura rune.
    public override void processHex(Hex h) {
        // Deal damage.
        ApplyDamage(h.Damage);
        // Destroy the hex.
        h.Destroy();
        // Process if the Aura Rune health is low enough.
        if (IsDead()) {
            // Give the ControlEntity the aura
            h.Source.Aura = AuraTemplate;
            Destroy(gameObject);
        }
    }





}
