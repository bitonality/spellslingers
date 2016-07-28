﻿using UnityEngine;
using System.Collections;

public class CastagonPoint : MonoBehaviour {
	public int CastagonPointID;
	public bool Touched = false;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Wand") {
			Castagon c = gameObject.GetComponentInParent<Castagon> ();
			c.AddPoint (this);
			this.GetComponentInChildren<Light> ().color = new Color (1F, 0.735F, 0F);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
