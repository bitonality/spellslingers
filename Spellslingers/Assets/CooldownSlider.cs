using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CooldownSlider : MonoBehaviour {

	GameObject gop;
	Player p;
	GameObject canvas;
	Slider s;

	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
		s = canvas.GetComponentInChildren<Slider> ();
	}

	public void cooldown(Hex hex, float cooldown) {
		
	}


	// Update is called once per frame
	void Update () {
		
	}
}
