﻿/*
 * This is the old AI script
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Ai : ControlEntity {


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

	public void StartAi () {
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
/*
		//Record the default speed
		defaultSpeed = speed;
		//Start the shooting loop


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



	//pass null to wand, we don't particularly care about it for the AI context
	public override bool CanShoot(Hex h, GameObject wand) {
		if (this.cooldown.ContainsKey (h.HexName)) {
			if (Time.time >= this.cooldown [h.HexName] + h.cooldown) {
				this.cooldown.Remove (h.HexName);
			} else {
				return false;
			}
		} 
		this.cooldown.Add (h.HexName, Time.time);
		return true;


	}

	public override void processHex(Hex h) {
		h.aiCollide (gameObject);
		this.Health -= h.Damage;
		h.Destroy ();
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.Health/this.MaxHealth);
		if (this.IsDead ())
			Destroy (this.gameObject);
	}
}
*/