using UnityEngine;
using System.Collections;
using VRTK;

// This script belongs to both controllers of a player and deals with controller events/casting.
public class CastListener : MonoBehaviour {

    // Prefab of the castagon to spawn.
	public GameObject castagonTemplate;

    // Stores the castigon of a player.
	private Castagon instantiatedCastagon;

    // Pass the AI so we can kick off the start task and have a target for the aim assist
    public GameObject ai;

    // Color for the wand light if it is aimed properly.
	public Color GoodColorLerp; 

    // Color for the wand light if it is in the wrong direction.
    public Color BadColorLerp;

	// Cache player on start up to avoid unneeded traversals of the heiarchy tree. 
	private Player player;

	void Start () {

        // Controllers must have VRTK_ControllerEvents attached or the listener will not work properly.
		if (GetComponent<VRTK_ControllerEvents> () == null) {
			Debug.LogError ("VRTK_ControllerEvents_ListenerExample is required to be attached to a SteamVR Controller that has the VRTK_ControllerEvents script attached to it");
			return;
		}

		// Register controller event listeners.
		GetComponent<VRTK_ControllerEvents> ().TriggerPressed += new ControllerInteractionEventHandler (DoTriggerPressed);
		GetComponent<VRTK_ControllerEvents> ().TriggerReleased += new ControllerInteractionEventHandler (DoTriggerReleased);
		GetComponent<VRTK_InteractGrab> ().ControllerGrabInteractableObject += new ObjectInteractEventHandler (ObjectGrabbed);
		GetComponent<VRTK_InteractGrab> ().ControllerUngrabInteractableObject += new ObjectInteractEventHandler (ObjectReleased);

        // Cache player.
		this.player = gameObject.GetComponentInParent<Player> ();
	}

	void ObjectGrabbed(object sender, ObjectInteractEventArgs e) {
		// If player grabs the wand, make set the collider to trigger so it won't collide.
		if (e.target.tag == "Wand") {
			e.target.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}

	void ObjectReleased(object sender, ObjectInteractEventArgs e) {
		// This event will fire with a force drop, so we re-enable the trigger on the collider so physics will work properly.
		if (e.target.tag == "Wand") {
			e.target.GetComponent<BoxCollider> ().isTrigger = false;
            // Remove the queued player spell.
            player.queuedSpell = null;
            // Remove a castagon if there is one.
            if (instantiatedCastagon != null) instantiatedCastagon.destroy();
		}
	}


	void DoTriggerReleased(object sender, ControllerInteractionEventArgs e) {
        // If we have a castagon in the world, destroy it.
		if (instantiatedCastagon != null) {
			instantiatedCastagon.destroy ();
		}

        // Check if the player has a spell queued for firing.
		if (player.queuedSpell != null) {
            if(!player.HoldingWand(this.gameObject)) return;

			GameObject wand = gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject();
            // Disable light on the wand.
			wand.GetComponentInChildren<Light>().intensity = 0;
            // Get the angle between the controller-wand vector and the controller-ai vector.
			float angle = Vector3.Angle (wand.transform.position - gameObject.transform.position, ai.transform.position - gameObject.transform.position);
            // Run the shooting check on the queued hex.
			if (player.CanShoot(player.queuedSpell, gameObject)) {
                // Cast the hex from the wand launch point with random accuracy modifiers.
				player.CastHex (player.queuedSpell, wand.transform.Find("WandLaunchPoint").position, new Vector3 (
					Random.Range(-1, 1) * Random.Range(angle/2, angle)/15, 
					Random.Range(-1, 1) * Random.Range(angle/2, angle)/15, 
					Random.Range(-1, 1) * Random.Range(angle/2, angle)/15
				) + ai.transform.position);

                // Reset the queued spell. This will also stop the check in the FixedUpdate() method.
				player.queuedSpell = null;
			}
		}
	}

	void FixedUpdate() {
		if (player.queuedSpell != null) {
            if (player.HoldingWand(this.gameObject)) {
                GameObject wand = gameObject.GetComponent<VRTK_InteractGrab>().GetGrabbedObject();
                // Get the angle between the controller-wand vector and the controller-ai vector.
                float angle = Vector3.Angle(wand.transform.position - gameObject.transform.position, ai.transform.position - gameObject.transform.position);
                // Adjust the color of the wand light based on the accuracy of the direction of the wand
                wand.GetComponentInChildren<Light>().color = Color.Lerp(GoodColorLerp, BadColorLerp, Mathf.InverseLerp(0, 180, angle));
                // Set the intensity of the wand light to maxmimum. TODO: potentially change this later if the color lerping feels weird
                wand.GetComponentInChildren<Light>().intensity = 8;
            }
		}
	}
		
	void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
	{

		if (player.HoldingWand(this.gameObject)) {
            // Create a castagon from the template and spawn it at the CastagonPoint child of the wand. Set the x and y Euler angle values but not the z angle to avoid unwanted rotating of the castagon.
			instantiatedCastagon = (Instantiate (castagonTemplate, gameObject.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("CastagonPoint").position, Quaternion.Euler (new Vector3 (gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, 0f))) as GameObject).GetComponent<Castagon> ();
            // Set the player of the castagon in the castagon instance.
            instantiatedCastagon.player = gameObject.GetComponentInParent<Player> ();
		}
	}
}
