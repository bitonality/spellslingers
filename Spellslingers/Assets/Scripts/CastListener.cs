using UnityEngine;
using System.Collections;
using VRTK;

public class CastListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GetComponent<VRTK_ControllerEvents>() == null)
		{
			Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a SteamVR Controller that has the VRTK_ControllerEvents script attached to it");
			return;
		}

		//Setup controller event listeners
		GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
		GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
		GetComponent<VRTK_InteractGrab> ().ControllerGrabInteractableObject += new ObjectInteractEventHandler (ObjectGrabbed);
		GetComponent<VRTK_InteractGrab> ().ControllerUngrabInteractableObject += new ObjectInteractEventHandler (ObjectReleased);

	}


	void ObjectGrabbed(object sender, ObjectInteractEventArgs e) {
		//If player grabs the wand, make it a kinematic object
		if (e.target.tag == "Wand") {
			e.target.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void ObjectReleased(object sender, ObjectInteractEventArgs e) {
		//If player drops the wand or is forced to drop it, disable the kinematic properties

		if (e.target.tag == "Wand") {
			e.target.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}


	void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
	{
		Debug.Log("Controller on index '" + index + "' " + button + " has been " + action 
			+ " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
	}

	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		//DebugLogger(e.controllerIndex, "TRIGGER", "pressed down", e);
	
	}


	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
	{
		//DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
		Hex hex = new Disarm();
		hex.cast (this.gameObject);

	}
}
