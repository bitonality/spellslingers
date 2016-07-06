using UnityEngine;
using System.Collections;

public class rockEvent : MonoBehaviour {
    System.Random rnd = new System.Random();
    int minTimeBetween = 10; //seconds
    int maxTimeBetween = 21; //seconds

    void Start()
    {
        StartCoroutine(startTimer());
    }

	IEnumerator startTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(rnd.Next(minTimeBetween, maxTimeBetween));
            GameObject drop = Instantiate(Resources.Load("RockMaster")) as GameObject;
            drop.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
