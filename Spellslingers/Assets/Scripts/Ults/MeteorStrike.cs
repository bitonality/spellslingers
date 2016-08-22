using UnityEngine;
using System.Collections;

public class MeteorStrike : Ultimate {

    public int NumberOfMeteors;
    public GameObject MeteorTemplate;

    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        Vector3 center = target.transform.position;
        for (int i = 0; i < NumberOfMeteors; i++) {
            GameObject meteor = Instantiate(MeteorTemplate, (center + new Vector3(0, 5, 0)) + (Random.insideUnitSphere * 3), Quaternion.identity) as GameObject;
        }
        this.Destroy();
    }






}
