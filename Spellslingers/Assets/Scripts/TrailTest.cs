using UnityEngine;
using System.Collections;

public class TrailTest : MonoBehaviour {
	double i = -2.0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 ((float)i, (float)3.569, (float)-3.51);
		i += 0.04;
	}
}