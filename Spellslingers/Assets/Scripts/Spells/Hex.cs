using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {


	//out of 100
	public double damage;

	//speed multiplier
	public double velocity;

	public override void cast (GameObject castController) {
		if ( castController == null)
			return;
		//Finds the wand in a roundabout way... Change later
		gameObject.transform.position = castController.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.position;
		launch (castController);
	}

	private void launch(GameObject controller) {
		SteamVR_TrackedObject trackedObj = controller.GetComponent<SteamVR_TrackedObject> ();
		Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
		if (origin != null) {
			
			gameObject.GetComponent<Rigidbody>().velocity = origin.TransformVector ((SteamVR_Controller.Input((int)trackedObj.index).velocity) * (float) velocity );
			gameObject.GetComponent<Rigidbody>().angularVelocity = origin.TransformVector (SteamVR_Controller.Input((int)trackedObj.index).angularVelocity);
		} else {
			gameObject.GetComponent<Rigidbody> ().velocity = (SteamVR_Controller.Input ((int)trackedObj.index).velocity) * (float) velocity;
			gameObject.GetComponent<Rigidbody> ().angularVelocity = SteamVR_Controller.Input ((int)trackedObj.index).angularVelocity;

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
