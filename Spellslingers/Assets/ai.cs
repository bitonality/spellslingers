using UnityEngine;
using System.Collections;



public class ai : MonoBehaviour {
	//Only until we get the spells setup, so we don't get errors
	spell dizzy;
	spell damage;
	spell disarm;
	private System.Random rnd = new System.Random ();
	//-1 for left, +1 for right, 0 for default
	private int movementDirection = 0;

	//Level is 1/2/3, set somewhere else but set here for now.
	static int level = 1; 
	//Speed is 5/5.5/6 m/s
	float speed = (4.5F + (0.5F * level));

	public Hex hex1;
	public float time1;
	public Hex hex2;
	public float time2;

	//load hexes into the hashtable on load
	private Hashtable spells = new Hashtable();

	void Start () {
		spells.Add (hex1, time1);
		spells.Add (hex2, time2);
		Debug.Log ("Started");

		foreach (DictionaryEntry de in spells) {
			StartCoroutine (ShootSpell ((Hex) de.Key, (float) de.Value));
		}
	
		//Once every 0.33 seconds, check if the AI is in danger
		InvokeRepeating("checkSafety", 0F, 0.33F);
	}

	IEnumerator ShootSpell(Hex h, float delaytime) {
		yield return new WaitForSeconds (delaytime);
		shootSpell (h);
	}


	//TODO: Bad assumption kys
	void OnTriggerExit(Collider col) {
		gameObject.GetComponent<Rigidbody> ().velocity *= -1;
	}

	//TODO: Make georgie do this
	public  void shootSpell(Hex hex)
	{
		Hex proj = Instantiate (hex, gameObject.transform.position + new Vector3(0, 1F, 0), gameObject.transform.rotation) as Hex;
		proj.GetComponent<Rigidbody>().velocity = (GameObject.FindGameObjectWithTag("MainCamera").transform.position - gameObject.transform.position- new Vector3(0,1F, 0)).normalized * 10;
	}

	void checkSafety()
	{
		if (aiBase.isInDanger ()) {
			//Move 
			Vector3 position = this.gameObject.transform.position;
			Vector3 direction = new Vector3 (0, 0, (float) 200 * this.rnd.Next (-2, 0) * 2 + 3);
			aiBase.move(this.gameObject, direction);
		} else {
			aiBase.cancelMove (this.gameObject);
		}
	}
}
