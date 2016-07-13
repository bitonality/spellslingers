using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {
	//num seconds until clean up
	private float timeout = 20;

	//represents the minimum velocity for the spell to move to be casted
	private double minVelocity = 2;

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
		//We use the quicker squared magnitude call to avoid the expensive sqrt call
		if (origin != null && SteamVR_Controller.Input((int)trackedObj.index).velocity.sqrMagnitude > (damage*damage)) {
			gameObject.GetComponent<Rigidbody>().velocity = origin.TransformVector ((SteamVR_Controller.Input((int)trackedObj.index).velocity) * (float) velocity );
			gameObject.GetComponent<Rigidbody>().angularVelocity = origin.TransformVector (SteamVR_Controller.Input((int)trackedObj.index).angularVelocity);
		} else {
			gameObject.GetComponent<Rigidbody> ().velocity = (SteamVR_Controller.Input ((int)trackedObj.index).velocity) * (float) velocity;
			gameObject.GetComponent<Rigidbody> ().angularVelocity = SteamVR_Controller.Input ((int)trackedObj.index).angularVelocity;

		}

		Destroy (this.gameObject, timeout);
	}

	//Desfault destroy, can be overridden when appropriate in child spells
	public virtual void destroy() {
		Destroy (this.gameObject);
	}

	public abstract void playerCollide (GameObject playerCameraRig);







}
