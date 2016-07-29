using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {


    // String name of the hex.
	public string HexName;

	// Seconds to delay the destruction of the Hex.
	public float Timeout;

	// How much damage to deal to the target.
	public float Damage;

	// Velocity of the spell.
	public float Velocity;

    // The future scheduled destruction of the hex. We store this so we can cancel it if we manually destroy a hex through the hex.destroy() code.
    private IEnumerator ScheduledDestroy;


    // We can define a custom destruction behavior in hex sub-classes if we need.
    public virtual void BehavioralDestroy() {
        Destroy(this.gameObject);
    }

    // This method should be directly called when we want to forcibly destroy a spell.
	public void Destroy() {
        // If our scheduled destruction hasn't happened yet, cancel it because we are going to manually destroy it.
        if(ScheduledDestroy != null) {
            StopCoroutine(this.ScheduledDestroy);
        }

        // Call the destroy method that potentially has added behavior.
        this.BehavioralDestroy();
	}

    // Schedules a destruction
    public void ScheduleDestroy(float seconds) {
        this.ScheduledDestroy = DestroyInFuture(seconds);
        StartCoroutine(this.ScheduledDestroy);
    }

    private IEnumerator DestroyInFuture(float seconds) {
            yield return new WaitForSeconds(seconds);
            this.Destroy();
    }

    public abstract void playerCollide(GameObject playerCameraRig);
	public abstract void aiCollide(GameObject aiBody);
}
