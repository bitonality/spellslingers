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

    // The rotation of the object from the previous FixedUpdate.
    private Quaternion LastRotation;

    // Max rotation the game object can move before being destroyed.
    public float MaxRotation {
        get;
        set;
    }

    // Represents the shooter of the hex.
    public ControlEntity Source {
        get;
        set;
    }

    // The future scheduled destruction of the hex. We store this so we can cancel it if we manually destroy a hex through the hex.destroy() code.
    private IEnumerator ScheduledDestroy;


    // We can define a custom destruction behavior in hex sub-classes if we need.
    public virtual void BehavioralDestroy() {
        Destroy(gameObject);
    }

    // This method should be directly called when we want to forcibly destroy a spell.
	public void Destroy() {
        // If our scheduled destruction hasn't happened yet, cancel it because we are going to manually destroy it.
        if(ScheduledDestroy != null) {
            StopCoroutine(ScheduledDestroy);
        }

        // Call the destroy method that potentially has added behavior.
        BehavioralDestroy();
	}

    // Schedules a destruction
    public void ScheduleDestroy(float seconds) {
        ScheduledDestroy = DestroyInFuture(seconds);
        StartCoroutine(ScheduledDestroy);
    }

    private IEnumerator DestroyInFuture(float seconds) {
            yield return new WaitForSeconds(seconds);
        Destroy();
    }


    void FixedUpdate() {
        // The following code is used to avoid spiraling issues. If a hex has to make more than 180 degrees of total correction then we destroy it.
        if (LastRotation != null) {
            // If we've rotated an absurd amount
            if (Quaternion.Angle(this.gameObject.transform.rotation, LastRotation) > MaxRotation) {
                BehavioralDestroy();
            }
        }
         
    }

    public virtual void Start() {
        LastRotation = this.gameObject.transform.rotation;
        MaxRotation = 90;
    }


    public abstract void playerCollide(GameObject playerCameraRig);
	public abstract void aiCollide(GameObject aiBody);


    
}
