﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {


    private MotionBlur blur;

    //duration of stun in ms
    public float duration;

    //duration of stun in ms, for the AI
    public float AIDuration;

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
        playerCameraRig.GetComponentInChildren<AudioReverbZone>(true).gameObject.SetActive(true);
        playerCameraRig.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        playerCameraRig.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, duration / 1000);
    }


	public override void aiCollide (GameObject aiBody) {
        
        float delta = aiBody.GetComponent<NewAI> ().speed / 3;
		aiBody.GetComponent<NewAI> ().setSpeed (delta);
		scheduleSetSpeed (aiBody, AIDuration, aiBody.GetComponent<NewAI>().speed + delta);
        

        aiBody.GetComponent<ControlEntity>().ApplyInfluence(influences.STUN);
        aiBody.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.STUN, AIDuration);
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
            this.blur.gameObject.GetComponentInParent<Player>().gameObject.GetComponentInChildren<AudioReverbZone>(true).gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
