﻿using UnityEngine;
using System.Collections;
using System;

public class Forcefield : Aura {
    public GameObject GameForcefield;

    public override void InitializeAura(GameObject target) {
        base.InitializeAura(target);
        target.GetComponent<ControlEntity>().ApplyInfluence(influences.FORCEFIELD);
        target.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.FORCEFIELD, this.Length);
        GameForcefield.SetActive(true);
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
        GameForcefield.SetActive(false);
        Destroy(this.gameObject);
    }
}
