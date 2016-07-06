using UnityEngine;
using System.Collections;

public class collideWithWand : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnColliderEnter(Collider col) {
		if (col.GetComponent<GameObject> ().tag == "Wand") {
			Camera.main.ResetFieldOfView ();
		}
	}
}