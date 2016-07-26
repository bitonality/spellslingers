﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

// Abstraction layer that encapsulates AI and Players.
public abstract class ControlEntity : MonoBehaviour {

    // The health bar UI element tied to the ControlEntity.
	public GameObject HealthBar;
    // Abstract method to process the hex collision.
	public abstract void processHex (Hex h);
    // Abstract method to determine whether the ControlEntity can shoot or not.
	public abstract bool CanShoot(Hex h, GameObject launchPoint);

	// Cooldown dictionary that maps string Spell names to Ta future time when the next spell is castable (Time.time + cooldown).
	public Dictionary<string, float> cooldown {
		get;
		set;
	}

    // Max health value set in the inspector.
	public float MaxHealth;

    // Current health of the player.
    public float Health {
        get;
        set;
    }
   
    // Returns if the player is dead or not.
	public bool IsDead() {
		return(Health <= 0);
	}

    // Instantiates a hex based on a template at source and launches it at target with force modifer forceMod
	public void CastHex (Hex hex, Vector3 source, Vector3 target, float forceMod) {
		Hex proj = Instantiate (hex, source, new Quaternion(0,0,0,0)) as Hex;
		proj.gameObject.GetComponent<Rigidbody> ().AddForce ((target-source).normalized * (float) hex.velocity * forceMod);
	}
}
