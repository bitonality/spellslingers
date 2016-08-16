using UnityEngine;
using System.Collections;

public class UltimateZone : MonoBehaviour {

    public int ZoneID;



    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "GameController") {
            this.gameObject.GetComponentInParent<UltimateHandler>().ZoneEntered(this.gameObject, col.gameObject);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "GameController") {
            if (col.gameObject.GetComponentInChildren<ParticleSystem>() != null) {
                this.gameObject.GetComponentInParent<UltimateHandler>().ZoneEntered(this.gameObject, col.gameObject);
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
