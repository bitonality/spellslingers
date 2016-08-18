using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : Ultimate {


    public GameObject HexTemplate;
    public int NumberOfHexes;
    private int HexCounter = 0;


    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        InvokeRepeating("IntervalLightning", 0.1F, 0.2F);
    }

    void IntervalLightning() {
        if (HexCounter > NumberOfHexes) {
            CancelInvoke("IntervalLightning");
            this.Destroy();
        }
        else {
            HexCounter++;
            this.Source.GetComponent<ControlEntity>().CastHex(HexTemplate.GetComponent<Hex>(), this.Source.GetComponent<Targetable>().TargetPoint.gameObject, this.Source.GetComponent<Targetable>().CurrentTarget(), 10F, 4F);
        }

    }
}
