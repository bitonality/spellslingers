using UnityEngine;
using System.Collections;

public class DifficultyChanger : MonoBehaviour
{
    public int difficulty;
   public  string Difficulty;
    void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        UnityEngine.UI.Text textobj = GameObject.Find("DifficultyHere").GetComponent<UnityEngine.UI.Text>();
        textobj.text = Difficulty;

    }
}
