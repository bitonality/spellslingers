using UnityEngine;
using System.Collections;

public class LightningBolt : Ultimate {


    public int Damage;


    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        target.GetComponent<Targetable>().ApplyDamage(Damage);
        this.Destroy();
    
    }

    public override void Destroy() {
        if (this.Source.GetComponent<Player>() != null) {
            Instantiate(this.Source.GetComponent<Player>().WandTemplate);
        }
        Destroy(this.gameObject, this.GetComponent<ParticleSystem>().time);
    }


}
