using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : ControlEntity {

	public GameObject slider;

	public override void CastHex (Hex hex, GameObject source, Vector3 target){
		//Finds the wand in a roundabout way... Change later
		hex.gameObject.transform.position = source.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.position;
		hex.gameObject.GetComponent<Rigidbody> ().AddForce (source.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild("WandLaunchPoint").transform.forward.normalized * (float) hex.velocity);
		//Destroy (hex, hex.timeout);

	}

	public override void processHex(Hex h) {
		h.playerCollide (gameObject);
		this.health -= h.damage;
		h.destroy ();
		Debug.Log ("Player health: " + health);
		if (this.IsDead ())
			Destroy (this.gameObject);
	}

	public override bool CanShoot (Hex h, GameObject controller) {
		if (controller != null) {
			if (controller.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == null) return false;
		}

		if (this.cooldown.ContainsKey (h.name)) {
			if (Time.time >= this.cooldown[h.name]) {
				slider.GetComponent<Slider> ().minValue = 0;
				slider.GetComponent<Slider> ().maxValue = h.cooldown;
				slider.GetComponent<Slider> ().value = slider.GetComponent<Slider> ().minValue;
				this.cooldown.Remove (h.name);
				return true;
			} else {
				return false;
			}
		} else {
			this.cooldown.Add (h.name, Time.time + h.cooldown) ;
			slider.GetComponent<Slider> ().minValue = 0;
			slider.GetComponent<Slider> ().maxValue = h.cooldown;
			slider.GetComponent<Slider> ().value = slider.GetComponent<Slider> ().minValue;
			return true;
		}
	}


	// Use this for initialization
	void Start () {
		cooldown = new System.Collections.Generic.Dictionary<string, float> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		foreach(KeyValuePair<string, float> spell in cooldown) {
			Slider s = slider.GetComponent<Slider> ();
			if (Time.time >= spell.Value) {
					s.value += Time.deltaTime;
				}
			}
	
	}




}
