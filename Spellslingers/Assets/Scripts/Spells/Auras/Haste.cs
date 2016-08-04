using UnityEngine;
using System.Collections;
using System;

public class Haste : Aura {

    public float SpellSpeedModifier;

    public override void InitializeAura(GameObject target) {
        base.InitializeAura(target);
        this.IntervalEnumerator = IntervalAura();
        StartCoroutine(this.IntervalEnumerator);
        this.Target.GetComponent<ControlEntity>().SpellSpeedModifier = SpellSpeedModifier;
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
        if(this.Target.GetComponent<ControlEntity>() != null) {
            this.Target.GetComponent<ControlEntity>().SpellSpeedModifier = this.Target.GetComponent<ControlEntity>().DefaultSpellSpeedModifier;
        } 
        Destroy(this.gameObject);
    }
}
