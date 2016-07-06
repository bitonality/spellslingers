using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Spell : MonoBehaviour {

	long cooldown;

	public abstract void cast (GameObject castController);

	//Initialization (to avoid NPE)
	void Awake() {

	}
		
	//Change to update later for input event
	void FixedUpdate() {

	}








}
