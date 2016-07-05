using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public class Hex : Spell {


	//out of 100
	double damage;

	//speed multiplier
	double velocity = 2;

	//projectile that will spawn once the spell is cast
	GameObject projectile;


	public override void cast (GameObject castController) {
		GameObject projectile = (GameObject)Instantiate(Resources.Load("Hex"));
		projectile.transform.position = castController.transform.position;
		projectile.GetComponent<Rigidbody> ().velocity = castController.transform.GetComponent<Rigidbody> ().velocity;
		projectile.GetComponent<Rigidbody> ().angularVelocity = castController.transform.GetComponent<Rigidbody> ().angularVelocity;
		launch (castController, projectile);
	}

	private void launch(GameObject controller, GameObject projectile) {
		SteamVR_TrackedObject trackedObj = controller.GetComponent<SteamVR_TrackedObject> ();
		Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
		if (origin != null) {
			
			projectile.GetComponent<Rigidbody>().velocity = origin.TransformVector ((SteamVR_Controller.Input((int)trackedObj.index).velocity) * (float) velocity );
			projectile.GetComponent<Rigidbody>().angularVelocity = origin.TransformVector (SteamVR_Controller.Input((int)trackedObj.index).angularVelocity);
		} else {
			projectile.GetComponent<Rigidbody> ().velocity = (SteamVR_Controller.Input ((int)trackedObj.index).velocity) * (float) velocity;
			projectile.GetComponent<Rigidbody> ().angularVelocity = SteamVR_Controller.Input ((int)trackedObj.index).angularVelocity;

		}
	}


	//Initialization (to avoid NPE)
	void Awake() {

	}

	//Change to update later for input event
	void FixedUpdate() {

	}








}
