using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public abstract class ControlEntity : MonoBehaviour {


	public abstract void CastHex (Hex hex, GameObject source, Vector3 target);
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
}
