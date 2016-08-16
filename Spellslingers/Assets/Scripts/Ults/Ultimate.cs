using UnityEngine;
using System.Collections;

public abstract class Ultimate : MonoBehaviour {

    [HideInInspector]
    public GameObject Source;
    [HideInInspector]
    public GameObject Target;

    public virtual void Cast(GameObject source, GameObject target) {
        this.Source = source;
        this.Target = target;
    }


}
