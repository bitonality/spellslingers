using UnityEngine;
using System.Collections;

public class ai : MonoBehaviour {
	int level = 1; //1-3, set somewhere else but set here for now.
	float speed;

	void Start () {
		//If level 3 (hardest), immediately shoot a dizzy spell
		if (level == 3) {
			aiBase.shootSpell (dizzy);
		}

		//Once every 2/1/.66 seconds, shoot a damage spell
		shootSpellRepeat(2/level, damage);

		//Once every 0.33 seconds, check if the AI is in danger
		checkSafety(0.33);

		//Determine what speed should be. Speed is 5/5.5/6 m/s
		speed = 4.5 + (0.5 * level);
	}

	IEnumerator shootSpellRepeat(float secondsBetween, spell spellType)
	{
		while(true)
		{
			yield return new WaitForSeconds(secondsBetween);
			aiBase.shootSpell (spellType); //Again, georgie does this
		}
	}

	IEnumerator checkSafety(float secondsBetween)
	{
		while(true)
		{
			yield return new WaitForSeconds(secondsBetween);
			if (aiBase.isInDanger ()) {
				Debug.Log ("In danger, moving");
				//Move 
				Vector3 position = this.gameObject.transform.position;
				Vector3 newPosition = new Vector3 (position.x + 5, position.y, position.z);
				//Last arg was calculated with equation time = distance/velocity (speed)
				aiBase.move(this.gameObject, newPosition, (Vector3.Distance(position, newPosition) / speed));
			} else {
				Debug.Log ("Not in danger");
			}
		}
	}
}
