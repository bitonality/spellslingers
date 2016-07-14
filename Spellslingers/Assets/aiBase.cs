using UnityEngine;
using System.Collections;

public class aiBase : MonoBehaviour {
	public void move(GameObject ai, Vector3 goal, float time){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		Vector3 current = ai.transform.position;
		rb.isKinematic = false;
		rb.detectCollisions = true;
		rb.useGravity = false;
		rb.drag = 0;
		rb.angularDrag = 0;
		float xDistance = (goal.x - current.x)/time;
		float yDistance = (goal.y - current.y)/time;
		float zDistance = (goal.z -current.z)/time;
		rb.velocity = new Vector3 (xDistance, yDistance, zDistance); // This calculation has a .05% deviation from the goal, call accordingly or ignore, not terribly mathematically significant.
		StartCoroutine(scheduleCancelMove(ai, time));
	}

	public IEnumerator scheduleCancelMove(GameObject ai, float time) {
		yield return new WaitForSeconds(time);
		cancelMove(ai);
	}

	public Vector3 getVelocity(GameObject ai){
		return ai.GetComponent<Rigidbody>().velocity;
	}

	public override string ToString ()
	{
		return string.Format ("[aiBase]");
	}

	public bool isMoving(GameObject ai){
		Rigidbody rb = ai.GetComponent<Rigidbody>();
		return (rb.velocity.y == 0 && rb.velocity.x == 0 && rb.velocity.z == 0);
	}

	public Vector3 getPos(GameObject ai){
		return ai.GetComponent<Rigidbody> ().transform.position;
	}

	//TODO: Make georgie do this
	public void shootSpell(spell type, Vector3 endpoint)
	{
		return;
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

	public void cancelMove(GameObject GO) {
		GO.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
		return;
	}
}