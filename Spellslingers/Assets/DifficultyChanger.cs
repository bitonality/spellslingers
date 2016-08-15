using UnityEngine;
using System.Collections;

public class DifficultyChanger : MonoBehaviour
{
    public int difficulty;
   public  string Difficulty;
    void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        TextMesh textobj = GameObject.Find("DifficultyHere").GetComponent<TextMesh>();
        textobj.text = Difficulty;

    }
}
