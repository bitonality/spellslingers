﻿using System.Collections;
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

    [HideInInspector]
    private float RoamRadius = 1;
    [HideInInspector]
    private Vector3 StartPosition;

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



    private float RandomMovementX = 0;
    private float RandomMovementZ = 0;

    public Queue<validStates> currentAction;

    //Called every 0.02 seconds
    public override void FixedUpdate()
    {

        base.FixedUpdate();
        if(!currentAction.Contains(validStates.DANGER) && isInDanger().Count > 0) {
            currentAction.Clear();
            currentAction.Enqueue(validStates.DANGER);
        }
        
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
       // Debug.Log("Changing state from  " + oldState + " to " + newState + " at time " + Time.time);
        switch (newState)
        {
            case validStates.HIT:
                currentAction.Enqueue(validStates.DANGER);
                break;
            case validStates.IDLE:
                //Move in a random direction
                    //Vector3 Destination = new Vector3(UnityEngine.Random.Range(-3f, 3f) * speed, 0, UnityEngine.Random.Range(-5f, 5f) * speed);
                    //gameObject.GetComponent<Rigidbody>().velocity = (Destination);
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
                            CastHex(h, gameObject.transform.FindChild("Bip01").FindChild("LaunchPoint").gameObject, this.CurrentTarget().GetComponent<Targetable>().gameObject, 4, 5);
                            this.gameObject.GetComponent<Animation>().CrossFade("Sky_Attack1");
                            
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
        this.gameObject.GetComponent<Animation>().CrossFadeQueued("Levitate_sky", 1F);
        return null;
    }

    //Called every time, but only if a state change did not occur.
    private object currentExclusive(validStates state)
    {
        if(state == validStates.IDLE) {
            currentAction.Enqueue(validStates.PRESHOOT);
        }
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
            if (RandomMovementX == 0) {
                RandomMovementX = UnityEngine.Random.Range(-1.0F, 1.0F);
            }

            if (RandomMovementZ == 0) {
                RandomMovementZ = UnityEngine.Random.Range(-1.0F, 1.0F);
            }
                //Move out of danger
                ArrayList dangerousSpells = isInDanger();
            if (dangerousSpells.Count > 0) {
                //Try to move out of the way
                float xValue = RandomMovementX;
                if (this.originalPosition.x - this.gameObject.transform.position.x < -2) {
                    // need to move right
                    xValue = -1;
                
                } else if (this.originalPosition.x - this.gameObject.transform.position.x > 2) {
                    // need to move left
                    xValue = 1;
                 
                }

                float zValue = RandomMovementZ;
                if (this.originalPosition.z - this.gameObject.transform.position.z < -2) {
                    zValue = -1;
                    // need to move backward
                }
                else if (this.originalPosition.z - this.gameObject.transform.position.z > 2) {
                    zValue = 1;
                    // need to move forward
                }

                Vector3 direction = new Vector3(xValue, 0, zValue).normalized;
                gameObject.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Acceleration);
                if(direction.x > 0 && direction.z < 0) {
                    // forward to the left
                    this.gameObject.GetComponent<Animation>().CrossFadeQueued("Levitate_L", 1F);
                    //this.gameObject.GetComponent<Animation>().CrossFade("Levitate_F", 1F);
                } else if (direction.x < 0 && direction.z < 0) {
                    // forward to the right
                    this.gameObject.GetComponent<Animation>().CrossFadeQueued("Levitate_R", 1F);
                    //this.gameObject.GetComponent<Animation>().CrossFade("Levitate_F", 1F);
                } else if (direction.z < 0 && direction.x > 0  ) {
                    // backwards to the left
                    this.gameObject.GetComponent<Animation>().CrossFadeQueued("Levitate_L", 1F);
                    //this.gameObject.GetComponent<Animation>().CrossFade("Levitate_B", 1F);
                } else if(direction.z > 0 && direction.x < 0) {
                    // backwards to the right
                    this.gameObject.GetComponent<Animation>().CrossFadeQueued("Levitate_R", 1F);
                    //this.gameObject.GetComponent<Animation>().CrossFade("Levitate_B", 1F);
                }
                currentAction.Clear();
                currentAction.Enqueue(validStates.DANGER);

            }
            else {
                currentAction.Enqueue(validStates.IDLE);
                this.RandomMovementX = 0;
                this.RandomMovementZ = 0;
            }
        }
        else if (state == validStates.STARTUP) {
            // TODO: Only move into IDLE if the player has a wand
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
        Difficulty = PlayerPrefs.GetInt("difficulty", 3);
        
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

    void Start() {
        this.StartPosition = this.gameObject.transform.position;
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
