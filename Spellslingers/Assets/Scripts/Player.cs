using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	public int health {
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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
