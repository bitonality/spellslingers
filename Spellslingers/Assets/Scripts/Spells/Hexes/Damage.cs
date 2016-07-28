using UnityEngine;
using System.Collections;


public class Damage : Hex {




	public override void playerCollide (GameObject playerCameraRig)
	{
		Player p = playerCameraRig.GetComponent<Player> ();
		p.Health -= (int) this.Damage;

	}

	public override void aiCollide(GameObject aiBody) {

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
