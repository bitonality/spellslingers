using UnityEngine;
using System.Collections;
using VRTK;


// Class to handle the collision of hexes with objects.
public class HexCollide : MonoBehaviour {
    // Particle system to instantiate when the object collides with something.
    public GameObject explosion;

    // Number of physics collisions the hex can have before being destroyed.
    public int numLegalCollisions;

    // Tracks the number of collisions the hex has had.
    private int numCollisions = 0;

    void OnCollisionEnter(Collision col) {
        // Create an explosion at the collision point.

        if (explosion != null) {
            GameObject effect = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        }

        if(this.gameObject.GetComponent<Hex>().ExplosionSound != null) {
            this.gameObject.GetComponent<Hex>().Audio.clip = this.gameObject.GetComponent<Hex>().ExplosionSound;
            AudioSource.PlayClipAtPoint(this.gameObject.GetComponent<Hex>().Audio.clip, this.gameObject.transform.position);
        }


        // Check that the spell isn't hitting another spell by the same sender
        if(col.gameObject.GetComponent<Hex>() != null && col.gameObject.GetComponent<Hex>().Source == this.gameObject.GetComponent<Hex>().Source || (col.gameObject.GetComponent<Targetable>() != null && this.gameObject.GetComponent<Hex>().Source == col.gameObject.GetComponent<Targetable>())) {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), col.gameObject.GetComponent<Collider>());
            return;
        }

        // If the spell collides with a ControlEntity.
        if (col.gameObject.GetComponent<Targetable>() != null ) {
            // Process the spell for the specific hex and ControlEntity
            col.gameObject.GetComponent<Targetable>().processHex(this.GetComponent<Hex>());
            return;
        }

        if (numCollisions < numLegalCollisions) {
            // Processing for under or at collision limit.
            numCollisions++;
        }
        else {
            // Processing for above collision limit.
            this.gameObject.GetComponent<Hex>().Destroy();
        }
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Forcefield") {


            Hex h = this.gameObject.GetComponent<Hex>();

            // If the player is shooting out of the force field.
            if (h.Source.GetComponent<ControlEntity>() != null && (h.Source.GetComponent<ControlEntity>().influenceDict[influences.FORCEFIELD].GetStatus() == true && col.GetComponent<Aura>().Target == h.Source)) {
                return;
            }
            // If the enemy player shooting into it has haste.
            if(h.Source.GetComponent<ControlEntity>() != null && h.Source.GetComponent<ControlEntity>().influenceDict[influences.HASTE].GetStatus() == true) {
                return;
            }


            if (explosion != null) {
                GameObject effect = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
                Destroy(effect, effect.GetComponent<ParticleSystem>().time);
            }

            // Hexes will never despawn because of this, produces swarm-like effect.
            h.MaxRotation = 360;
            HomingProjectile hp = this.gameObject.GetComponent<HomingProjectile>();
            hp.Sensitivity = 50;
            this.gameObject.GetComponent<Rigidbody>().velocity *= -1;
            h.Damage = h.Damage / 2;
            this.gameObject.transform.rotation = Quaternion.Inverse(this.gameObject.transform.rotation);

            GameObject cachedTarget = this.gameObject.GetComponent<HomingProjectile>().Target;

            this.gameObject.GetComponent<HomingProjectile>().Target = h.Source.gameObject;
            h.Source = cachedTarget.GetComponent<ControlEntity>();
            
        }
    }
}