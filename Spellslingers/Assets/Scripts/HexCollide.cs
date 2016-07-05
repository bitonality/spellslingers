using UnityEngine;
using System.Collections;
using VRTK;

public class HexCollide : MonoBehaviour
{

	public GameObject explosion;

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Hex")
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(col.gameObject);
		
			//disarm if they have a wand
			VRTK_InteractGrab[] controllers = this.GetComponentsInChildren<VRTK_InteractGrab>();

			foreach (VRTK_InteractGrab controller in controllers) {
				if (controller.GetGrabbedObject () != null) {
					GameObject wand = controller.GetGrabbedObject ();
					controller.ForceRelease ();
					wand.GetComponent<Rigidbody>().velocity = new Vector3 (0, 5, 0);
				}

			}

		}
	}
}