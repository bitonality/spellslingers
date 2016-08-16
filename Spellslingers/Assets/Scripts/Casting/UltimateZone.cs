using UnityEngine;
using System.Collections;

public class UltimateZone : MonoBehaviour {

    public int ZoneID;



    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "UltimateOrb") {
            this.gameObject.GetComponentInParent<UltimateHandler>().ZoneEntered(this.gameObject, col.gameObject.GetComponentInParent<SteamVR_TrackedObject>().gameObject);
        }
    }



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
