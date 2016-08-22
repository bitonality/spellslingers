using UnityEngine;
using System.Collections;
using System;

public class Swarm : Aura {


    public float DegreesPerSecond = 90;

    public override void InitializeAura(GameObject target) {
        base.InitializeAura(target);
        target.GetComponent<ControlEntity>().ApplyInfluence(influences.ORBIT);
        target.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.ORBIT, this.Length);
        // Get a list of targets that the player has.
        foreach (GameObject playerTargets in this.Target.GetComponent<Targetable>().Targets) {
            // If the player's target also targets the player (mutual targets).
            if (this.Target.GetComponent<Targetable>().MutualTargets(playerTargets)) {
                // Push the target cubes as priority into the enemy target list
                foreach (Transform child in this.gameObject.transform) {
                    // Only add targetable children to the target array.
                    if (child.gameObject.GetComponent<Targetable>() != null) {
                        playerTargets.GetComponent<Targetable>().AddTarget(child.gameObject);
                    }
                }
            }
        }
        this.IntervalEnumerator = IntervalAura();
        this.Interval = 0.5F;
        StartCoroutine(this.IntervalEnumerator);
    }

    public override IEnumerator IntervalAura() {
        while(this.gameObject.transform.childCount > 0 && CurrentLength < this.Length) {
            CurrentLength += Interval;
            yield return new WaitForSeconds(this.Interval);
        }
        TerminateAura();
    }

    public override void TerminateAura() {
        Destroy(this.gameObject);
        this.Target.GetComponent<ControlEntity>().RemoveInfluenceTimer(influences.ORBIT, 0);
    }

    void Update() {
        // Don't run this code unless the aura has been explicitly initialized.
        if (this.Target != null) { 
           this.gameObject.transform.Rotate(Vector3.up * DegreesPerSecond * Time.deltaTime, Space.World);
      }
    }
}
