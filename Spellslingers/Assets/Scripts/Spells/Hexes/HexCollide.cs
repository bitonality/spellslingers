﻿using UnityEngine;
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
        Debug.Log("HexCollide: Colliding with " + col.gameObject);
        // Create an explosion at the collision point.
        //GameObject effect = (GameObject)Instantiate(explosion, transform.position, transform.rotation);

        // Schedule the destruction of the particle system.
        //Destroy(effect, effect.GetComponentInChildren<ParticleSystem>().duration);

        // If the spell collides with a ControlEntity.
        if (col.gameObject.GetComponent<Targetable>() != null) {
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

    void OnTriggerEnter(Collision col) {
        if(col.gameObject.tag == "Forcefield") {
            Hex h = this.gameObject.GetComponent<Hex>();
            h.MaxRotation = 360;
            Invoke("ResetMaxRotation", 0.5F);
            this.gameObject.GetComponent<Rigidbody>().velocity *= -1;
            this.gameObject.GetComponent<HomingProjectile>().Target = h.Source.TargetPoint;
        }
    }

    void ResetMaxRotation() {
        this.gameObject.GetComponent<Hex>().MaxRotation = 90;
    }
}