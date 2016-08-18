using UnityEngine;
using System.Collections;

public class HexToAura : MonoBehaviour {
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Aura;
        void Start()
    {
        GameObject.Find("Camera").GetComponent<ControlEntity>().AddTarget(Enemy1);
    }
        void OnTriggerEnter(Collider other){
            Debug.Log("I'm triggered");
            gameObject.GetComponent<ControlEntity>().CurrentTarget().GetComponent<ControlEntity>().ApplyDamage(10000); // Kills current target of entity. 
            gameObject.GetComponent<ControlEntity>().AddTarget(Enemy2);
            gameObject.GetComponent<ControlEntity>().AddTarget(Aura);
        }
    }
