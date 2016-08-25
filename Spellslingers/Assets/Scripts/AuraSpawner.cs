using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class AuraSpawner : MonoBehaviour {
    private static System.Random rng = new System.Random();
    public GameObject[] SpawnableAuras;
    public GameObject[] AuraTargeters;
    public AudioClip SpawnSound;

   void Awake() {
        InvokeRepeating("SpawnAura", 20F, 20F);
        InvokeRepeating("Cleanup", 25F, 20F);
    }


    void SpawnAura() {
        this.gameObject.GetComponentInChildren<AudioSource>().clip = SpawnSound;
        this.gameObject.GetComponentInChildren<AudioSource>().Play();
        List<Transform> validPoints = new List<Transform>();
        foreach (Transform t in this.gameObject.transform) {
            if (t.transform.childCount == 0 && t.gameObject.GetComponent<AudioSource>() == null) {
                validPoints.Add(t);
            }
        }

        Shuffle(validPoints);

        for (int i = 0; i < SpawnableAuras.Length; i++) {
            GameObject auraTemplate = SpawnableAuras[i];
            GameObject aura = Instantiate(auraTemplate, validPoints[i].transform.position, auraTemplate.transform.rotation) as GameObject;
            aura.transform.SetParent(validPoints[i]);
        }
        
        foreach (GameObject ob in AuraTargeters) {
            foreach (Transform t in this.gameObject.transform) {
                if (ob != null) {
                    if(t.gameObject.GetComponent<AudioSource>() == null) {
                        
                        ob.GetComponent<ControlEntity>().AddTarget(t.GetChild(0).gameObject);
                    }
                    
                }
            }
        }
        
    }

    void Cleanup() {
        foreach (Transform t in this.gameObject.transform) {
            if (t.transform.childCount != 0) {
                Destroy(t.transform.GetChild(0).gameObject);
            }
        }
    }

    public static void Shuffle(List<Transform> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            Transform value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
