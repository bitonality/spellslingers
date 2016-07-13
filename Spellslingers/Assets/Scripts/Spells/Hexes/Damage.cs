using UnityEngine;
using System.Collections;


public class Damage : Hex {

	public double damage {
		get;
		set;
	}


	public override void playerCollide (GameObject playerCameraRig)
	{
		Player p = playerCameraRig.GetComponent<Player> ();
		p.health -= (int) this.damage;

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
