using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {


	//out of 100
	public double damage;

	//speed multiplier
	public double velocity;

	//projectile that will spawn once the spell is cast
	private GameObject projectile;


	public override void cast (GameObject castController) {
		if ( castController == null)
			return;
		GameObject projectile = (GameObject)Instantiate(Resources.Load("Hex"));
		//Finds the wand in a roundabout way... Change later
		projectile.transform.position = castController.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.position;
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

	public abstract void playerCollide (GameObject playerCameraRig);

	//Initialization (to avoid NPE)
	void Awake() {

	}

	//Change to update later for input event
	void FixedUpdate() {

	}








}
