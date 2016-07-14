using UnityEngine;
using System.Collections;

public class aibasetest : MonoBehaviour {

	aiBase scripto;

	void Start () {
		Debug.Log ("Firing movement");
		Debug.Log (this.gameObject);
		aiBase baseo = gameObject.AddComponent <aiBase>() as aiBase;
		baseo.move (this.gameObject, new Vector3 (0, 0, 0), 10F);
		StartCoroutine (waitfor(this.gameObject));

		
	}
		
	IEnumerator waitfor(GameObject GO){
				yield return new WaitForSeconds (5.0F);
		GO.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
	}
}
