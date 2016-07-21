using UnityEngine;
using System.Collections;

public class Castagon : MonoBehaviour {



	void OnTriggerExit(Collider col) {
		this.destroy ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void destroy() {
		Destroy (this.gameObject);
	}
}
