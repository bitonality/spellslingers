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
	public static ArrayList isInDanger() {
		//Get all spells
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Hex");
		ArrayList dangerousSpells = new ArrayList();
		foreach (GameObject spell in spells) {
			Debug.Log (spell);
			//TODO: Un-hardcode max length (50 right now)
			if (Physics.Raycast (spell.transform.position, spell.gameObject.GetComponent<Rigidbody> ().velocity.normalized, 50F, 1 << 8)) {
				dangerousSpells.Add (spell);
			}
		}
		return dangerousSpells;
	}

	public static void cancelMove(GameObject GO) {
		GO.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
		return;
	}
}