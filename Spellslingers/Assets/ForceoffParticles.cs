using UnityEngine;
using System.Collections;

public class ForceoffParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<ParticleSystem> ().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
