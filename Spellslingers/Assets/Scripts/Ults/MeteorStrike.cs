using UnityEngine;
using System.Collections;

public class MeteorStrike : Ultimate {

    public int NumberOfMeteors;
    private int MeteorCounter = 0;
    public GameObject MeteorTemplate;

    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        InvokeRepeating("Meteor", 0.1F, 0.1F);
    }

    private void Meteor() {
        Vector3 center = this.Target.transform.position;
        Vector3 mod = new Vector3(Random.Range(0, 2), Random.Range(10, 20), Random.Range(0, 2));
        GameObject meteor = Instantiate(MeteorTemplate, center + mod, Quaternion.identity) as GameObject;
        MeteorCounter++;
        if (MeteorCounter > NumberOfMeteors) {
            this.Destroy();
        }
    }




}
