using UnityEngine;
using System.Collections;
using VRTK;

public class CastagonPoint : MonoBehaviour {
	public int CastagonPointID;
	public bool Touched = false;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Wand") {
			Castagon c = gameObject.GetComponentInParent<Castagon> ();
			c.AddPoint (this);
            SteamVR_TrackedObject trackedObj = col.gameObject.GetComponent<VRTK_InteractableObject>().GetGrabbingObject().GetComponent<SteamVR_TrackedObject>();
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)trackedObj.index);
            StartCoroutine(TriggerVibration(controller, 0.1F, 1F));
			gameObject.GetComponentInChildren<ParticleSystem> ().startColor = gameObject.GetComponentInParent<Castagon> ().TouchedColor;
			gameObject.GetComponentInChildren<ParticleSystem> ().Simulate (gameObject.GetComponentInChildren<ParticleSystem> ().duration);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator TriggerVibration(SteamVR_Controller.Device device, float length, float strength) {
        for (float i = 0; i < length; i += Time.deltaTime) {
            Debug.Log("Firing feedback");
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
}
