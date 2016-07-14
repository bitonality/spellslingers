using UnityEngine;
using System.Collections;

public class aibasetest : MonoBehaviour {

	aiBase scripto;

	void Start () {
		Debug.Log ("Firing movemnet");
		Debug.Log (this.gameObject);
		aiBase baseo = gameObject.AddComponent <aiBase>() as aiBase;
		baseo.move (this.gameObject, new Vector3 (28, 0, 0), 10F);
	}

	// Update is called once per frame
	void Update () {

	}
}
