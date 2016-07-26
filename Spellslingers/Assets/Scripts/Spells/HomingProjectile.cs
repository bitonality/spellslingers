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
		this.ConstantMagnitude = h.velocity;
        this.target = target;
		this.sensitivity = sensitivity;
		h.gameObject.GetComponent<Rigidbody> ().AddForce (controller.sqrMagnitude * h.velocity * c.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild("WandLaunchPoint").transform.forward);
		update = true;

    }

	void FlipUpdate() {
		update = true;
	}


    void FixedUpdate ()
	{
		if (update) {
			Vector3 relativePosition = target.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation (relativePosition);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, sensitivity);
			Vector3 translation = Vector3.forward * ConstantMagnitude * Time.deltaTime;
			transform.Translate (translation);	

		}
	}
}
