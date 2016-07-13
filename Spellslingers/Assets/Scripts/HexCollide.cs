﻿using UnityEngine;
using System.Collections;
using VRTK;

public class HexCollide : MonoBehaviour
{
	//This handles the collisions of hexes... We should try to keep most spell-specific collision processing in each sub-type's collision handler
	public GameObject explosion;


	//Nullable
	public int numLegalCollisions;
	private int numCollisions = 0;


	void OnCollisionEnter (Collision col)
	{
		GameObject effect = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
		Destroy (effect, effect.GetComponentInChildren<ParticleSystem> ().duration);

		//If we've hit a player
		if (col.gameObject.GetComponent<Player>() != null) {
			Hex hex = this.GetComponent<Hex> ();
			hex.playerCollide (col.gameObject);
			Destroy (this.gameObject);
			Debug.Log ("Player was hit");
	
			return;
		}

		if (col.gameObject.tag == "Finish") {
			Destroy (this.gameObject);
			Destroy (col.gameObject);
			return;
		}

		if (numCollisions < numLegalCollisions) { 
			//Processing for under or at collision limit
			numCollisions++;
		} else {
			//Processing for above collision limit
			Destroy (this.gameObject);

		}



	}
}