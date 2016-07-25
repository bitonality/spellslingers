using UnityEngine;
using System.Collections;
using VRTK;

public class CastListener : MonoBehaviour {
	public GameObject castagonTemplate;
	private Castagon instantiatedCastagon;
	//public DPad dpad;
	public GameObject ai;

	//cached player on start up
	private Player player;



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

		this.player = gameObject.GetComponentInParent<Player> ();
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

		if (player.queuedSpell != null) {
			GameObject wand = gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject();
			float angle = Vector3.Angle (wand.transform.position - gameObject.transform.position, ai.transform.position - gameObject.transform.position);
			float accuracy = angle / 360;
			bool canshoot = player.CanShoot (player.queuedSpell, gameObject);
			if (canshoot) {
				player.CastHex (player.queuedSpell, wand.transform.Find("WandLaunchPoint").position, new Vector3 (Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy)));
				player.queuedSpell = null;
			}
		}
	}

	void FixedUpdate() {
		if (player.queuedSpell != null) {
			GameObject wand = gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject();
			float angle = Vector3.Angle (wand.transform.position - gameObject.transform.position, ai.transform.position - gameObject.transform.position);
			wand.GetComponentInChildren<Light> ().color = Color.Lerp (new Color (255, 0, 255), new Color (255, 0, 0), angle / 360);
		}
	}
		
	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		if (gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject () != null) {
			instantiatedCastagon = (Instantiate (castagonTemplate, gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("CastagonPoint").position, Quaternion.Euler (new Vector3 (gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, 0f))) as GameObject).GetComponent<Castagon> ();
			instantiatedCastagon.player = gameObject.GetComponentInParent<Player> ();
		}

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
