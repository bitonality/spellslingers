using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castagon : MonoBehaviour {


	public Player player {
		get;
		set;
	}


	public List<CastagonInsepctorEntry> InspectorSpells;

	private Queue<CastagonPoint> ActivatedPoints {
		get;
		set;
	}


	public void AddPoint(CastagonPoint cp) {
		this.ActivatedPoints.Enqueue (cp);
		CheckSpell ();
	}

	public void CheckSpell() {
		CastagonPoint cp = ActivatedPoints.Dequeue();
		for (int i = (InspectorSpells.Count - 1); i >= 0; i--) {
			CastagonInsepctorEntry potential = InspectorSpells [i];
			if (cp.CastagonPointID !=  potential.order [potential.order.Count - 1]) {
				InspectorSpells.RemoveAt (i);
			} else {
				if (potential.order.Count == 1) {
					player.queuedSpell = potential.hex.GetComponent<Hex>();
					this.destroy ();
				}
				potential.order.RemoveAt (potential.order.Count - 1);
			}
		}
	}

	void OnTriggerExit(Collider col) {
		this.destroy ();
	}

	// Use this for initialization
	void Start () {
		this.ActivatedPoints = new Queue<CastagonPoint> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void destroy() {
		Destroy (this.gameObject);
	}


	[System.Serializable]
	public class CastagonInsepctorEntry {
		public string name;
		public List<int> order;
		public GameObject hex;
	}



}
