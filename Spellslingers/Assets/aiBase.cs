using UnityEngine;
using System.Collections;

public class aiBase : MonoBehaviour {
	public void move(GameObject ai, Vector3 goal, float time){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		Vector3 current = ai.transform.position;
		rb.isKinematic = true;
		rb.detectCollisions = true;
		rb.drag = 0;
		rb.angularDrag = 0;
		float xDistance = (current.x - goal.x)/time;
		float yDistance = (current.y - goal.y)/time;
		float zDistance = (current.z - goal.z)/time;
		rb.velocity = new Vector3 (xDistance, yDistance, zDistance);
		new WaitForSeconds (time);
		rb.velocity = new Vector3 (0,0,0);

	}
	public Vector3 getVelocity(GameObject ai){
		return ai.GetComponent<Rigidbody>().velocity;
	}

	public bool isMoving(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		return (rb.velocity.y == 0 && rb.velocity.x == 0 && rb.velocity.z == 0);
	}
	public Vector3 getPos(GameObject ai){
		return ai.GetComponent<Rigidbody> ().transform.position;
	}

	/*
	 * public void shootSpell(spell type, double x, double y, double z, double velocity) //George will do this
 	 */

	public void Start() {

	}

	//Assumes AI and spells are on their own layer
	public bool isInDanger() {
		//Get all spells
		//Georgie said this like would be STC
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Spell");
		foreach (GameObject spell in spells) {
			//TODO: Un-hardcode max length (100 right now)
			//TODO: Un-hardcode layer ID. Assuming 1 for now because we only have two layers
			if (Physics.Raycast (spell.transform.position, spell.GetComponent<Rigidbody> ().velocity, 100F, 1)) {
				return true;
			}

		
	}
		return false;
}
}