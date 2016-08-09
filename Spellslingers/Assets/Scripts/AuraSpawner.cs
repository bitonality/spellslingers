using UnityEngine;
using System.Collections;

public class AuraSpawner : MonoBehaviour {

    public GameObject[] SpawnableAuras;
    public GameObject[] AuraTargeters;

   void Awake() {
        InvokeRepeating("SpawnAura", 5F, 20F);
    }


    void SpawnAura() {
        Transform[] attachPoints = this.gameObject.GetComponentsInChildren<Transform>();
        System.Random rnd = new System.Random();
        int randomIndex = rnd.Next(0, attachPoints.Length);
        GameObject auraTemplate = SpawnableAuras[rnd.Next(0, SpawnableAuras.Length)];
        GameObject aura = Instantiate(auraTemplate, attachPoints[randomIndex].transform.position, auraTemplate.transform.rotation) as GameObject;
        Debug.Log(aura);
        aura.transform.SetParent(attachPoints[randomIndex]);
        foreach(GameObject ob in AuraTargeters) {
            ob.GetComponent<ControlEntity>().AddTarget(aura);
        }
    }
}
