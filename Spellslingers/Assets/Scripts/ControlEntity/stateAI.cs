using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class stateAI : ControlEntity {

	public float health = 100;

	//time until next state change 
	private float timeUntilChange = 0;

	//Time until the AI is allowed to shoot after being idle
	public float delayUntilShoot;

	//For stacking of effects that disable shooting
	public float ShootingCycleDisabled = 0;

	public enum validStates
    {
        HIT,
        IDLE,
        DANGER,
        PRESHOOT,
        SHOOTING,
        POSTSHOOT,
        DEAD
    }
	public Queue currentAction = new Queue();

	//Called every 0.02 seconds
	void FixedUpdate() {
		if (currentAction.Count <= 0) {
			//Something went wrong with starting the AI
			Debug.LogWarning("currentAction size is 0, meaning start() was not called before fixedUpdate()");
			return;
		}
		if (currentAction.Count != 1) {
			//Pop leading item off the array and call justLeft with it
			justLeft ((validStates) currentAction.Dequeue (), getCurrentState ());
		} else {
			//If there was not a state change, call currentExclusive
			currentExclusive (getCurrentState());
		}
		//Call currentInclusive, regardless if there was a state change or not
		currentInclusive (getCurrentState ());
	}

	//Called only when changing state
	private object justLeft(validStates oldState, validStates newState) {
		switch (newState) {
		case validStates.HIT:
			break;
		case validStates.IDLE:
			//Move in a random direction
			this.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (-5f, 5f) * 10, 0, Random.Range (-5f, 5f) * 10), ForceMode.Impulse); 
			//Schedule interruptable state change in 3 seconds
			timeUntilChange = Time.time + 3;
			break;
		case validStates.PRESHOOT:
			//Stop movement
			this.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
			//Wait for 1 second
			timeUntilChange = Time.time + 1;
			break;
		case validStates.SHOOTING:
			//Pick a hex
			Hex h = pickHex();
			if (CanShoot (h, this.gameObject)) {
				//Shoot
				CastHex (h, gameObject.transform.GetChild(0).gameObject.transform.position, GameObject.FindGameObjectWithTag ("MainCamera").transform, 1F, new Vector3(0,0,0));
				//Go back to idle state
				currentAction.Enqueue (validStates.IDLE);
			} else {
				//Return to PRESHOOT stage
				currentAction.Enqueue(validStates.PRESHOOT);
			}
			break;
		}
		return null;
	}

	//Called every time, but only if a state change did not occur.
	private object currentExclusive(validStates state) {
		return null;
	}

	//Pick the best hex for the situation
	//TODO: This
	private Hex pickHex() {
		return new Stun();
	}

	//Called regardless if a state change occurred, every time. 
	private object currentInclusive(validStates state) {
		if (state == validStates.IDLE && timeUntilChange >= Time.time) {
			//Change state to preshoot
			currentAction.Enqueue(validStates.PRESHOOT);
		}
		if (state == validStates.PRESHOOT && timeUntilChange >= Time.time) {
			//Change to shooting state
			currentAction.Enqueue (validStates.SHOOTING);
		}
		return null;
	}

	//Returns the object's current state
	private validStates getCurrentState() {
		return (validStates) currentAction.Peek ();
	}

    // Use this for initialization
    void Start () {
        //Start out the queue with idle
		currentAction.Enqueue (validStates.IDLE);

		//List of cooldowns
		cooldown = new System.Collections.Generic.Dictionary<string, float> ();
	}

	public override bool CanShoot(Hex h, GameObject launchPoint) {
		return Time.time >= ShootingCycleDisabled;
	}

	public override void processHex(Hex h) {
		h.aiCollide (gameObject);
		this.health -= h.damage;
		h.destroy ();
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.health/200);
		if (this.IsDead ()) {
			Destroy (this.gameObject);
		}
	}

	//Prevent AI from leaving play bounds
	//TODO: Add the bounds
	void OnTriggerExit(Collider col) {
		if (col.tag == "aiBounds") {
			gameObject.GetComponent<Rigidbody> ().velocity *= -1;
		}
	}
    
	private ArrayList checkDanger() {
		//Get all spells
		//TODO: Don't iterate over all objects
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Hex");
		ArrayList dangerousSpells = new ArrayList();
		foreach (GameObject spell in spells) {
			Debug.Log (spell);
			//TODO: Un-hardcode max length (50 right now)
			//for some reason the spells array consistently had hexes with no rigibodies in it TODO: redesign
			if (spell.gameObject.GetComponent<Rigidbody>() != null && Physics.Raycast (spell.transform.position, spell.gameObject.GetComponent<Rigidbody> ().velocity.normalized, 50F, 1 << 8)) {
				dangerousSpells.Add (spell);
			}
		}
		return dangerousSpells;
	}
}
