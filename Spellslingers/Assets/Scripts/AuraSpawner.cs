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
        Transform[] attachPoints = this.gameObject.GetComponentsInChildren<Transform>();
        List<Transform> validPoints = new List<Transform>(attachPoints);
        for(int i = validPoints.Count - 1; i >=0; i--) {
            if(validPoints[i].childCount != 0) {
                validPoints.RemoveAt(i);
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
            ob.GetComponent<ControlEntity>().AddTarget(aura);
        }
    }
}
