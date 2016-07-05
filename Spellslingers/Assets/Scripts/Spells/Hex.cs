using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public class Hex : Spell {


	//out of 100
	double damage;

	//Meters per second
	double velocity;

	//projectile that will spawn once the spell is cast
	GameObject projectile;


	public override void cast (GameObject castController) {
		GameObject projectile = (GameObject)Instantiate(Resources.Load("Hex"));
		projectile.transform.position = castController.transform.position;
		projectile.GetComponent<VRTK_InteractableObject> ().Grabbed (castController);
		projectile.GetComponent<VRTK_InteractableObject> ().Ungrabbed (castController);
	}

	//Initialization (to avoid NPE)
	void Awake() {

	}

	//Change to update later for input event
	void FixedUpdate() {

	}








}
