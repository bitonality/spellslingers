using UnityEngine;
using System.Collections;



public class ai : ControlEntity {
	//For stacking of effects that disable shooting
	public float ShootingCycleDisable = 0;

	//Speed it moves at (m/s)
	public float speed;

	//So that you can order most important->least important spells in the inspector
	public GameObject[] spookyFactor;

	//Level is 1/2/3, set somewhere else but set here for now.
	private static int level = 1;

	//Used for movement (so it's unpredictable)
	private System.Random rnd = new System.Random ();

	//Hex and spells are in their own respective array so they can be in the inspector. Then, they're placed into a hashtable.
	public GameObject[] hexes;

	public float[] times;

	//load hexes into the hashtable on load
	private Hashtable spells = new Hashtable();

	void Start () {
		cooldown = new System.Collections.Generic.Dictionary<string, float> ();
		if (hexes.Length != times.Length) {
			throw new MissingReferenceException ("Hex count does not match time count");
		}
		for (int i = 0; i < hexes.Length; i++) {
			spells.Add (hexes [i], times [i]);
		}
		Debug.Log ("Started");

		foreach (DictionaryEntry de in spells) {
			StartCoroutine(spellTimer((((GameObject) de.Key).GetComponent<Hex>()), (float) de.Value));
		}

		//Once every 0.33 seconds, check if the AI is in danger
		InvokeRepeating ("checkSafety", 0F, 0.33F);
	}

	//TODO: Bad assumption kys
	void OnTriggerExit(Collider col) {
		gameObject.GetComponent<Rigidbody> ().velocity *= -1;
	}

	IEnumerator spellTimer (Hex h, float t) {
		while (true) {
			yield return new WaitForSeconds (t);
			if(ShootingCycleDisable <= Time.time && CanShoot(h, null)) {
				CastHex (h, gameObject.transform.GetChild(0).gameObject, GameObject.FindGameObjectWithTag ("MainCamera").transform.position);
			}
		}
	}


	void checkSafety()
	{
		ArrayList dangerousSpells = aiBase.isInDanger ();
		if (dangerousSpells.Count > 0) {
			//Choose the one that is most important
			//Move 
			Vector3 position = this.gameObject.transform.position;
			Vector3 direction = new Vector3 (0, 0, speed * this.rnd.Next (-2, 0) * 2 + 3);
			aiBase.move(this.gameObject, direction);
		} else {
			aiBase.cancelMove (this.gameObject);
		}
	}

	//Source should be the AI specific launch point, target is the player
	public override void CastHex (Hex hex, GameObject source, Vector3 target) {
		Hex proj = Instantiate (hex, source.transform.position, new Quaternion(0,0,0,0)) as Hex;
		proj.gameObject.GetComponent<Rigidbody> ().AddForce ((target-gameObject.transform.position).normalized * (float) hex.velocity);
		proj.gameObject.tag = "AIHex";
		//Destroy (hex.gameObject, hex.timeout);
	}


	//pass null to wand, we don't particularly care about it for the AI context
	public override bool CanShoot(Hex h, GameObject wand) {
		if (this.cooldown.ContainsKey (h.name)) {
			if (Time.time >= this.cooldown [h.name] + h.cooldown) {
				this.cooldown.Remove (h.name);
				return true;
			} else {
				return false;
			}
		} else {
			this.cooldown.Add (h.name, Time.time);
			return true;
		}

	}

	public override void processHex(Hex h) {
		h.aiCollide (gameObject);
		this.health -= h.damage;
		h.destroy ();
		Debug.Log ("AI Health: " + health);
		if (this.IsDead ())
			Destroy (this.gameObject);
	}
}
