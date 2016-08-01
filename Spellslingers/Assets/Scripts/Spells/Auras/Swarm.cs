using UnityEngine;
using System.Collections;
using System;

public class Swarm : Aura {


    public float DegreesPerSecond = 90;
    
    public override void InitializeAura(GameObject target) {
        this.Target = target;
        this.gameObject.transform.SetParent(this.Target.gameObject.transform);
        this.IntervalEnumerator = IntervalAura();
        this.Interval = 0.5F;
        StartCoroutine(this.IntervalEnumerator);
    }

    public override IEnumerator IntervalAura() {
        while(this.gameObject.transform.childCount > 0) {
            this.Target.GetComponent<ControlEntity>().Enemy.GetComponent<ControlEntity>().Enemy = this.gameObject.transform.GetChild(0).gameObject;
            yield return new WaitForSeconds(this.Interval);
        }
        TerminateAura();
    }

    public override void TerminateAura() {
        this.Target.GetComponent<ControlEntity>().Enemy = this.Target;
        Destroy(this.gameObject);
    }

    void Update() {
        this.gameObject.transform.Rotate(Vector3.up * DegreesPerSecond * Time.deltaTime, Space.Self);
    }
}
