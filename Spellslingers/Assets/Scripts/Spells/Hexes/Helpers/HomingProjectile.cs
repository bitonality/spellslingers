using UnityEngine;
using System.Collections;
using VRTK;

public class HomingProjectile : MonoBehaviour {

    public GameObject Target {
        get;
        set;
    }
	public float Sensitivity {
        get;
        set;
    }
    private float ConstantMagnitude;
	private bool update = false;

	public void LaunchProjectile(Hex hex, GameObject source, GameObject target, float sensitivity, float controllerMagnitude)
    {
        if(source == null || target == null) {
            return;
        }

        this.Target = target;
        if (target.GetComponent<Targetable>() == null) {
            this.Target = target.GetComponentInParent<Targetable>().gameObject;
        }
        
		this.Sensitivity = sensitivity;
		gameObject.GetComponent<Rigidbody>().AddForce(source.transform.forward * (float) controllerMagnitude * hex.Velocity);
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
            if (this.Target == null) {
                if (this.gameObject.GetComponent<Hex>().Source.CurrentTarget() == null) {
                    return;
                } else {
                    this.Target = this.gameObject.GetComponent<Hex>().Source.CurrentTarget();
                }
            } else {
                Quaternion rotation = Quaternion.LookRotation(Target.GetComponent<Targetable>().TargetPoint.position - transform.position);
                Quaternion adjustedRotation = Quaternion.Slerp(transform.rotation, rotation, Sensitivity * Time.deltaTime);
                gameObject.GetComponent<Rigidbody>().MoveRotation(adjustedRotation);
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward.normalized * ConstantMagnitude;
            }
		}
	}
}
