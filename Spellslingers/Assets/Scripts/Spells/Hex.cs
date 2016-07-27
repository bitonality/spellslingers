using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {


    // String name of the hex.
	public string HexName;

	// Seconds to delay the destruction of the Hex.
	public float timeout = 20;

	// How much damage to deal to the target.
	public float damage;

	// Velocity of the spell.
	public float velocity;

	// Default destroy method, can be overridden when appropriate in child spells
	public virtual void destroy() {
		Destroy (this.gameObject);
	}


	public abstract void playerCollide(GameObject playerCameraRig);
	public abstract void aiCollide(GameObject aiBody);
}
