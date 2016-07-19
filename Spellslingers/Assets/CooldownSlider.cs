using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownSlider : MonoBehaviour {

	GameObject canvas;
	private float updateTick = 0.05F;

	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
	}

	public void cooldown(Hex hex, float future) {
		Slider s = canvas.GetComponentInChildren<Slider> ();
		s.value = 0;
		double incrementValue =  (1/ (double) (hex.cooldown / updateTick));
		Debug.Log (incrementValue);
		StartCoroutine(Increment (incrementValue, s, future));
	}

	IEnumerator Increment(double increment, Slider s, float future) {
		int i = 0;
		while (future > Time.time) {
			s.value += (float) increment;
			i++;
			yield return new WaitForSeconds (updateTick);
		}
		Debug.Log (i);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
