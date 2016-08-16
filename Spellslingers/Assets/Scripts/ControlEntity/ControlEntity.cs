using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine.UI;
using System;

// Abstraction layer that encapsulates AI and Players.
public abstract class ControlEntity : Targetable, Influenceable {

    public bool UltimateMode {
        get;
        set;
    }

    // Tracks how many auras the CE has triggered.
    public int UltimateCounter {
        get;
        set;
    }

    
    public GameObject influenceText;

    // How many auras the player needs to cast before being allowed to use ults.
    public int UltimateChargeTrigger;

    // Inspector facing location for aura particle effects.
    public GameObject AuraParticleAttachPoint;

    // Inspector facing value for the default spell speed modifier.
    public float DefaultSpellSpeedModifier = 1;

    // Encapsulated value for the current spell speed modifier.
    public float SpellSpeedModifier {
        get;
        set;
    }

    public Dictionary<influences, InfluenceValue> influenceDict;

    // For testing purposes.
    public GameObject InitialEnemy;

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

    // Chargebar for the ultimate.
    public GameObject UltimateChargeBar;

    // Instantiates a hex based on a template at source and launches it at target with force modifer forceMod
    public void CastHex(Hex hex, Transform source, Transform target, float sensitivity, float controllerVelocity) {
        Hex proj = Instantiate(hex, source.position, source.rotation) as Hex;
        proj.GetComponent<HomingProjectile>().LaunchProjectile(hex, source, target, sensitivity, controllerVelocity * SpellSpeedModifier);
        ActiveHexes.Add(proj);
        proj.Source = this;
        proj.ScheduleDestroy(proj.Timeout);
    }

    public void IncrementUltimateCounter(int i) {
        if(UltimateCounter + i > UltimateChargeTrigger) {
            return;
        }

        UltimateCounter += i;
        if(this.UltimateChargeBar != null) {
            this.UltimateChargeBar.GetComponent<Image>().fillAmount = (this.UltimateChargeTrigger / this.UltimateCounter);
        }

    }

    public virtual void CastUltimate(GameObject target, GameObject ultimate) {
        this.UltimateMode = false;
        GameObject instantiatedUltimate = Instantiate(ultimate);
        instantiatedUltimate.GetComponent<Ultimate>().Cast(this.gameObject, target);
    }

	public override void Awake() {
        base.Awake();
        UltimateMode = false;
        UltimateCounter = 0;
        this.SpellSpeedModifier = this.DefaultSpellSpeedModifier;
        this.ActiveHexes = new HashSet<Hex>();
        influenceDict = new Dictionary<influences, InfluenceValue>();
        influenceDict.Add(influences.DISARM, new InfluenceValue(false, 0, "Disarmed"));
        influenceDict.Add(influences.STUN, new InfluenceValue(false, 0, "Stunned"));
        InitialEnemy.GetComponent<Targetable>().Priority = 2;
        this.Targets.Add(InitialEnemy);
        InvokeRepeating("RedrawInfluences", 0.1F, 0.5F);
        


    }

    public virtual void ApplyInfluence(influences inf)
    {
        Debug.Log("Influence " + inf + " applied at " + Time.time);
        influenceDict[inf].SetStatus(true);
        UpdateInfluenceText();
        return;
    }
	public virtual void RemoveInfluence(influences inf) {
        Debug.Log("Influence " + inf + " removed at " + Time.time);
        influenceDict[inf].SetStatus(false);
        UpdateInfluenceText();
        return;
	}

    public void RemoveInfluenceTimer(influences inf, float time)
    {
        Debug.Log("Influence " + inf + " scheduled for removal in " + time + "s at " + Time.time);
        influenceDict[inf].SetTime(time);
        return;
    }

    public virtual void UpdateInfluenceText()
    {
        influenceText.GetComponent<Text>().text = "";
        foreach (KeyValuePair<influences, InfluenceValue> influence in influenceDict) {
            if (influence.Value.GetStatus()) {
                influenceText.GetComponent<Text>().text = influenceText.GetComponent<Text>().text + "\n" + influence.Value.GetName() + "(" + String.Format("{0:0.0}", Mathf.Round(influence.Value.GetTime() - Time.time)) + "s)";
            }
        }
    }

    public virtual void FixedUpdate() {
        
    }

    public void CheckInfluenceTimers() {
        foreach (KeyValuePair<influences, InfluenceValue> influence in influenceDict) {
            if (influence.Value.GetTime() <= Time.time) {
                RemoveInfluence(influence.Key);
            }
        }
    }

    public void RedrawInfluences() {
        CheckInfluenceTimers();
        UpdateInfluenceText();
    }
}


