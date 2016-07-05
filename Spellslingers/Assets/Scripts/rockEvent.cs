using UnityEngine;
using System.Collections;

public class rockEvent : MonoBehaviour {
    System.Random rnd = new System.Random();
    int minTimeBetween = 1; //second
    int maxTimeBetween = 2; //second

    void Start()
    {
        StartCoroutine(startTimer());
    }

    IEnumerator startTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(rnd.Next(minTimeBetween, maxTimeBetween));
            Debug.Log("Triggered");
            GameObject drop = Instantiate(Resources.Load("RockMaster")) as GameObject;
            drop.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
