using UnityEngine;
using System.Collections;
using System;

public class Swarm : Aura {


    public float DegreesPerSecond = 90;
    public GameObject SwarmCubeTemplate;

    public override void InitializeAura(GameObject target) {
        Target = target;
        gameObject.transform.SetParent(Target.gameObject.transform);
        GameObject cube = Instantiate(SwarmCubeTemplate, gameObject.transform.position + new Vector3(1, 0, 0), Quaternion.identity) as GameObject;
        cube.transform.SetParent(gameObject.transform);
        // Get a list of targets that the player has.
        foreach (GameObject playerTargets in Target.GetComponent<ControlEntity>().Targets) {
            // If the player's target also targets the player (mutual targets).
            if (Target.GetComponent<ControlEntity>().MutualTargets(playerTargets)) {
                // Push the target cubes as priority into the enemy target list
                foreach (Transform child in gameObject.transform) {
                    playerTargets.GetComponent<ControlEntity>().AddTarget(child.gameObject);
                }
            }
        }
        IntervalEnumerator = IntervalAura();
        Interval = 0.5F;
        StartCoroutine(IntervalEnumerator);
    }

    public override IEnumerator IntervalAura() {
        while(gameObject.transform.childCount > 0) {
            yield return new WaitForSeconds(Interval);
        }
        TerminateAura();
    }

    public override void TerminateAura() {
        Destroy(gameObject);
    }

    void Update() {
        // Don't run this code unless the aura has been explicitly initialized.
        if (Target != null) {
            gameObject.transform.Rotate(Vector3.up * DegreesPerSecond * Time.deltaTime, Space.Self);
      }
    }
}
