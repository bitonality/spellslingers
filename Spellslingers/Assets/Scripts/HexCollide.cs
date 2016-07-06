using UnityEngine;
using System.Collections;
using VRTK;

public class HexCollide : MonoBehaviour
{
	//This handles the collisions of hexes... We should try to keep most spell-specific collision processing in each sub-type's collision handler
	public GameObject explosion;


	//Nullable
	public int? numLegalCollisions;
	public int numCollisions = 0;


	void OnCollisionEnter (Collision col)
	{

		//If we've hit the player
		if (col.gameObject.tag == "MainCamera") {
			Hex hex = this.gameObject.GetComponent<Hex> ();
			hex.playerCollide (col.gameObject);
			Destroy (this.gameObject);
			return;
		}
			
		if (numLegalCollisions != null && numCollisions <= numLegalCollisions) { 
			//Processing for under or at collision limit
			Instantiate (explosion, transform.position, transform.rotation);

		} else {
			//Processing for above collision limit
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (this.gameObject);

		}


	}
}