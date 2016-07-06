using UnityEngine;
using System.Collections;
using VRTK;

public class Disarm : Hex {


	public double SETTING_DAMAGE = 5;
	public double SETTING_VELOCITY = 2;



	public override void playerCollide (GameObject playerCameraRig)
	{
		//Disarm wand regardless of hand
		VRTK_InteractGrab[] controllers = playerCameraRig.GetComponentsInChildren<VRTK_InteractGrab>();
		foreach (VRTK_InteractGrab controller in controllers) {
			GameObject wand = controller.GetGrabbedObject ();
			controller.ForceRelease();
			wand.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 5, 0));
		}
	}

	// Use this for initialization
	void Start () {
		damage = SETTING_DAMAGE;
		velocity = SETTING_VELOCITY;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
