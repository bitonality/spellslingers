using UnityEngine;
using System.Collections;
using System;

public class Swarm : Aura {


    public float DegreesPerSecond = 90;

    public override void InitializeAura(GameObject target) {
        this.Target = target;
        this.gameObject.transform.position = this.Target.gameObject.transform.position + this.Position;
        this.gameObject.transform.SetParent(this.Target.gameObject.transform);
        // Get a list of targets that the player has.
        foreach (GameObject playerTargets in this.Target.GetComponent<Targetable>().Targets) {
            // If the player's target also targets the player (mutual targets).
            if (this.Target.GetComponent<Targetable>().MutualTargets(playerTargets)) {
                // Push the target cubes as priority into the enemy target list
                foreach (Transform child in this.gameObject.transform) {
                    playerTargets.GetComponent<Targetable>().AddTarget(child.gameObject);
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
        // Don't run this code unless the aura has been explicitly initialized.
        if (this.Target != null) { 
           this.gameObject.transform.Rotate(Vector3.up * DegreesPerSecond * Time.deltaTime, Space.World);
      }
    }
}
