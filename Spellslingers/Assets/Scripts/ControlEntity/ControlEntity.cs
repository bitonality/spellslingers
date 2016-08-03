using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

// Abstraction layer that encapsulates AI and Players.
public abstract class ControlEntity : Targetable {
	public enum influences { 
		DISARM,
		STUN
	}

    // For testing purposes.
    public GameObject InitialEnemy;
    public GameObject InitialAura;

    // Aura that the player currently has stashed
    public GameObject Aura {
        get;
        set;
    }

    // HashSet of hexes that a player has currently shot.
    public HashSet<Hex> ActiveHexes {
        get;
        set;
    }

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


	public void changeInfluenceState(influences influence, bool newState) {
		
		currentInfluences [influence] = newState;
	}

    // Instantiates a hex based on a template at source and launches it at target with force modifer forceMod
    public void CastHex(Hex hex, Transform source, Transform target, float sensitivity, float controllerVelocity) {
        Hex proj = Instantiate(hex, source.position, source.rotation) as Hex;
        proj.GetComponent<HomingProjectile>().LaunchProjectile(hex, source, target, sensitivity, controllerVelocity);
        this.ActiveHexes.Add(proj);
        proj.Source = this;
        proj.ScheduleDestroy(proj.Timeout);
	}



	void Awake() {
        this.ActiveHexes = new HashSet<Hex>();
		this.currentInfluences = new Dictionary<influences, bool> ();
        this.Targets.Add(InitialEnemy);
        this.Targets.Add(InitialAura);
	}
}
