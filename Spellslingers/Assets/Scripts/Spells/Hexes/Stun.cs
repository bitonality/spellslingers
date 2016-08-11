using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {


    private MotionBlur blur;

    //duration of stun in ms
    public float duration;

	//How often the decrement will run
	private float repeatRate = 0.2F;

	//calculated value for how much to decrease the blur size by
	public double interval = 0;


	public override void playerCollide (GameObject playerCameraRig)
	{
        MotionBlur blur = playerCameraRig.GetComponentInChildren<MotionBlur>();
        this.blur = blur;
        blur.blurAmount = 0.92F;
        double iterations = (duration / 1000) / repeatRate;
        interval = this.blur.blurAmount / iterations;
        ScheduleBlur();

        playerCameraRig.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        playerCameraRig.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, duration / 1000);
    }


	public override void aiCollide (GameObject aiBody) {
        /*
        float delta = aiBody.GetComponent<NewAI> ().speed / 2;
		aiBody.GetComponent<NewAI> ().setSpeed (delta);
		scheduleSetSpeed (aiBody, interval, aiBody.GetComponent<NewAI>().speed + delta);
        */
        aiBody.GetComponent<NewAI>().currentAction.Enqueue(NewAI.validStates.STUNNED);
        aiBody.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        aiBody.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, duration / 1000);
       // aiBody.GetComponent<NewAI>().Shake(0.1F, duration);
    }

	IEnumerator scheduleSetSpeed(GameObject aiBody, double waitTime, float newSpeed) {
		yield return new WaitForSeconds((float) waitTime);
		aiBody.GetComponent<NewAI> ().setSpeed (newSpeed);
	}

	public override void BehavioralDestroy() {
        gameObject.SetActive(false);
	}

    IEnumerator scheduleStop(GameObject playerCameraRig)
    {
        yield return new WaitForSeconds((float)duration);
        playerCameraRig.GetComponent<ParticleSystem>().Stop();
    }

    void ScheduleBlur() {
        InvokeRepeating("FadeBlur", 1.0F, repeatRate);
    }
    void FadeBlur() {
        this.blur.blurAmount = (float)(this.blur.blurAmount - this.interval);
        if (this.blur.blurAmount <= 0) {
            Destroy(this.gameObject);
        }
    }
}
