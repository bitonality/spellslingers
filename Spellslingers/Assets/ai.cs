using UnityEngine;
using System.Collections;



public class ai : MonoBehaviour {
	//So that you can order most important->least important spells in the inspector
	public GameObject[] spookyFactor;

	//Level is 1/2/3, set somewhere else but set here for now.
	private static int level = 1;

	//Used for movement (so it's unpredictable)
	private System.Random rnd = new System.Random ();

	//Speed is 5/5.5/6 m/s
	float speed = (4.5F + (0.5F * level));

	//Hex and spells are in their own respective array so they can be in the inspector. Then, they're placed into a hashtable.
	public Hex[] hexes;
	public float[] times;

	//load hexes into the hashtable on load
	private Hashtable spells = new Hashtable();

	void Start () {
		if (hexes.Length != times.Length) {
			throw new MissingReferenceException("Hex count does not match time count");
		}
		for (int i = 0; i < hexes.Length; i++) {
			spells.Add (hexes [i], times [i]);
		}
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
		if (aiBase.isInDanger ().Count > 0) {
			//Choose the one that is most important
			//Move 
			Vector3 position = this.gameObject.transform.position;
			Vector3 direction = new Vector3 (0, 0, (float) 200 * this.rnd.Next (-2, 0) * 2 + 3);
			aiBase.move(this.gameObject, direction);
		} else {
			aiBase.cancelMove (this.gameObject);
		}
	}
}
