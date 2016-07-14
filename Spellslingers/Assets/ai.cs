using UnityEngine;
using System.Collections;


public class ai : MonoBehaviour {
	//Only until we get the spells setup, so we don't get errors
	spell dizzy = new spell();
	spell damage = new spell();
	spell disarm = new spell();

	//Starting here we get actual keep-this code
	//Include the framework
	aiBase framework = new aiBase();

	//Level is 1/2/3, set somewhere else but set here for now.
	static int level = 1; 
	//Speed is 5/5.5/6 m/s
	float speed = (4.5F + (0.5F * level));

	void Start () {
		//If level 3 (hardest), immediately shoot a dizzy spell
		if (level == 3) {
			//TODO: Make the spell aim at the player
			framework.shootSpell (dizzy, new Vector3(0, 0, 0));
		}

		//Once every 2/1/.66 seconds, shoot a damage spell
		shootSpellRepeat(2/level, damage);

		//Once every 3/2/1 shoot a dizzy spell
		shootSpellRepeat((4 + (-1 * level)), dizzy);

		//Once every 5/4/3 seconds, shoot a disarm spell
		shootSpellRepeat((6 + (-1 * level)), disarm);

		//Once every 0.33 seconds, check if the AI is in danger
		checkSafety(0.33F);
	}

	IEnumerator shootSpellRepeat(float secondsBetween, spell spellType)
	{
		while(true)
		{
			yield return new WaitForSeconds(secondsBetween);
			//TODO: Have georgie set this up. Also, make it aim at the player
			framework.shootSpell (spellType, new Vector3(0, 0, 0)); //Again, georgie does this
		}
	}

	IEnumerator checkSafety(float secondsBetween)
	{
		while(true)
		{
			yield return new WaitForSeconds(secondsBetween);
			if (framework.isInDanger ()) {
				Debug.Log ("In danger, moving");
				//Move 
				Vector3 position = this.gameObject.transform.position;
				Vector3 newPosition = new Vector3 (position.x + 5, position.y, position.z);
				//Last arg was calculated with equation time = distance/velocity (speed)
				framework.move(this.gameObject, newPosition, (Vector3.Distance(position, newPosition) / speed));
			} else {
				Debug.Log ("No longer in danger");
				framework.cancelMove (this.gameObject);
			}
		}
	}
}
