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

	public void cooldown(Hex hex, float cooldown) {
		Slider s = canvas.GetComponentInChildren<Slider> ();
		s.value = 0;
		double incrementValue =  (1/ (double) (hex.cooldown / updateTick));
		StartCoroutine(Increment (incrementValue, s, cooldown));
	}

	IEnumerator Increment(double increment, Slider s, float cooldown) {
		int i = 0;
		while (cooldown >= 0) {
			s.value += (float) increment;
			i++;
			cooldown -= Time.deltaTime;
			yield return new WaitForSeconds (updateTick);
		}
		Debug.Log (i);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
