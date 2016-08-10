using UnityEngine;
using System.Collections;

public abstract class Aura : MonoBehaviour {
    // ControlEntity target for the aura.
    public GameObject Target {
        get;
        set;
    }

    public GameObject AuraParticleEffect;

    // Number of seconds for the effect to occur.
    public float Length;

    // Incrementing value that tracks the current lifetime of the aura.
    public float CurrentLength {
        get;
        set;
    }

    // How often in seconds to run the aura code in the child class.
    public float Interval;

    // Each aura will have a different spawn location, so we need a modifier to alter the spawning location relative to the center of the Target transform
    public Vector3 Position = Vector3.zero;

    public IEnumerator IntervalEnumerator;

    public virtual void InitializeAura(GameObject target) {
        this.Target.GetComponent<ControlEntity>().IncrementUltimateCounter(1);
        this.Target = target;
        this.gameObject.transform.position = this.Target.gameObject.transform.position + this.Position;
        this.gameObject.transform.SetParent(this.Target.gameObject.transform);
        if(AuraParticleEffect != null) {
            if(Target.GetComponent<ControlEntity>().AuraParticleAttachPoint != null) {
                AuraParticleEffect.transform.position = Target.GetComponent<ControlEntity>().AuraParticleAttachPoint.transform.position;
            }
            

        }
    }
    public abstract IEnumerator IntervalAura();
    public virtual void TerminateAura() {
        // Schedule the destruction of the particle system.
       if(this.gameObject.GetComponentInChildren<ParticleSystem>() != null) {
            Destroy(this.gameObject.GetComponentInChildren<ParticleSystem>());
        } 
    }

    public virtual void Start() {
        this.CurrentLength = 0;
    }

}
