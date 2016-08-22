using UnityEngine;
using System.Collections;

public class MeteorCollide : MonoBehaviour {

    public float Damage;
    public GameObject Explosion;

    void OnCollisionEnter(Collider col) {
        if(col.gameObject.GetComponent<Targetable>() != null) {
            col.gameObject.GetComponent<Targetable>().ApplyDamage(Damage);
        }

        GameObject explosion = Instantiate(Explosion, col.gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().time);
        this.gameObject.SetActive(false);

    }
	
	
}
