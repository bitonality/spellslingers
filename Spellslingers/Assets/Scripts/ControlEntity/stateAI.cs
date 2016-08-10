using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StateAI : ControlEntity
{
    //Difficulty - Easy/Normal/Hard: 1/2/3
    // TODO: Make a way to set this in-game
    public int Difficulty = 2;

    //time until next state change 
    private float timeUntilChange = 0;

    //Time until the AI is allowed to shoot after being idle
    public float IdlePauseTime;
    // A difficulty modifier is the amount it will increase/decrease (depending on the context) if the difficulty is not set to 2.
    // For example, IdleDifficultyMod increases/decreases IdlePauseTime if difficulty is 1/3, respectivly. However, SpeedDifficultyMod decreases/increases speed if the difficulty is 1/3.
    public float IdleMod = 1;

    //For stacking of effects that disable shooting
    public float ShootingCycleDisabled = 0;

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
        //Debug.Log (col);
        if (col.gameObject.tag == "AIBoundry")
        {
            Debug.Log("Returning to center");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce((originalPosition - gameObject.transform.position) * speed, ForceMode.Impulse);
        }
    }

    public Queue<validStates> currentAction = new Queue<validStates>();

    //Called every 0.02 seconds
    void FixedUpdate()
    {        
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
        //Debug.Log ("Player health: " + this.Enemy.GetComponent<ControlEntity> ().Health);
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
                //Wait for 1 second
                timeUntilChange = Time.time + PreshootDelayMod;
                currentAction.Enqueue(validStates.SHOOTING);
                break;
            case validStates.SHOOTING:
                //Make sure there is a hex to chose from (otherwise, go back to POSTSHOOT stage)
                if (spellsToShoot.Length != 0)
                {
                    //Pick a hex
                    Hex h = pickHex();
                    if (CanShoot(h, gameObject))
                    {
                        //Shoot
                        CastHex(h, gameObject.transform.GetChild(0).gameObject.transform, this.CurrentTarget().GetComponent<Targetable>().TargetPoint, 2, 5);
                        //Go back to POSTSHOOT state
                        currentAction.Enqueue(validStates.POSTSHOOT);
                    }
                    else
                    {
                        //Return to PRESHOOT state
                        currentAction.Enqueue(validStates.PRESHOOT);
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
                //Kill this script, so that it doesn't keep running this loop
                enabled = false;
                break;
            default:
                break;
        }
        return null;
    }

    //Called every time, but only if a state change did not occur.
    private object currentExclusive(validStates state) { 
        return null;
    }

    //Pick the best hex for the situation
    //TODO: This
    private Hex pickHex()
    {
        System.Random rnd = new System.Random();
        return spellsToShoot[rnd.Next(0, spellsToShoot.Length)];
    }

    //Called regardless if a state change occurred, every time. 
    private object currentInclusive(validStates state)
    {
        if (state == validStates.DANGER)
        {
            //Move out of danger
            ArrayList dangerousSpells = isInDanger();
            if (dangerousSpells.Count > 0)
            {
                //Choose the one that is most important
                //TODO: Sort spells
                //string[] priorities = new string[] {"Damage", "Disarm", "Stun"};
                //Move 
                Vector3 direction = new Vector3(speed * (float)Vector3.Cross(((GameObject)dangerousSpells[0]).transform.position, gameObject.transform.position).normalized.x, UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
                gameObject.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
                currentAction.Clear();
                currentAction.Enqueue(validStates.DANGER);
            }
            else
            {
                // Stop movement
                //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                // Push idle state
                Debug.Log("Moving to idle state from danger state");
                currentAction.Enqueue(validStates.IDLE);
            }
        }
        else if (state == validStates.STARTUP)
        {
            // TODO: Only move into IDLE if the player has a wand
            currentAction.Enqueue(validStates.IDLE);
        }
        return null;
    }

    //Returns the object's current state
    private validStates getCurrentState()
    {
        return (validStates)currentAction.Peek();
    }

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        defaultSpeed = speed;
        originalPosition = gameObject.transform.position;
        //Make sure 1 <= difficulty >= 3
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
            GetComponent<CastListener>().ModifyMaxAngle(-1 * MaxAngleMod);
            IdlePauseTime -= IdleMod;
            PreshootDelay -= PreshootDelayMod;
            speed += SpeedMod;
        }
        //Start out the queue with startup
        currentAction.Enqueue(validStates.STARTUP);
    }

    public override bool CanShoot(Hex h, GameObject launchPoint)
    {
        //Check if the current time is greater than when the shooting cycle is disabled to and make sure it is not (hence the !) is under the influence of DISARM
        return (Time.time >= ShootingCycleDisabled); //&& !currentInfluences[influences.DISARM]);
    }

    public override void processHex(Hex h)
    {
        h.aiCollide(gameObject);
        ApplyDamage(h.Damage);
        h.Destroy();
        if (IsDead())
        {
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
}
