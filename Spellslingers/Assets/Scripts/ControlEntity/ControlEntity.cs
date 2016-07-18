using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ControlEntity : MonoBehaviour {


	public virtual void processHex (Hex h);

	public virtual bool CanShoot(Hex h, GameObject launchPoint);

	//<Hex, CastTime>
	public Dictionary<Hex, float> cooldown {
		get;
		set;
	}

	//out of 100
	public double health {
		get;
		set;
	}

	public bool IsDead()
	{
		if (health <= 0) {
			return true;
		} else {
			return false;
		}
	}

	public virtual void CastHex (Hex h, GameObject source, Vector3 target);


}
