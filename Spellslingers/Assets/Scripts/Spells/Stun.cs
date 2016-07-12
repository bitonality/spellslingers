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
	private BlurOptimized blur;

	public override void playerCollide (GameObject playerCameraRig)
	{
		Player p = playerCameraRig.GetComponent<Player> ();
		Transform[] childTransforms = playerCameraRig.GetComponentsInChildren<Transform> ();
		foreach(Transform t in childTransforms) {
			if (t.parent.gameObject.tag == "MainCamera") {
				BlurOptimized blur = t.gameObject.AddComponent<BlurOptimized> ();
				this.blur = blur;
				Destroy (blur, (float) (this.duration / 1000));
				double iterations = (duration/1000) / repeatRate;
				interval = this.blur.blurSize / iterations;
				this.blur.blurIterations = 1;
				ScheduleBlur ();
				return;
			}
		}

	}



	private void FadeBlur() {
		this.blur.blurSize -= (float) this.interval;
		Debug.Log (this.blur.blurSize);

	}

	private void ScheduleBlur() {
		InvokeRepeating("FadeBlur", 0, repeatRate);
	}


}
