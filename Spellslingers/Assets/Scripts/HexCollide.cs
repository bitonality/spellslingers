using UnityEngine;
using System.Collections;

public class HexCollide : MonoBehaviour
{

	public GameObject explosion;

 	void Awake() {
		if(this.GetComponent<BoxCollider>() != null) 
			this.GetComponent<BoxCollider>().isTrigger = true;	
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Hex")
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(col.gameObject);
		
		}
	}
}