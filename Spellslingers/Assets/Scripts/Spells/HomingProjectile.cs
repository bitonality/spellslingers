using UnityEngine;
using System.Collections;
using VRTK;

public class HomingProjectile : MonoBehaviour {

    private Transform target;
	private float sensitivity;
	private float ConstantMagnitude;
	private Quaternion LastRotation;
	private bool update = false;

	public void LaunchProjectile(Transform target, Hex h, float sensitivity, Vector3 controller, GameObject c)
    {
		h.gameObject.transform.rotation = c.transform.rotation;
        this.target = target;
		this.sensitivity = 0;
		gameObject.GetComponent<Rigidbody>().AddForce(c.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild ("WandLaunchPoint").transform.forward * (float) controller.magnitude * h.velocity);
		Invoke ("FlipUpdate", 0.1F);

    }

	void FlipUpdate() {
		update = true;
        this.ConstantMagnitude = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

	}



    void FixedUpdate ()
	{
		if (update) {
			Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
			Quaternion adjustedRotation = Quaternion.Slerp(transform.rotation, rotation, 1.5F * Time.deltaTime);
            gameObject.GetComponent<Rigidbody>().MoveRotation(adjustedRotation);
			gameObject.GetComponent<Rigidbody>().velocity = transform.forward.normalized * this.ConstantMagnitude;
		}
	}
}
