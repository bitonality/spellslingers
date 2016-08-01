using UnityEngine;
using System.Collections;

public abstract class Aura : MonoBehaviour {
    // ControlEntity target for the aura.
    public GameObject Target;

    // Number of seconds for the effect to occur.
    public float Length;

    // How often in seconds to run the aura code in the child class.
    public virtual float Interval {
        get;
        set;
    }

    // Each aura will have a different spawn location, so we need a modifier to alter the spawning location relative to the center of the Target transform
    public Vector3 Position = Vector3.zero;

    public IEnumerator IntervalEnumerator;

    public abstract void InitializeAura(GameObject target);
    public abstract IEnumerator IntervalAura();
    public abstract void TerminateAura();

}
