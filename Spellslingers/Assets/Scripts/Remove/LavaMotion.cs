using UnityEngine;
using System.Collections;

public class LavaMotion : MonoBehaviour {
	int waveduration = 5; // Duration wave is at max height
	int minBetween = 5; // Min time between waves
	int maxBetween = 10;// Max time between waves
    float incriment = 1000; // Determines Smoothness of lava flow, lower for better performance / raise for higher performance. Definetly misspelled on purpose.
    float pass = .02F; // Passes the seconds call as floats, increases speed of lava flow, tied directly to incriment. increment / pass = time for lava to flow up and down
	float lavaHeight = 2F; // Maximum lava height, 2.0F is approximately the top of the mesh of the original stands, will increase when altering for main scene.
    void Start () {
        transform.position = new Vector3(4, 0, 0);
        StartCoroutine(startTimer());
	}

	IEnumerator startTimer() 
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minBetween, maxBetween));
            Debug.Log("Lava event occuring");
			for(float i = 0; i<lavaHeight; i+= (float)(lavaHeight / incriment)){
                yield return new WaitForSeconds(pass);
				transform.position = new Vector3(4, i, 0);
			}
			yield return new WaitForSeconds (waveduration);
            Debug.Log("Lava event ending");
			for(float num = lavaHeight  ; num>0F;num-=(float)(lavaHeight / incriment))
            {
				yield return new WaitForSeconds (pass);
                transform.position = new Vector3(4, num, 0);
		}
	}


}
}