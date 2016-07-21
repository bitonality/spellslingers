using UnityEngine;
using System.Collections;

public class CastagonPoint : MonoBehaviour {
	public int CastagonPointID;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Wand") {
			Castagon c = gameObject.GetComponentInParent<Castagon> ();
			c.AddPoint (this);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
