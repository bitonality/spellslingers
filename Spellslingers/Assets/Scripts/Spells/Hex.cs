using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {


    // String name of the hex.
	public string HexName;

	// Seconds to delay the destruction of the Hex.
	public float timeout = 5;

	// How much damage to deal to the target.
	public float damage;

	// Velocity of the spell.
	public float velocity;

    // The future scheduled destruction of the hex. We store this so we can cancel it if we manually destroy a hex through the hex.destroy() code.
    private IEnumerator ScheduledDestroy;

	// Default destroy method, can be overridden when appropriate in child spells.
    // This method should be directly called when we want to forcibly destroy a spell.
	public virtual void destroy() {
        if(ScheduledDestroy != null) {
            StopCoroutine(this.ScheduledDestroy);
        }

		Destroy (this.gameObject);
	}

    public void ScheduleDestroy(float seconds) {
        this.ScheduledDestroy = DestroyInFuture(seconds);
        StartCoroutine(this.ScheduledDestroy);
    }

    private IEnumerator DestroyInFuture(float seconds) {
            yield return new WaitForSeconds(seconds);
            this.destroy();
    }

    public abstract void playerCollide(GameObject playerCameraRig);
	public abstract void aiCollide(GameObject aiBody);
}
