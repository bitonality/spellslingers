using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;


public abstract class Hex : Spell {
	//num seconds until clean up
	public float timeout = 20;

	//out of 100
	public double damage;

	//speed multiplier
	public double velocity;

	//Desfault destroy, can be overridden when appropriate in child spells
	public virtual void destroy() {
		Destroy (this.gameObject);
	}

	public abstract void playerCollide(GameObject playerCameraRig);
	public abstract void aiCollide(GameObject aiBody);







}
