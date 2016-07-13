using UnityEngine;
using System.Collections;

public class aiBase : MonoBehaviour {
	/*
	 * public void move(Vector3 position, double time)
	 * public bool isInDanger()
	 * public vector3 getPos()
	 * public vector3 getVolocity()
	 * public bool isMoving()
	 * public void shootSpell(spell type, double x, double y, double z, double velocity) //George will do this
 	 */

	public void Start() {
		//GameObject go = 
		//move (new GameObject, new Vector3, 5.0);
		move(this.gameObject, new Vector3(100F, 100F, 100F), 10F);
	}

	//TODO: Make this not broken
	public void move(GameObject AI, Vector3 direction, float velocity) {
		Debug.Log ("Direction: " + direction + ", velocity: " + velocity);
		Vector3 endPosition = AI.transform.position + direction;
		AI.transform.position  = Vector3.Lerp(AI.transform.position, endPosition, velocity * Time.deltaTime);

	}

	public bool isInDanger() {
		//Get all spells
		//Georgie said this like would be STC
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Spell");

	}
}
