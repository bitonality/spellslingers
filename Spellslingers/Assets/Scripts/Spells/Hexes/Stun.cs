using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {


	//duration of stun in ms
	public float duration;

	//How often the decrement will run
	//private float repeatRate = 0.2F;

	//calculated value for how much to decrease the blur size by
	public double interval = 0;


	public override void playerCollide (GameObject playerCameraRig)
	{
        playerCameraRig.GetComponent<ParticleSystem>().Play();
        StartCoroutine(scheduleStop(playerCameraRig));
        playerCameraRig.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        playerCameraRig.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, duration);
    }


	public override void aiCollide (GameObject aiBody) {
        /*
		float delta = aiBody.GetComponent<StateAI> ().speed / 2;
		aiBody.GetComponent<StateAI> ().setSpeed (delta);
		scheduleSetSpeed (aiBody, interval, aiBody.GetComponent<StateAI>().speed + delta);
        aiBody.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        aiBody.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, duration);
        */
        aiBody.GetComponent<StateAI>().Shake(0.1F, duration);
    }

	IEnumerator scheduleSetSpeed(GameObject aiBody, double waitTime, float newSpeed) {
		yield return new WaitForSeconds((float) waitTime);
		aiBody.GetComponent<StateAI> ().setSpeed (newSpeed);
	}

	public override void BehavioralDestroy() {
        gameObject.SetActive(false);
	}

    IEnumerator scheduleStop(GameObject playerCameraRig)
    {
        yield return new WaitForSeconds((float)duration);
        playerCameraRig.GetComponent<ParticleSystem>().Stop();
    }
}
