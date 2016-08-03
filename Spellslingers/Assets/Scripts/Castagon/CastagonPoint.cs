using UnityEngine;
using System.Collections;

public class CastagonPoint : MonoBehaviour {
	public int CastagonPointID;
	public bool Touched = false;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Wand") {
			Castagon c = gameObject.GetComponentInParent<Castagon> ();
			c.AddPoint (this);
			gameObject.GetComponentInChildren<ParticleSystem> ().startColor = gameObject.GetComponentInParent<Castagon> ().TouchedColor;
			gameObject.GetComponentInChildren<ParticleSystem> ().Simulate (gameObject.GetComponentInChildren<ParticleSystem> ().duration);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
