using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class stateAI : ControlEntity {

	//time until next state change 
	private float timeUntilChange = 3;

	//Time until the AI is allowed to shoot after being idle
	public float delayUntilShoot;

	//For stacking of effects that disable shooting
	public float ShootingCycleDisabled = 0;

	//Speed (m/s) of the AI
	public float speed;

	// TODO: Make this an array, etc.
	public Hex spellToShoot;

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

	// Make sure it can't leave the AI boundry.
	void OnTriggerExit(Collider col) {
		//Debug.Log (col);
		if (col.gameObject.tag == "AIBoundry") {
			gameObject.GetComponent<Rigidbody> ().velocity *= -1;
		}
	}

	public Queue<validStates> currentAction = new Queue<validStates>();

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
		//Debug.Log ("Player health: " + this.Enemy.GetComponent<ControlEntity> ().Health);
		//Debug.Log ("Changing state from  " + oldState + " to " + newState + " at time " + Time.time);
		switch (newState) {
		case validStates.DANGER:
            break;
		case validStates.HIT:
			currentAction.Enqueue (validStates.DANGER);
            break;
		case validStates.IDLE:
			//Move in a random direction
			Vector3 Destination = new Vector3(Random.Range(-5f, 5f) * 10, 0, Random.Range(-5f, 5f) * 10);
            this.gameObject.GetComponent<Rigidbody>().AddForce(Destination, ForceMode.Impulse);
            //Schedule interruptable state change in 3 seconds
			timeUntilChange = Time.time + 3;
			break;
		case validStates.PRESHOOT:
			//Stop movement
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			//Wait for 1 second
			timeUntilChange = Time.time + 1;
			break;
		case validStates.SHOOTING:
			//Pick a hex
			Hex h = pickHex();
			if (CanShoot (h, this.gameObject)) {
                //Shoot
          //      CastHex(h, gameObject.transform.GetChild(0).gameObject.transform, this.Enemy.transform, 2, 3);
                //Go back to IDLE state
				currentAction.Enqueue (validStates.POSTSHOOT);
			} else {
				//Return to PRESHOOT stage
				currentAction.Enqueue(validStates.PRESHOOT);
			}
			break;
		case validStates.POSTSHOOT:
			//For now head straight into IDLE
			currentAction.Enqueue (validStates.IDLE);
			break;
		case validStates.DEAD:
			//Kill this script, so that it doesn't keep running this loop
			this.enabled = false;
			break;
		}
		return null;
	}

	//Called every time, but only if a state change did not occur.
	private object currentExclusive(validStates state) {
		if (state == validStates.IDLE) {
			//Make sure it isn't in danger
			if (isInDanger ().Count > 0) {
				currentAction.Enqueue (validStates.DANGER);
			}
		}
		return null;
	}

	//Pick the best hex for the situation
	//TODO: This
	private Hex pickHex() {
		return spellToShoot;
	}

	//Called regardless if a state change occurred, every time. 
	private object currentInclusive(validStates state) {
		if (state == validStates.DANGER) {
			//Move out of danger
			ArrayList dangerousSpells = isInDanger ();
			if (dangerousSpells.Count > 0) {
				//Choose the one that is most important
				//string[] priorities = new string[] {"Damage", "Disarm", "Stun"};
				//So the spells are actually in alphabetical order. 
				//dangerousSpells.Sort (); 
				//Move 
				Vector3 position = this.gameObject.transform.position;
				Vector3 direction = new Vector3 (speed * (float)Vector3.Cross(((GameObject)dangerousSpells [0]).transform.position, gameObject.transform.position).normalized.x, 0, 0);
				gameObject.GetComponent<Rigidbody> ().AddForce (direction, ForceMode.Impulse);
			} else {
				//Stop movement
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				//Push idle state
				currentAction.Enqueue (validStates.IDLE);
			}
		}
		if (state == validStates.IDLE && timeUntilChange <= Time.time) {
			//Change state to preshoot
			currentAction.Enqueue(validStates.PRESHOOT);
		}
		if (state == validStates.PRESHOOT && timeUntilChange <= Time.time) {
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
		this.Health -= h.damage;
		h.destroy ();
		this.HealthBar.GetComponent<Image> ().fillAmount = (float) (this.Health/this.MaxHealth);
		if (this.IsDead ()) {
			Destroy (this.gameObject);
		}
	}
    
	private ArrayList isInDanger() {
		//Get all spells
		//TODO: Don't iterate over all objects
		GameObject[] spells = GameObject.FindGameObjectsWithTag("Hex");
		ArrayList dangerousSpells = new ArrayList();
		foreach (GameObject spell in spells) {
			//Debug.Log (spell);
			//TODO: Un-hardcode max length (50 right now)
			//for some reason the spells array consistently had hexes with no rigibodies in it TODO: redesign
			if (spell.gameObject.GetComponent<Rigidbody>() != null && Physics.Raycast (spell.transform.position, spell.gameObject.GetComponent<Rigidbody> ().velocity.normalized, 50F, 1 << 8)) {
				dangerousSpells.Add (spell);
			}
		}
		return dangerousSpells;
	}
}
