using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


// ControlEntity with the purpose of being destroyed once its health is low enough.
public abstract class Targetable : MonoBehaviour {

    // Outward facing for unity inspector, rest of logic is in TargetPoint variable.
    public Transform InsepctorTargetPoint;

    // A transform on the object that incoming targeted things will transfer.
    public Transform TargetPoint {
        get {
            if (InsepctorTargetPoint != null) {
                return InsepctorTargetPoint;
            }
            return gameObject.transform;
        }

        set {
            InsepctorTargetPoint = value;
        }
    }

    // Under the assumption that there is only one enemy for each player.
    public List<GameObject> Targets;

    // The health bar UI element tied to the ControlEntity.
    public GameObject HealthBar;

    // Abstract method to process the hex collision.
    public abstract void processHex(Hex h);

    // Max health value set in the inspector.
    public float MaxHealth;

    // Current health of the player.
    public float Health {
        get;
        set;
    }


    public virtual void Awake() {
        this.Targets = new List<GameObject>();
        this.Health = this.MaxHealth;
        this.TargetPoint = this.InsepctorTargetPoint;
    }

    // Returns if the player is dead or not.
    public bool IsDead() {
        return (Health <= 0);
    }
    public bool MutualTargets(GameObject target) {
        if (target == null) {
            return false;
        }

        if (target.GetComponent<Targetable>().Targets.Contains(this.gameObject) && Targets.Contains(target)) {

            return true;
        }
        return false;
    }

    public GameObject CurrentTarget() {
        for (int i = Targets.Count - 1; i >= 0; i--) {
            if (Targets[i] != null) {
                return Targets[i];
            }
            else {
                Targets.RemoveAt(i);
            }
        }

        return null;
    }


    // Safely checks for extra additions and adds an element
    // TODO: Consider making sure no duplicates anywhere in the list (unnecessary right now).
    public void AddTarget(GameObject target) {
        if (Targets[Targets.Count - 1] != target) {
            Targets.Add(target);
        }
    }


    // Safely update the health bar even if there isn't one.
    public void ApplyDamage(float damage) {
        // Update healthbar UI with new health amount.
        this.Health -= damage;
        // Avoid spilling over max health amount.
        if(this.Health > this.MaxHealth) {
            this.Health = this.MaxHealth;
        }
        if (this.HealthBar != null) {
            this.HealthBar.GetComponent<Image>().fillAmount = (float)(this.Health / this.MaxHealth);
        }
    }


}
