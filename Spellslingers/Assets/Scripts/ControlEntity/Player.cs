using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : ControlEntity {

    // Serialized inspector friendly entry for mapping spells to their specific cooldown slider UI element.
	public SliderInsepctorEntry[] InspectorSliders;

    // On startup, the InspectorSliders is loaded into this map for fast lookups of the mapping.
	private Dictionary<string, Slider> Sliders;


	// Represents a properly queued spell from a castagon. Null means no spell queued.
	public Hex queuedSpell {
		get;
		set;
	}

    // Called when a spell collides with a Player.
	public override void processHex(Hex h) {

        // Process the collision with the player.
		h.playerCollide (gameObject);
        // Deal damage.
		this.Health -= h.Damage;
        // Update healthbar UI with new health amount.
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.Health/this.MaxHealth);
        // Destroy the hex.
        h.Destroy();
        // Process if the player is dead.
		if (this.IsDead ())
			Destroy (this.gameObject);
	}

	public override bool CanShoot (Hex h, GameObject controller) {
        // If the player is holding their wand.
        if (GetWand(controller) == null) return false;
        // Checks if the spell is in cooldown.
		if (this.cooldown.ContainsKey (h.HexName)) {
			if (Time.time >= this.cooldown[h.HexName]) {
				this.cooldown.Remove (h.HexName);
			} else {
				return false;
			}
		} 

        // Once we are at this point in the code, we know we will be returning true.
        // Assume spell is sucessfully cast, so add the spell to the cooldown.
		this.cooldown.Add (h.HexName, Time.time + h.cooldown) ;
        // Get the cooldown slider associated with the cast hex.
		Slider slider = Sliders [h.HexName];
        // Reset the min and max scale of the slider in case we want to modify cooldown amounts at runtime
		slider.minValue = 0;
		slider.maxValue = h.cooldown;
		slider.value = slider.minValue;
		return true;
	}

    // If the player is holding their wand return the game object, if not, return null.
    // Pass the SteamVR Controller game object to this
    public GameObject GetWand(GameObject controller) {
        if (controller != null) {
            return controller.GetComponent<VRTK_InteractGrab>().GetGrabbedObject();
        }
        return null;
    }

    public Transform ClosestTarget (GameObject wand, GameObject controller) {
        // Start with a worst case scenario 
        float angle = 179;
        Transform best = null;
        foreach(GameObject target in Targets) {
            float test = Vector3.Angle(wand.transform.position - controller.transform.position, target.transform.position - controller.transform.position);
            if(test < angle) {
                angle = test;
                best = target.transform;
            }
        }

        return best;
       
    }

	void Start () {
        // Instantiate the cooldown dictionary.
		cooldown = new System.Collections.Generic.Dictionary<string, float> ();

		// Populate our slider map with values from the inspector.
		Sliders = new Dictionary<string, Slider> ();
		foreach (SliderInsepctorEntry entry in InspectorSliders) {
			Sliders.Add (entry.hex.GetComponent<Hex>().HexName, entry.slider.GetComponent<Slider> ());
		}

        // Initial settting of queuedSpell.
		queuedSpell = null;
	}

	
	void FixedUpdate () {
        // Iterate over all the sliders and update their value 
		foreach(KeyValuePair<string, float> spell in cooldown) {
			if (Time.time <= spell.Value) {
				Sliders [spell.Key].value += Time.fixedDeltaTime;
			}
		}
	}


    // Represents an inspector friendly way to map spells to sliders
	[System.Serializable]
	public class SliderInsepctorEntry {
		public GameObject hex;
		public GameObject slider;
	}





}
