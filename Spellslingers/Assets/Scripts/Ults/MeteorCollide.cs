using UnityEngine;
using System.Collections;

public class MeteorCollide : MonoBehaviour {

    public float Damage;
    public GameObject Explosion;

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.GetComponent<Targetable>() != null || col.tag == "MeteorCleanup") {
            col.gameObject.GetComponent<Targetable>().ApplyDamage(Damage);
            GameObject explosion = Instantiate(Explosion, col.gameObject.transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }



    }
	
	
}
