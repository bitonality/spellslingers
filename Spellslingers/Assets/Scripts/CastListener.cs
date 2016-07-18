﻿using UnityEngine;
using System.Collections;
using VRTK;

public class CastListener : MonoBehaviour {
	public DPad dpad;
	public GameObject leftTemplate;
	public GameObject rightTemplate;

	private Vector3 travel;



	// Use this for initialization
	void Start () {
		if (GetComponent<VRTK_ControllerEvents> () == null) {
			Debug.LogError ("VRTK_ControllerEvents_ListenerExample is required to be attached to a SteamVR Controller that has the VRTK_ControllerEvents script attached to it");
			return;
		}

		//Setup controller event listeners
		GetComponent<VRTK_ControllerEvents> ().GripPressed += new ControllerInteractionEventHandler (DoGripPress);
		GetComponent<VRTK_ControllerEvents> ().GripReleased += new ControllerInteractionEventHandler (DoGripRelease);

		GetComponent<VRTK_ControllerEvents> ().TriggerPressed += new ControllerInteractionEventHandler (DoTriggerPressed);
		GetComponent<VRTK_ControllerEvents> ().TriggerReleased += new ControllerInteractionEventHandler (DoTriggerReleased);
		GetComponent<VRTK_InteractGrab> ().ControllerGrabInteractableObject += new ObjectInteractEventHandler (ObjectGrabbed);
		GetComponent<VRTK_InteractGrab> ().ControllerUngrabInteractableObject += new ObjectInteractEventHandler (ObjectReleased);


	}

	void DoGripPress(object sender, ControllerInteractionEventArgs e) {
		
	}

	void DoGripRelease(object sender, ControllerInteractionEventArgs e) {

	}


	void ObjectGrabbed(object sender, ObjectInteractEventArgs e) {
		//If player grabs the wand, make it not collide
		if (e.target.tag == "Wand") {
			e.target.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}

	void ObjectReleased(object sender, ObjectInteractEventArgs e) {
		//If player drops the wand or is forced to drop it, enable collision
		if (e.target.tag == "Wand") {
			e.target.GetComponent<BoxCollider> ().isTrigger = false;
		}
	}



	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
	}
		
	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		DPad_Direction? direction = DPad.GetButtonPressed (SteamVR_Controller.Input ((int)e.controllerIndex));
		GameObject newProjectile = null;
		switch (direction) {
		case DPad_Direction.LEFT:
			newProjectile = Instantiate<GameObject> (leftTemplate) as GameObject;
			break;
		case DPad_Direction.RIGHT:
			newProjectile = Instantiate<GameObject> (rightTemplate) as GameObject;
			break;
		case null:
			return;
		}
			
	
		Player p = gameObject.GetComponentInParent<Player> ();
		if(p.CanShoot(newProjectile.GetComponent<Hex>(), gameObject)) p.CastHex (newProjectile.GetComponent<Hex>(), gameObject, null);

	}



}
