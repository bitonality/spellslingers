using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine.UI;

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

    public Dictionary<influences, InfluenceValue> influenceDict = new Dictionary<influences, InfluenceValue>();

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

	public override void Awake() {
        base.Awake();
        UltimateMode = false;
        UltimateCounter = 0;
        this.SpellSpeedModifier = this.DefaultSpellSpeedModifier;
        this.ActiveHexes = new HashSet<Hex>();
        influenceDict.Add(influences.DISARM, new InfluenceValue(false, 0));
        influenceDict.Add(influences.STUN, new InfluenceValue(false, 0));
        this.Targets.Add(InitialEnemy);
        this.Targets.Add(InitialAura);

    }

    public virtual object ApplyInfluence(influences inf)
    {
        Debug.Log("Influence " + inf + " applied at " + Time.time);
        influenceDict[inf].SetStatus(true);
        if (gameObject.GetComponent<NewAI>() != null)
        {
            GetComponent<NewAI>().UpdateInfluenceText();
        }
        return null;
    }
	public virtual object RemoveInfluence(influences inf) {
        Debug.Log("Influence " + inf + " removed at " + Time.time);
        influenceDict [inf].SetStatus(false);
        if (gameObject.GetComponent<NewAI>() != null)
        {
            GetComponent<NewAI>().UpdateInfluenceText();
        }
        return null;
	}

    public object RemoveInfluenceTimer(influences inf, float time)
    {
        Debug.Log("Influence " + inf + " scheduled for removal in " + time + "s at " + Time.time);
        influenceDict[inf].setTime(time);
        StartCoroutine(IERemoveInfluence(inf, time));
        return null;
    }

    private IEnumerator IERemoveInfluence(influences inf, float time)
    {
        yield return new WaitForSeconds(time);
        RemoveInfluence(inf);
    }
}
