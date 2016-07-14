using UnityEngine;
using System.Collections;

public class TestLauncher : MonoBehaviour {

	public GameObject player;
	public Hex hex;
	public float delay;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("LaunchAtPlayer", delay, 3F);
	}

	void LaunchAtPlayer() {
		Hex proj = Instantiate (hex, gameObject.transform.position + new Vector3(0, 1F, 0), gameObject.transform.rotation) as Hex;
		proj.GetComponent<Rigidbody>().velocity = (player.transform.position - gameObject.transform.position- new Vector3(0,1F, 0)).normalized * 10;
		
	}
}
