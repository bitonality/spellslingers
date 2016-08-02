using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

// Abstraction layer that encapsulates AI and Players.
public abstract class ControlEntity : MonoBehaviour {
	public enum influences
	{
		DISARM,
		STUN
	}
    // HashSet of hexes that a player has currently shot.
    public HashSet<Hex> ActiveHexes {
        get;
        set;
    }

    // Under the assumption that there is only one enemy for each player.
    public List<GameObject> Targets;

    // The health bar UI element tied to the ControlEntity.
	public GameObject HealthBar;
    // Abstract method to process the hex collision.
	public abstract void processHex (Hex h);
    // Abstract method to determine whether the ControlEntity can shoot or not.
	public abstract bool CanShoot(Hex h, GameObject launchPoint);

	// Cooldown dictionary that maps string Spell names to Ta future time when the next spell is castable (Time.time + cooldown).
	public Dictionary<string, float> cooldown {
		get;
		set;
	}

	public Dictionary<influences, bool> currentInfluences {
		get;
		set;
	}

    // Max health value set in the inspector.
	public float MaxHealth;

    // Current health of the player.
    public float Health {
        get;
        set;
    }
   
    // Returns if the player is dead or not.
	public bool IsDead() {
		return(Health <= 0);
	}

	public void changeInfluenceState(influences influence, bool newState) {
		
		currentInfluences [influence] = newState;
	}

    // Instantiates a hex based on a template at source and launches it at target with force modifer forceMod
    public void CastHex(Hex hex, Transform source, Transform target, float sensitivity, float controllerVelocity) {
        Hex proj = Instantiate(hex, source.position, source.rotation) as Hex;
        proj.GetComponent<HomingProjectile>().LaunchProjectile(hex, source, target, sensitivity, controllerVelocity);
        this.ActiveHexes.Add(proj);
        proj.ScheduleDestroy(proj.Timeout);
	}


    public bool MutualTargets(GameObject target) {
        return (target.GetComponent<ControlEntity>().Targets.Contains(this.gameObject) && Targets.Contains(target)) ;
    }

    public GameObject CurrentTarget() {
        for(int i = Targets.Count - 1; i >= 0; i--) {
            if (Targets[i] != null) {
                return Targets[i];
            } else {
                Targets.RemoveAt(i);
            }
        }

        return null;
    }


    // Safely checks for extra additions and adds an element
    public void AddTarget(GameObject target) {
        if (Targets[Targets.Count - 1] != target) { 
            Targets.Add(target);
        }
    }
   

	void Awake() {
        this.Targets = new List<GameObject>();
		this.Health = this.MaxHealth;
        this.ActiveHexes = new HashSet<Hex>();
		this.currentInfluences = new Dictionary<influences, bool> ();
	}
}
