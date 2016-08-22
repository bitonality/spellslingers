using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class NewAI : ControlEntity

{
    //Difficulty - Easy/Normal/Hard: 1/2/3
    // TODO: Make a way to set this in-game
    [HideInInspector]
    public int Difficulty;

    public GameObject[] Ultimates;

    //time until next state change 
    private float timeUntilChange = 0;

    //Time until the AI is allowed to shoot after being idle
    public float IdlePauseTime = 3;

    // A difficulty modifier is the amount it will increase/decrease (depending on the context) if the difficulty is not set to 2.
    // For example, IdleDifficultyMod increases/decreases IdlePauseTime if difficulty is 1/3, respectivly. However, SpeedDifficultyMod decreases/increases speed if the difficulty is 1/3.
    public float IdleMod = 1;

    //Speed (m/s) of the AI
    public float speed;
    public float SpeedMod = 10;

    // Delay the AI must wait while in PRESHOOT and NOT be in danger
    public float PreshootDelay = 1;
    public float PreshootDelayMod = .5f;

    // MaxAngle in CastListener. Default is 30
    public float MaxAngleMod = 10;

    private float defaultSpeed;

    //List of spells the AI is allowed to shoot
    public Hex[] spellsToShoot;

    private Vector3 originalPosition;

    public enum validStates
    {
        STARTUP,
        STUNNED,
        HIT,
        IDLE,
        DANGER,
        PRESHOOT,
        SHOOTING,
        POSTSHOOT,
        DEAD
    }



    // Make sure it can't leave the AI boundry.
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "AIBoundry")
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce((originalPosition - gameObject.transform.position) * speed, ForceMode.Impulse);
        }
    }

    public Queue<validStates> currentAction;

    //Called every 0.02 seconds
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentAction.Count <= 0)
        {
            //Something went wrong with starting the AI
            Debug.LogWarning("currentAction size is 0, meaning awake() was not called before fixedUpdate()");
            return;
        }
        if (currentAction.Count != 1 && timeUntilChange <= Time.time)
        {
            //Pop leading item off the queue and call justLeft with it
            justLeft(currentAction.Dequeue(), getCurrentState());
        }
        else
        {
            //If there was not a state change, call currentExclusive
            currentExclusive(getCurrentState());
        }
        //Call currentInclusive, regardless if there was a state change or not
        currentInclusive(getCurrentState());
    }

    //Called only when changing state
    private object justLeft(validStates oldState, validStates newState)
    {
        Debug.Log("Changing state from  " + oldState + " to " + newState + " at time " + Time.time);
        switch (newState)
        {
            case validStates.HIT:
                currentAction.Enqueue(validStates.DANGER);
                break;
            case validStates.IDLE:
                //Move in a random direction
                Vector3 Destination = new Vector3(UnityEngine.Random.Range(-5f, 5f) * speed, 0, UnityEngine.Random.Range(-5f, 5f) * speed);
                gameObject.GetComponent<Rigidbody>().AddForce(Destination, ForceMode.Impulse);
                //Schedule interruptable state change in {{ IdlePauseTime }} seconds
                timeUntilChange = Time.time + IdlePauseTime;
                currentAction.Enqueue(validStates.PRESHOOT);
                break;
            case validStates.PRESHOOT:
                //Stop movement
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                //Wait {{ PreshootDelay }} seconds. If the AI doesn't get in danger, move this will move into SHOOTING stage.
                timeUntilChange = Time.time + PreshootDelayMod;
                currentAction.Enqueue(validStates.SHOOTING);
                break;
            case validStates.SHOOTING:
                //Make sure there is a hex to chose from (otherwise, go back to POSTSHOOT stage)
                if (spellsToShoot.Length != 0)
                {
                    Hex h = pickHex();
                    if (CanShoot(h, gameObject))
                    {
                        if (this.Aura != null) {
                            GameObject auraBall = Instantiate(this.Aura, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity) as GameObject;
                            SetActiveRecursively(auraBall, true);
                            Aura aura = auraBall.gameObject.GetComponentInChildren<Aura>();
                            Destroy(auraBall.GetComponentInChildren<CastagonPoint>().gameObject);
                            aura.InitializeAura(this.gameObject);
                            this.Aura = null;
                        }
                        else if(this.UltimateCounter >= this.UltimateChargeTrigger) {
                            this.CastUltimate(this.CurrentTarget().gameObject, Ultimates[UnityEngine.Random.Range(0,Ultimates.Length)]);
                        }
                        else {
                            CastHex(h, gameObject.transform.GetChild(0).gameObject, this.CurrentTarget().GetComponent<Targetable>().gameObject, 4, 5);
                            
                        }
                        currentAction.Enqueue(validStates.POSTSHOOT);
                    }
                    else
                    {
                        currentAction.Enqueue(validStates.IDLE);
                    }
                }
                else
                {
                    Debug.LogWarning("No spells to choose from, defaulting back to POSTSHOOT");
                    currentAction.Enqueue(validStates.POSTSHOOT);
                }
                break;
            case validStates.POSTSHOOT:
                //For now head straight into IDLE
                currentAction.Enqueue(validStates.IDLE);
                break;
            case validStates.DEAD:
                influenceText.GetComponent<Text>().text = "Dead";
                //Kill this script, so that it doesn't keep running this loop
                enabled = false;
                break;
            default:
                break;
        }
        return null;
    }

    //Called every time, but only if a state change did not occur.
    private object currentExclusive(validStates state)
    {
        return null;
    }

    //Pick the best hex for the situation
    //TODO: This
    private Hex pickHex()
    {
        System.Random rnd = new System.Random();
        return spellsToShoot[rnd.Next(0, spellsToShoot.Length)];
    }

    //Called every time, even if a state change occurred
    private object currentInclusive(validStates state)
    {
        if (state == validStates.DANGER) {
            //Move out of danger
            ArrayList dangerousSpells = isInDanger();
            if (dangerousSpells.Count > 0) {
                //Try to move out of the way
                Vector3 direction = new Vector3(speed * Vector3.Cross(((GameObject)dangerousSpells[0]).transform.position, gameObject.transform.position).normalized.x, UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
                gameObject.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
                currentAction.Clear();
                currentAction.Enqueue(validStates.DANGER);
            }
            else {
                currentAction.Enqueue(validStates.IDLE);
            }
        }
        else if (state == validStates.STARTUP) {
            // TODO: Only move into IDLE if the player has a wand
            currentAction.Enqueue(validStates.IDLE);
        }
        //else if (state == validStates.STUNNED && GetComponent<ControlEntity>().influenceDict[influences.STUN].GetStatus() == false) {
        else if (state == validStates.STUNNED) {
         currentAction.Enqueue(validStates.IDLE);
        }
        return null;
    }
    
    private validStates getCurrentState()
    {
        return currentAction.Peek();
    }
    
    public override void Awake()
    {
        base.Awake();
        Difficulty = PlayerPrefs.GetInt("difficulty", 2);
        
        currentAction = new Queue<validStates>();
        defaultSpeed = speed;
        originalPosition = gameObject.transform.position;
        // Make sure 1 <= difficulty >= 3
        if (Difficulty > 3) Difficulty = 3;
        if (Difficulty < 1) Difficulty = 1;
        // Modify difficulty variables
        if (Difficulty == 1)
        {
            GetComponent<CastListener>().ModifyMaxAngle(MaxAngleMod);
            IdlePauseTime += IdleMod;
            PreshootDelay += PreshootDelayMod;
            speed -= SpeedMod;
        }
        else if (Difficulty == 3)
        {
           // GetComponent<CastListener>().ModifyMaxAngle(-1 * MaxAngleMod);
            IdlePauseTime -= IdleMod;
            PreshootDelay -= PreshootDelayMod;
            speed += SpeedMod;
        }
        //Start out the queue with startup
        currentAction.Enqueue(validStates.STARTUP);
    }

    public override bool CanShoot(Hex h, GameObject launchPoint)
    {
        //Make sure the AI isn't disarmed
        return (GetComponent<ControlEntity>().influenceDict[influences.DISARM].GetStatus() == false);
    }

    //What to do when a hex hits the AI (this is generic for any hex. Other effects, such as disarm, are handled by the hex itself)
    public override void processHex(Hex h)
    {
        h.aiCollide(gameObject);
        ApplyDamage(h.Damage);
        h.Destroy();
        if (IsDead())
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(1616.67F,-16.815F,-31.932F); // Messy and hardcoded due to crunch time, clean this up later.
            influenceText.GetComponent<Text>().text = "Dead";
            Destroy(gameObject);
        }
    }

    private ArrayList isInDanger()
    {
        // Get all spells from mutual enemies
        ArrayList dangerousSpells = new ArrayList();
        foreach (GameObject target in Targets)
        {
            if (MutualTargets(target))
            {
                HashSet<Hex> spells = target.GetComponent<ControlEntity>().ActiveHexes;
                foreach (Hex h in spells)
                {
                    if (h == null) continue;
                    GameObject spell = h.gameObject;
                    //for some reason the spells array consistently had hexes with no rigibodies in it 
                    if (spell.gameObject.GetComponent<Rigidbody>() != null && Physics.Raycast(spell.transform.position, spell.gameObject.GetComponent<Rigidbody>().velocity.normalized, 50F, 1 << 8))
                    {
                        dangerousSpells.Add(spell);
                    }
                }
            }
        }


        return dangerousSpells;
    }

    public void setSpeed(float newSpeed)
    {
        if (newSpeed >= 0)
        {
            speed = newSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }
    }
    public static void SetActiveRecursively(GameObject rootObject, bool active) {
        rootObject.SetActive(active);

        foreach (Transform childTransform in rootObject.transform) {
            SetActiveRecursively(childTransform.gameObject, active);
        }
    }
}
