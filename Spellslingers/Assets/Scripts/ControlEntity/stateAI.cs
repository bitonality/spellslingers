using UnityEngine;
using System.Collections;

public class stateAI : ControlEntity {

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

    public Queue<validStates> currentAction = new Queue();

	//Called every 0.02 seconds
	void FixedUpdate() {
		if (currentAction.size <= 0) {
			//Something went wrong with starting the AI
			Debug.Warning("currentAction size is 0, meaning start() was not called before fixedupdate()");
			return;
		}
		if (currentAction.size != 1) {
			//Pop leading item off the array and call justLeft with it
			justLeft (currentAction.deque (), getCurrentState ());
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
				break;
		}
	}

	//Called regardless if a state change occurred, every time. 
	private object currentInclusive(validStates state) {

	}

	//Called every time, but only if a state change did not occur.
	private object currentExclusive(validStates state) {

	}

	//Returns the object's current state
	private validStates getCurrentState() {
		return currentAction.peek ();
	}

    // Use this for initialization
    void Start () {
        //Start out the queue with idle
		currentAction.enque(validStates.IDLE);
	}
}
