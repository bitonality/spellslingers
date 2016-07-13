using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {

	//duration of stun in ms
	public double duration;

	//How often the decrement will run
	private float repeatRate = 0.2F;
	//calculated value for how much to decrease the blur size by
	public double interval;
	private MotionBlur blur;

	public override void playerCollide (GameObject playerCameraRig)
	{
		


			MotionBlur blur = playerCameraRig.GetComponentInChildren<MotionBlur> ();
				this.blur = blur;
				blur.blurAmount = 0.92F; 
				double iterations = (duration/1000) / repeatRate;
				interval = this.blur.blurAmount / iterations;
				ScheduleBlur ();


	}

	public override void destroy() {
		this.gameObject.SetActive(false);
	}

	 void FadeBlur() {
		this.blur.blurAmount =  (float) (this.blur.blurAmount - this.interval);
		if (this.blur.blurAmount <= 0) {
			Destroy (this.gameObject);
		}
	}

	 void ScheduleBlur() {
		InvokeRepeating("FadeBlur", 1.0F, repeatRate);
	}


}
