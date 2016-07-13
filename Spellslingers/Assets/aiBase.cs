using UnityEngine;
using System.Collections;

public class aiBase : MonoBehaviour {
	public void move(GameObject ai, Vector3 goal, double time){
		Rigidbody rb = GameObject.GetComponent<Rigidbody>();
		Vector3 current = ai.transform.position;
		rb.isKinematic = true;
		rb.detectCollisions = true;
		rb.drag = 0;
		rb.angularDrag = 0;
		double xDistance = (current.x - goal.x)/time;
		double yDistance = (current.y - goal.y)/time;
		double zDistance = (current.z - goal.z)/time;
	    rb.velocity = new Vector3 (xDistance, yDistance, zDistance);
		WaitForSeconds (time);
		rb.velocity = new Vector3 (0,0,0);

	}
	public Vector3 getVelocity(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		return rb.velocity;
	}

	public bool isMoving(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		if(rb.velocity.y==0 && rb.velocity.x == 0 && rb.velocity.z == 0){
			return false;
			}
		else{
			return true;
			}
	}
	public Vector3 getPos(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
	}
		
	/*
	 * 
	 * }
	 * public bool isInDanger()
	 * public vector3 getPos()
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
		return true;
	}
}
