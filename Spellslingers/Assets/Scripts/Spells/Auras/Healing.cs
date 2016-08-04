using UnityEngine;
using System.Collections;
using System;

public class Healing : Aura {

    public float HealthToRestore;
    private float TimeRestored = 0;
 

    public override void InitializeAura(GameObject target) {
        base.InitializeAura(target);
        this.TimeRestored = this.Length;
        this.IntervalEnumerator = IntervalAura();
        StartCoroutine(this.IntervalEnumerator);
        
    }

    public override IEnumerator IntervalAura() {
        while (CurrentLength < Length) {
            CurrentLength += Interval;
            yield return new WaitForSeconds(this.Interval);
        }
        TerminateAura();
    }

    public override void TerminateAura() {
        base.TerminateAura();
        Destroy(this.gameObject);
    }

    public void FixedUpdate() {
        if(this.Target.GetComponent<ControlEntity>() != null) {
            float amountToHeal = (HealthToRestore / TimeRestored) * Time.deltaTime;
            this.Target.GetComponent<ControlEntity>().ApplyDamage(-amountToHeal);
            this.Length -= Time.deltaTime;
            this.HealthToRestore -= amountToHeal;
        }
    }
}
