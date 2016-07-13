using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class Stun : Hex {

	//duration of stun in ms
	public double duration;

	//How often the decrement will run
	private float repeatRate = 0.5F;
	//calculated value for how much to decrease the blur size by
	private double interval;
	private MotionBlur blur;

	public override void playerCollide (GameObject playerCameraRig)
	{
		


		MotionBlur blur = playerCameraRig.GetComponentInChildren<SteamVR_Camera> ().gameObject.AddComponent<MotionBlur> ();
				this.blur = blur;
		blur.enabled = true;
				Destroy (blur, (float) (duration / 1000));
				double iterations = (duration/1000) / repeatRate;
				interval = this.blur.blurAmount / iterations;
				//ScheduleBlur ();
				return;



	}



	private void FadeBlur() {
		this.blur.blurAmount -= (float) this.interval;
		Debug.Log (this.blur.blurAmount);

	}

	private void ScheduleBlur() {
		InvokeRepeating("FadeBlur", 0, repeatRate);
	}


}
