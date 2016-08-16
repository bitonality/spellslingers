using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraSpawner : MonoBehaviour {

    public GameObject[] SpawnableAuras;
    public GameObject[] AuraTargeters;

   void Awake() {
        InvokeRepeating("SpawnAura", 5F, 20F);
    }


    void SpawnAura() {
        List<Transform> validPoints = new List<Transform>();
        foreach(Transform t in this.gameObject.transform) {
            if(t.transform.childCount == 0) {
                validPoints.Add(t);
            }
        }


        if(validPoints.Count == 0) {
            return;
        }

        System.Random rnd = new System.Random();
        int randomIndex = rnd.Next(0, validPoints.Count);
        GameObject auraTemplate = SpawnableAuras[rnd.Next(0, SpawnableAuras.Length)];
        GameObject aura = Instantiate(auraTemplate, validPoints[randomIndex].transform.position, auraTemplate.transform.rotation) as GameObject;
        aura.transform.SetParent(validPoints[randomIndex]);
        foreach(GameObject ob in AuraTargeters) {
            if (ob != null) {
                ob.GetComponent<ControlEntity>().AddTarget(aura);
            }
        }
    }
}
