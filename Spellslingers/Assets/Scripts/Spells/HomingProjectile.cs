using UnityEngine;
using System.Collections;
using VRTK;

public class HomingProjectile : MonoBehaviour {

    private Transform target;
	private float sensitivity;
	private float ConstantMagnitude;
	private bool update = false;

	public void LaunchProjectile(Transform target, Hex h, float sensitivity, Vector3 controller, GameObject c)
    {
        this.target = target;
		this.sensitivity = sensitivity;
		h.gameObject.GetComponent<Rigidbody> ().AddForce (controller.magnitude * h.velocity * c.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild("WandLaunchPoint").transform.forward);
        Invoke("FlipUpdate", 0.5F);

    }

	void FlipUpdate() {
		update = true;
        this.ConstantMagnitude = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
	}


    void FixedUpdate ()
	{
		if (update) {

            //  var targetRotation = Quaternion.LookRotation(target.position - transform.position);
             // homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
			Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
            //transform.rotation = Quaternion.Slerp (transform.rotation, rotation, sensitivity);
            gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, 20));

		}
	}
}
