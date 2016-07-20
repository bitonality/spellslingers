﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class ai : ControlEntity {
	//For stacking of effects that disable shooting
	public float ShootingCycleDisable = 0;

	//Speed it moves at (m/s)
	public float speed;

	private float defaultSpeed;

	//Level is 1/2/3, set somewhere else but set here for now.
	//Unused for right now.
	//private static int level = 1;

	//Used for movement (so it's unpredictable)
	private System.Random rnd = new System.Random ();

	//Hex and spells are in their own respective array so they can be in the inspector. Then, they're placed into a hashtable.
	public GameObject[] hexes;

	void Start () {
		/* 
		 * This doesn't work, for MVP leaving in the hardcode.
		//Convert gameobject to string 
		for (int i = 0; i < spookyFactor.Length; i++) {
			spookyStrings.Add(spookyFactor [i].GetComponent<Hex> ().name);
		}
		for (int i = 0; i < spookyStrings.Count; i++) {
			Debug.Log("Item " + i + ": " + spookyStrings[i]);
		}
		*/
		//Record the default speed
		defaultSpeed = speed;
		//Start the shooting loop
		foreach (GameObject currentHex in hexes) {
			Debug.Log (currentHex.name);
			StartCoroutine(spellTimer((((GameObject) currentHex).GetComponent<Hex>()), (float) currentHex.GetComponent<Hex>().cooldown));
		}

		cooldown = new System.Collections.Generic.Dictionary<string, float> ();

		//Once every 0.33 seconds, check if the AI is in danger
		InvokeRepeating ("checkSafety", 0F, 0.33F);
	}

	public void setSpeed(float newSpeed) {
		if (newSpeed >= 0) {
			speed = newSpeed;
		} else {
			speed = defaultSpeed;
		}
	}

	public float getDefaultSpeed() {
		return defaultSpeed;
	}

	//TODO: Bad assumption kys
	void OnTriggerExit(Collider col) {
		gameObject.GetComponent<Rigidbody> ().velocity *= -1;
	}

	IEnumerator spellTimer (Hex h, float t) {
		while (true) {
			yield return new WaitForSeconds (t);
			if(ShootingCycleDisable <= Time.time && CanShoot(h, null)) {
				CastHex (h, gameObject.transform.GetChild(0).gameObject, GameObject.FindGameObjectWithTag ("MainCamera").transform.position);
			}
		}
	}

	void checkSafety()
	{
		ArrayList dangerousSpells = aiBase.isInDanger ();
		if (dangerousSpells.Count > 0) {
			//Choose the one that is most important
			//string[] priorities = new string[] {"Damage", "Disarm", "Stun"};
			//So the spells are actually in alphabetical order. 
			dangerousSpells.Sort (); 
			//Move 
			Vector3 position = this.gameObject.transform.position;
			Vector3 direction = new Vector3 ( speed * ((GameObject)dangerousSpells [0]).transform.position.x, 0,0);
			aiBase.move(this.gameObject, direction);
		} else {
			aiBase.cancelMove (this.gameObject);
		}
	}

	//Source should be the AI specific launch point, target is the player
	public override void CastHex (Hex hex, GameObject source, Vector3 target) {
		Hex proj = Instantiate (hex, source.transform.position, new Quaternion(0,0,0,0)) as Hex;
		proj.gameObject.GetComponent<Rigidbody> ().AddForce ((target-gameObject.transform.position).normalized * (float) hex.velocity);
		proj.gameObject.tag = "AIHex";
		//Destroy (hex.gameObject, hex.timeout);
	}

	//pass null to wand, we don't particularly care about it for the AI context
	public override bool CanShoot(Hex h, GameObject wand) {
		if (this.cooldown.ContainsKey (h.name)) {
			if (Time.time >= this.cooldown [h.name] + h.cooldown) {
				this.cooldown.Remove (h.name);
			} else {
				return false;
			}
		} 
		this.cooldown.Add (h.name, Time.time);
		return true;


	}

	public override void processHex(Hex h) {
		h.aiCollide (gameObject);
		this.health -= h.damage;
		h.destroy ();
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.health/100);
		if (this.IsDead ())
			Destroy (this.gameObject);
	}
}