using UnityEngine;
using System.Collections;

public class AuraSpawner : MonoBehaviour {

    public TargetableAura[] SpawnableAuras;
    public GameObject[] AuraTargeters;

   void Start() {
        InvokeRepeating("SpawnAura", 5F, 20F);
    }


    void SpawnAura() {
        Transform[] attachPoints = this.gameObject.GetComponentsInChildren<Transform>();
        System.Random rnd = new System.Random();
        int randomIndex = rnd.Next(0, attachPoints.Length);
        TargetableAura auraTemplate = SpawnableAuras[rnd.Next(0, SpawnableAuras.Length)];
        GameObject aura = Instantiate(auraTemplate, attachPoints[randomIndex].transform.position, Quaternion.identity) as GameObject;
        aura.transform.SetParent(attachPoints[randomIndex], false);
        foreach(GameObject ob in AuraTargeters) {
            ob.GetComponent<ControlEntity>().AddTarget(aura);
        }
    }
}
