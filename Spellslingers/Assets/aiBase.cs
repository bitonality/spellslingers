using UnityEngine;
using System.Collections;

public class aiBase : MonoBehaviour {
	/*
	 * public void move(double x, double y, double z, double time)
	 * public bool isInDanger()
	 * public vector3 getPos()
	 * public vector3 getVolocity()
	 * public bool isMoving()
	 * public void shootSpell(spell type, double x, double y, double z, double velocity) //George will do this
 	 */

	public void Start() {
		//GameObject go = 
		//move (new GameObject, new Vector3, 5.0);
	}

	//If called with a double, will call with the float but calculate speed first
	public void move(GameObject AI, Vector3 direction, double time) {
		//Calculate speed
		//Get AI position
		float distance = Vector3.Distance(AI.transform.position, direction);
		float velocity = distance / time;
		move (AI, direction, velocity);
	}

	public void move(GameObject AI, Vector3 direction, float speed) {
		AI.transform.Translate(direction * speed);
	}
}
