using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownSlider : MonoBehaviour {

	GameObject canvas;
	private float updateTick = 0.1F;

	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
	}

	public void cooldown(Hex hex, float future) {
		Slider s = canvas.GetComponent<Slider> ();
		s.value = 0;
		float incrementValue = hex.cooldown / updateTick;
		StartCoroutine ();
	}

	IEnumerator Increment(float increment, Slider s, float future) {
		while (future >= Time.time) {
			yield return new WaitForSeconds (updateTick);
			s.value += increment;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
