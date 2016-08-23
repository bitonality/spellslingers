using UnityEngine;
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
                    // We've completed a correct sequence at this point, now we have to differentiate between hexes and auras.
                    // If the potential spell is a Hex, run hex code.
                    // potential.spell will be null when a generic aura pattern is defined in the castagon prefab.
                    if (potential.spell != null && potential.spell.GetComponent<Hex>() != null) {
                        player.queuedSpell = potential.spell.GetComponent<Hex>();
                    } else if(this.AuraAttachPoint.gameObject != null) {
                        // Otherwise check if there is a GameObject at the attach point. We check attach point rather than the player's currently cached aura so we don't run into continuity issues.
                        SetActiveRecursively(AuraAttachPoint.gameObject, true);
                        Aura aura = this.AuraAttachPoint.gameObject.GetComponentInChildren<Aura>();
                        aura.InitializeAura(player.gameObject);
                        player.Aura = null;

                        
                    }
                    // This should destroy the parented rune.
					this.destroy ();
				}
				potential.order.RemoveAt (potential.order.Count - 1);
			}
            if(InspectorSpells.Count == 0) {
                this.destroy();
            }
		}
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
		public List<int> order;
		public GameObject spell;
	}

    public static void SetActiveRecursively(GameObject rootObject, bool active) {
        rootObject.SetActive(active);

        foreach (Transform childTransform in rootObject.transform) {
            SetActiveRecursively(childTransform.gameObject, active);
        }
    }



}
