using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : Ultimate {


    public GameObject HexTemplate;
    public int NumberOfHexes;
    private int HexCounter;


    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        InvokeRepeating("IntervalLightning", 0.1F, 0.2F);
    }

    private class LightningProjectile {
        public GameObject Hex;
        public GameObject Source;

        public LightningProjectile(GameObject h, GameObject s) {
            this.Hex = h;
            this.Source = s;
        }
    }

    void IntervalLightning() {
        if (HexCounter > NumberOfHexes) {
            CancelInvoke("IntervalLightning");
            this.Destroy();
        }
        else {
            this.Source.GetComponent<ControlEntity>().CastHex(HexTemplate.GetComponent<Hex>(), this.Source.transform, this.Target.transform, 10F, 4F);
        }

    }
}
