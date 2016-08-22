using UnityEngine;
using System.Collections;

public class LightningBolt : Ultimate {


    public int Damage;


    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);
        this.gameObject.transform.position = target.transform.position;
        if(target.GetComponent<ControlEntity>() != null) {
            this.gameObject.transform.position = target.GetComponent<ControlEntity>().AuraParticleAttachPoint.transform.position;
        }
        target.GetComponent<Targetable>().ApplyDamage(Damage);
        this.Destroy();
    
    }

    public override void Destroy() {
        if (this.Source.GetComponent<Player>() != null) {
            Instantiate(this.Source.GetComponent<Player>().WandTemplate);
        }
        Destroy(this.gameObject, 10F);
    }


}
