using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {
	//num seconds until clean up
	private float timeout = 20;

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
		gameObject.GetComponent<Rigidbody>().AddForce(controller.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild ("WandLaunchPoint").transform.forward * (float) velocity);
		Destroy (this.gameObject, timeout);
	}

	//Desfault destroy, can be overridden when appropriate in child spells
	public virtual void destroy() {
		Destroy (this.gameObject);
	}

	public abstract void playerCollide (GameObject playerCameraRig);







}
