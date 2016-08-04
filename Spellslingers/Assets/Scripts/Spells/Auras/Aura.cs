using UnityEngine;
using System.Collections;

public abstract class Aura : MonoBehaviour {
    // ControlEntity target for the aura.
    public GameObject Target {
        get;
        set;
    }

    public GameObject AuraParticleEffect;

    private GameObject InstantiatedParticleEffect;

    // Number of seconds for the effect to occur.
    public float Length;

    // Incrementing value that tracks the current lifetime of the aura.
    public float CurrentLength {
        get;
        set;
    }

    // How often in seconds to run the aura code in the child class.
    public virtual float Interval {
        get;
        set;
    }

    // Each aura will have a different spawn location, so we need a modifier to alter the spawning location relative to the center of the Target transform
    public Vector3 Position = Vector3.zero;

    public IEnumerator IntervalEnumerator;

    public virtual void InitializeAura(GameObject target) {
        this.Target = target;
        this.gameObject.transform.position = this.Target.gameObject.transform.position + this.Position;
        this.gameObject.transform.SetParent(this.Target.gameObject.transform);
        if(AuraParticleEffect != null) {
            Vector3 auraPosition = Target.transform.position;
            if(Target.GetComponent<ControlEntity>().AuraParticleAttachPoint != null) {
                auraPosition = Target.GetComponent<ControlEntity>().AuraParticleAttachPoint.transform.position;
            }

            // Create an explosion at the collision point.
            this.InstantiatedParticleEffect = (GameObject) Instantiate(AuraParticleEffect, auraPosition, Target.transform.rotation);
        }
    }
    public abstract IEnumerator IntervalAura();
    public virtual void TerminateAura() {
        // Schedule the destruction of the particle system.
       if(this.InstantiatedParticleEffect != null) {
            Destroy(this.InstantiatedParticleEffect);
        } 
    }

    public virtual void Start() {
        this.CurrentLength = 0;
    }

}
