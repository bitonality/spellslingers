using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {


	//duration of stun in ms
	public double duration;

	//How often the decrement will run
	private float repeatRate = 0.2F;

	//calculated value for how much to decrease the blur size by
	public double interval = 0;


	public override void playerCollide (GameObject playerCameraRig)
	{
		playerCameraRig.GetComponent<ParticleSystem> ().Play();
		StartCoroutine(scheduleStop (playerCameraRig));
		playerCameraRig.GetComponent<ControlEntity> ().changeInfluenceState (ControlEntity.influences.DISARM, true);
	}


	public override void aiCollide (GameObject aiBody) {
		float delta = aiBody.GetComponent<Ai> ().speed / 2;
		aiBody.GetComponent<Ai> ().setSpeed (delta);
		scheduleSetSpeed (aiBody, interval, aiBody.GetComponent<Ai>().speed + delta);
		aiBody.GetComponent<Ai>().GetComponent<ControlEntity> ().changeInfluenceState (ControlEntity.influences.STUN, true);
	}

	IEnumerator scheduleSetSpeed(GameObject aiBody, double waitTime, float newSpeed) {
		yield return new WaitForSeconds((float) waitTime);
		aiBody.GetComponent<Ai> ().setSpeed (newSpeed);
	}

	public override void BehavioralDestroy() {
		this.gameObject.SetActive(false);
	}

	IEnumerator scheduleStop(GameObject playerCameraRig) {
		yield return new WaitForSeconds ((float)duration);
		playerCameraRig.GetComponent<ParticleSystem> ().Stop();
	}

	

}
