using UnityEngine;
using System.Collections;
using VRTK;

public class Mordecai : MonoBehaviour {
    [HideInInspector]
    public bool WandTouching = false;
    public GameObject UltimateZoneTemplate;

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Wand") {
            WandTouching = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Wand") {
            WandTouching = false;
        }
    }

  

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeWand(Player p, GameObject wand) {
        wand.GetComponent<VRTK_InteractableObject>().GetGrabbingObject().GetComponent<VRTK_InteractGrab>().ForceRelease();
        Destroy(wand);
        p.UltimateMode = true;
        GameObject zone = Instantiate(UltimateZoneTemplate, p.gameObject.GetComponentInChildren<Camera>().transform.position, Quaternion.identity) as GameObject;
        zone.GetComponent<UltimateHandler>().UltimatePlayer = p;
        Destroy(this.gameObject);
    }

   
}
