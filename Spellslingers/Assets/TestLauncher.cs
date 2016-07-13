using UnityEngine;
using System.Collections;

public class TestLauncher : MonoBehaviour {

	public GameObject player;
	public Hex hex;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("LaunchAtPlayer", 5, 3F);
	}

	void LaunchAtPlayer() {
		Hex proj = Instantiate (hex, gameObject.transform.position, gameObject.transform.rotation) as Hex;
		proj.GetComponent<Rigidbody>().velocity = (player.transform.position - gameObject.transform.position).normalized * 2;
		
	}
}
