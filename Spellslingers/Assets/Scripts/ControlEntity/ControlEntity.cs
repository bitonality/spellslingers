using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public abstract class ControlEntity : MonoBehaviour {

	public GameObject HealthBar;
	public abstract void processHex (Hex h);
	public abstract bool CanShoot(Hex h, GameObject launchPoint);

	//<Hex, CastTime>
	public Dictionary<string, float> cooldown {
		get;
		set;
	}

	//out of 100
	public double health;

	public bool IsDead()
	{

		return(health <= 0);
	}

	public void CastHex (Hex hex, Vector3 source, Vector3 target) {
		Hex proj = Instantiate (hex, source, new Quaternion(0,0,0,0)) as Hex;
		proj.gameObject.GetComponent<Rigidbody> ().AddForce ((target-gameObject.transform.position).normalized * (float) hex.velocity);
		Destroy (hex.gameObject, hex.timeout);
	}
}
