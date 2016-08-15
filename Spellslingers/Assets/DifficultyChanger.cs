using UnityEngine;
using System.Collections;

public class DifficultyChanger : MonoBehaviour
{
    public int difficulty;
   public  string Difficulty;
    void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        GameObject.Find("DifficultyHere").GetComponent<UnityEngine.UI.Text>().text = Difficulty;
        
    }
}
