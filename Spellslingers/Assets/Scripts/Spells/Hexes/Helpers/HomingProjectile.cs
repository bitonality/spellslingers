using UnityEngine;
using System.Collections;
using VRTK;

public class HomingProjectile : MonoBehaviour {

    public Transform Target {
        get;
        set;
    }
	public float Sensitivity {
        get;
        set;
    }
    private float ConstantMagnitude;
	private bool update = false;

	public void LaunchProjectile(Hex hex, Transform source, Transform target, float sensitivity, float controllerMagnitude)
    {
        this.Target = target;
		this.Sensitivity = sensitivity;
		gameObject.GetComponent<Rigidbody>().AddForce(source.forward * (float) controllerMagnitude * hex.Velocity);
		Invoke ("FlipUpdate", 0.1F);

    }

	void FlipUpdate() {
		update = true;
        ConstantMagnitude = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
	}



    void FixedUpdate ()
	{
		if (update) {
            // For if we destroy the hex object before the actual gameobject has been destroyed.
            if (this.gameObject == null) return; 
            if(this.Target == null) {
                this.Target = this.gameObject.GetComponent<Hex>().Source.CurrentTarget().transform;
            }
			Quaternion rotation = Quaternion.LookRotation (Target.position - transform.position);
			Quaternion adjustedRotation = Quaternion.Slerp(transform.rotation, rotation, Sensitivity * Time.deltaTime);
            gameObject.GetComponent<Rigidbody>().MoveRotation(adjustedRotation);
			gameObject.GetComponent<Rigidbody>().velocity = transform.forward.normalized * ConstantMagnitude;
		}
	}
}
