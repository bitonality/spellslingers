using UnityEngine;
using System.Collections;

public class TutorialTargetting : MonoBehaviour {

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Aura;
     void Start()
    {
        gameObject.GetComponent<ControlEntity>().AddTarget(Enemy1);
    }
 public void tutorialSecondStage()
    {
        gameObject.GetComponent<ControlEntity>().CurrentTarget().GetComponent<ControlEntity>().ApplyDamage(10000); // Kills first entity
        gameObject.GetComponent<ControlEntity>().AddTarget(Enemy2);
        gameObject.GetComponent<ControlEntity>().AddTarget(Aura);
    }   
    
}
