using UnityEngine;
using System.Collections;
using VRTK;

public class HomingProjectile : MonoBehaviour {

    private Transform target;
	private float sensitivity;
    private float ConstantMagnitude;
	private bool update = false;

	public void LaunchProjectile(Hex hex, Transform source, Transform target, float sensitivity, float controllerMagnitude)
    {
        this.target = target;
		this.sensitivity = sensitivity;
		gameObject.GetComponent<Rigidbody>().AddForce(source.forward * (float) controllerMagnitude * hex.velocity);
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
