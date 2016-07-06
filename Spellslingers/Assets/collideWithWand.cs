using UnityEngine;
using System.Collections;

public class collideWithWand : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collider col) {
		if (col.GetComponent<GameObject> ().tag == "Wand") {

		}
	}
}
