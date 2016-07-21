using UnityEngine;
using System.Collections;
using VRTK;

public class CastListener : MonoBehaviour {
	public GameObject castagonTemplate;
	private Castagon instantiatedCastagon;
	public DPad dpad;
	public GameObject leftTemplate;
	public GameObject rightTemplate;
	public GameObject ai;

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
		//	ai.GetComponent<ai> ().StartAi ();
		}
	}

	void ObjectReleased(object sender, ObjectInteractEventArgs e) {
		//If player drops the wand or is forced to drop it, enable collision
		if (e.target.tag == "Wand") {
			e.target.GetComponent<BoxCollider> ().isTrigger = false;
		}
	}



	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
		if (instantiatedCastagon != null) {
			instantiatedCastagon.destroy ();
		}
	}
		
	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		if(gameObject.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() != null)
			instantiatedCastagon = (Instantiate (castagonTemplate, gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject().transform.FindChild ("CastagonPoint").position, gameObject.transform.rotation) as GameObject).GetComponent<Castagon>();


		/*
		Player p = gameObject.GetComponentInParent<Player> ();
		DPad_Direction? direction = DPad.GetButtonPressed (SteamVR_Controller.Input ((int)e.controllerIndex));
		GameObject newProjectile = null;
		switch (direction) {
		case DPad_Direction.LEFT:
			if (p.CanShoot (leftTemplate.GetComponent<Hex> (), gameObject)) {
				newProjectile = Instantiate<GameObject> (leftTemplate) as GameObject;	
				p.CastHex (newProjectile.GetComponent<Hex> (), gameObject, new Vector3 (0, 0, 0));
			}
			break;	
		case DPad_Direction.RIGHT:
			bool canshoot = p.CanShoot (rightTemplate.GetComponent<Hex> (), gameObject);
			if (canshoot) {
				newProjectile = Instantiate<GameObject> (rightTemplate) as GameObject;
				p.CastHex (newProjectile.GetComponent<Hex> (), gameObject, new Vector3 (0, 0, 0));
			}
			break;
		default:
			return;
		}
		*/
	}
}
