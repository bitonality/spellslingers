using UnityEngine;
using System.Collections;
using System;

public class Targetable : ControlEntity {


    // Called when a spell collides with a Player.
    public override void processHex(Hex h)
    {



        // Deal damage.
        this.Health -= h.Damage;
        // Destroy the hex.
        h.Destroy();
        // Process if the player is dead.
        if (this.IsDead())
            Destroy(this.gameObject);
    }

    public override bool CanShoot(Hex h, GameObject launchPoint)
    {
        throw new NotImplementedException();
    }
}
