using UnityEngine;
using System.Collections;

public abstract class Ultimate : MonoBehaviour {

    [HideInInspector]
    GameObject Source;
    [HideInInspector]
    GameObject Target;

    public virtual void Cast(GameObject source, GameObject target) {
        this.Source = source;
        this.Target = target;
    }


}
