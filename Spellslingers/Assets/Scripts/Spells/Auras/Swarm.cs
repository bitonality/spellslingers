﻿using UnityEngine;
using System.Collections;
using System;

public class Swarm : Aura {


    public float DegreesPerSecond = 90;
    
    public override void InitializeAura(GameObject target) {
        this.Target = target;
        this.gameObject.transform.SetParent(this.Target.gameObject.transform);
        // Get a list of targets that the player has.
        foreach (GameObject playerTargets in this.Target.GetComponent<ControlEntity>().Targets) {
            // If the player's target also targets the player (mutual targets).
            if (this.Target.GetComponent<ControlEntity>().MutualTargets(playerTargets)) {
                // Push the target cubes as priority into the enemy target list
                foreach (Transform child in this.gameObject.transform) {
                    playerTargets.GetComponent<ControlEntity>().AddTarget(child.gameObject);
                }
            }
        }
        this.IntervalEnumerator = IntervalAura();
        this.Interval = 0.5F;
        StartCoroutine(this.IntervalEnumerator);
    }

    public override IEnumerator IntervalAura() {
        while(this.gameObject.transform.childCount > 0) {
            yield return new WaitForSeconds(this.Interval);
        }
        TerminateAura();
    }

    public override void TerminateAura() {
        Destroy(this.gameObject);
    }

    void Update() {
        this.gameObject.transform.Rotate(Vector3.up * DegreesPerSecond * Time.deltaTime, Space.Self);
    }
}