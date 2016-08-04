﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

// Abstraction layer that encapsulates AI and Players.
public abstract class ControlEntity : Targetable, Influenceable {


    // Inspector facing location for aura particle effects.
    public GameObject AuraParticleAttachPoint;

    // Inspector facing value for the default spell speed modifier.
    public float DefaultSpellSpeedModifier = 1;

    // Encapsulated value for the current spell speed modifier.
    public float SpellSpeedModifier {
        get;
        set;
    }

    public Dictionary<influences, bool> influenceDict = new Dictionary<influences, bool>();

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

    // Instantiates a hex based on a template at source and launches it at target with force modifer forceMod
    public void CastHex(Hex hex, Transform source, Transform target, float sensitivity, float controllerVelocity) {
        Hex proj = Instantiate(hex, source.position, source.rotation) as Hex;
        proj.GetComponent<HomingProjectile>().LaunchProjectile(hex, source, target, sensitivity, controllerVelocity * SpellSpeedModifier);
        ActiveHexes.Add(proj);
        proj.Source = this;
        proj.ScheduleDestroy(proj.Timeout);
    }



	public override void Awake() {
        base.Awake();
        this.SpellSpeedModifier = this.DefaultSpellSpeedModifier;
        this.ActiveHexes = new HashSet<Hex>();
        influenceDict.Add(influences.DISARM, false);
        influenceDict.Add(influences.STUN, false);
        this.Targets.Add(InitialEnemy);
        this.Targets.Add(InitialAura);

    }

    public virtual object ApplyInfluence(influences inf)
    {
        influenceDict[inf] = true;
        return null;
    }
	public virtual object RemoveInfluence(influences inf) {
		influenceDict [inf] = false;
		return null;

	}

    public object RemoveInfluenceTimer(influences inf, float time)
    {
        StartCoroutine(IERemoveInfluence(inf, time));
        return null;
    }

    private IEnumerator IERemoveInfluence(influences inf, float time)
    {
        yield return new WaitForSeconds(time);
        RemoveInfluence(inf);
    }
}
