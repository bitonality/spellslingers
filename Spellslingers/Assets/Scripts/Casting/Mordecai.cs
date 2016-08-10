using UnityEngine;
using System.Collections;
using VRTK;

public class Mordecai : MonoBehaviour {

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
    }
}
