using UnityEngine;
using System.Collections;

public class LavaMotion : MonoBehaviour {
    public readonly int waveduration = 5;
    // Use this for initialization
    void Start () {
        transform.position = new Vector3(4, 0, 0);
        StartCoroutine(startTimer());
	}

    IEnumerable startTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            transform.position = new Vector3(4, (double waveTime = (double)System.Math.Ceiling(Time.fixedTime))Time.fixedTime * .018, 0);
            double waveTime = (double)System.Math.Ceiling(Time.fixedTime) + 30 * 90;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.fixedTime *.0018 <= 1.08)
        {
            
        }
    }
}
