using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : ControlEntity {

	public SliderInsepctorEntry[] InspectorSliders;
	private Dictionary<string, Slider> sliders;

	public override void CastHex (Hex hex, GameObject source, Vector3 target){
		//Finds the wand in a roundabout way... Change later
		hex.gameObject.transform.position = source.GetComponent<VRTK_InteractGrab> ().GetGrabbedObject ().transform.FindChild ("WandLaunchPoint").transform.position;
		hex.gameObject.GetComponent<Rigidbody> ().AddForce (source.GetComponent<VRTK_InteractGrab>().GetGrabbedObject().transform.FindChild("WandLaunchPoint").transform.forward.normalized * (float) hex.velocity);
		//Destroy (hex, hex.timeout);

	}

	public override void processHex(Hex h) {
		h.playerCollide (gameObject);
		this.health -= h.damage;
		//TODO: magic valu
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.health/100);
		h.destroy ();
		if (this.IsDead ())
			Destroy (this.gameObject);
	}

	public override bool CanShoot (Hex h, GameObject controller) {
		if (controller != null) {
			if (controller.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() == null) return false;
		}

		if (this.cooldown.ContainsKey (h.name)) {
			if (Time.time >= this.cooldown[h.name]) {
				this.cooldown.Remove (h.name);
			} else {
				return false;
			}
		} 
			this.cooldown.Add (h.name, Time.time + h.cooldown) ;



		Slider slider = sliders [h.name];
		slider.minValue = 0;
		slider.maxValue = h.cooldown;
		slider.value = slider.minValue;
		return true;
	}


	// Use this for initialization
	void Start () {
		cooldown = new System.Collections.Generic.Dictionary<string, float> ();

		//load our map from the inspector
		sliders = new Dictionary<string, Slider> ();
		foreach (SliderInsepctorEntry entry in InspectorSliders) {
			sliders.Add (entry.name, entry.slider.GetComponent<Slider> ());
		}
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		foreach(KeyValuePair<string, float> spell in cooldown) {
			if (Time.time <= spell.Value) {
				sliders [spell.Key].value += Time.fixedDeltaTime;
			}
		}
	}

	[System.Serializable]
	public class SliderInsepctorEntry {
		public string name;
		public GameObject slider;
	}





}
