using UnityEngine;
using System.Collections;

public class HexToAura : MonoBehaviour {
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Aura;
        void Start()
    {
       
        gameObject.GetComponentInParent<ControlEntity>().AddTarget(Aura);
    }
        void OnTriggerEnter(Collider other){
            Debug.Log("I'm triggered");
            other.gameObject.GetComponentInParent<ControlEntity>().CurrentTarget().GetComponent<ControlEntity>().ApplyDamage(10000); // Kills current target of entity. 
            other.gameObject.gameObject.GetComponentInParent<ControlEntity>().AddTarget(Enemy2);
            gameObject.GetComponentInParent<ControlEntity>().AddTarget(Aura);
        }
    }
