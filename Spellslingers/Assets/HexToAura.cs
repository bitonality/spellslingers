using UnityEngine;
using System.Collections;

public class HexToAura : MonoBehaviour {
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Aura;
        void Start()
    {
       
    }
        void OnTriggerEnter(Collider other){
            Debug.Log("I'm triggered");
            other.gameObject.GetComponentInParent<ControlEntity>().CurrentTarget().GetComponent<ControlEntity>().ApplyDamage(10000); // Kills current target of player
            other.gameObject.GetComponentInParent<ControlEntity>().AddTarget(Enemy2);
        Debug.Log("I reach here");
            other.gameObject.GetComponentInParent<ControlEntity>().AddTarget(Aura);
        }
    }
