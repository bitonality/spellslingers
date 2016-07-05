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

	}

	void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
	{
		Debug.Log("Controller on index '" + index + "' " + button + " has been " + action 
			+ " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
	}

	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{
		DebugLogger(e.controllerIndex, "TRIGGER", "pressed down", e);
	
	}


	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
	{
		DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
	
		GameObject go = this.gameObject;
		Spell hex = new Hex ();
		hex.cast (go);

}
}
