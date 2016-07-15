using UnityEngine;
using System.Collections;

public class aiBase {
	
	public static void move(GameObject ai, Vector3 directionForce){
		ai.GetComponent<Rigidbody>().AddForce (directionForce, ForceMode.Impulse); 
	}


	public static Vector3 getVelocity(GameObject ai){
		return ai.GetComponent<Rigidbody>().velocity;
	}



	public static bool isMoving(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		return (rb.velocity.y == 0 && rb.velocity.x == 0 && rb.velocity.z == 0);
	}

	public static Vector3 getPos(GameObject ai){
		return ai.GetComponent<Rigidbody> ().transform.position;
	}



	//Assumes AI and spells are on their own layer
	public static bool isInDanger() {
		//Get all spells
		//Georgie said this like would be STC
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Hex");
		foreach (GameObject spell in spells) {
			//TODO: Un-hardcode max length (100 right now)
			//TODO: Un-hardcode layer ID. Assuming 1 for now because we only have two layers
			if (Physics.Raycast (spell.transform.position, spell.GetComponent<Rigidbody> ().velocity, 100F, 1 << 8)) {
				return true;
			}

		}
		return false;
	}

	public static void cancelMove(GameObject GO) {
		GO.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
		return;
	}
}