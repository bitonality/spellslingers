using UnityEngine;
using System.Collections;
using VRTK;

public class Player : ControlEntity {
	
	public override void CastHex (Hex hex, GameObject source, Vector3 target){
		//Finds the wand in a roundabout way... Change later
		hex.gameObject.transform.position = source.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.position;
		if (target == null) {
			hex.gameObject.GetComponent<Rigidbody> ().AddForce (source.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.forward * (float)hex.velocity);
		} else {
			hex.gameObject.GetComponent<Rigidbody> ().AddForce ((target - source.transform.position) * (float) hex.velocity);
		}
		Destroy (this.gameObject, hex.timeout);
	}

	public override void processHex(Hex h) {
		h.playerCollide (gameObject);
		this.health -= h.damage;
		h.destroy ();
	}

	public override bool CanShoot (Hex h, GameObject wand) {
		if (wand != null) {
			VRTK_InteractGrab controller = wand.GetComponentInParent<VRTK_InteractGrab> ();
			if (controller == null) return false;
		}

		if (this.cooldown.ContainsKey (h)) {
			if (Time.time >= this.cooldown[h] + this.cooldown[h]) {
				return true;
			} else {
				return false;
			}
		} else {
			this.cooldown.Add (h, Time.time);
			return true;
		}
		//always assume the worst
		return false;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
