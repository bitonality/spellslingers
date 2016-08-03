﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Castagon : MonoBehaviour {

    public Color TouchedColor;

    public Transform AuraAttachPoint;

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
		if (!cp.Touched) {
			cp.Touched = true;
			ActivatedPoints.Enqueue (cp);
			CheckSpell ();
		}
	}

    

	public void CheckSpell() {
		CastagonPoint cp = ActivatedPoints.Dequeue();
		for (int i = (InspectorSpells.Count - 1); i >= 0; i--) {
			CastagonInsepctorEntry potential = InspectorSpells [i];
			if (cp.CastagonPointID !=  potential.order [potential.order.Count - 1]) {
				InspectorSpells.RemoveAt (i);
			} else {
				if (potential.order.Count == 1) {
                    if (potential.spell != null) {
                        player.queuedSpell = potential.spell.GetComponent<Hex>();
                    }
                    // If we've made it this far, assume that it's an aura.
                    if(player.Aura != null) {
                        cp.gameObject.GetComponent<Aura>().InitializeAura(player.gameObject);
                        player.Aura = null;
                    }
                    destroy();
				}
				potential.order.RemoveAt (potential.order.Count - 1);
			}
		}
	}



	// Use this for initialization
	void Start () {
        ActivatedPoints = new Queue<CastagonPoint> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void destroy() {
		Destroy (gameObject);
	}


	[System.Serializable]
	public class CastagonInsepctorEntry {
		public string name;
		public List<int> order;
		public GameObject spell;
	}



}
