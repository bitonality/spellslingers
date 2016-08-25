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

    [HideInInspector]
    private float RoamRadius = 1;
    [HideInInspector]
    private Vector3 StartPosition;

    private float defaultSpeed;

    //List of spells the AI is allowed to shoot
    public Hex[] spellsToShoot;

    private Vector3 originalPosition;


    public AudioClip[] AggroSounds;
    public AudioClip[] PainSounds;
   


    public enum ValidStates
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

    public Queue<ValidStates> CurrentAction;

    //Called every 0.02 seconds
    public override void FixedUpdate()
    {

        base.FixedUpdate();
        // If the AI is in danger, clear the state queue and enqueue the danger state.
        if(!CurrentAction.Contains(ValidStates.DANGER) && isInDanger().Count > 0) {
            CurrentAction.Clear();
            CurrentAction.Enqueue(ValidStates.DANGER);
            RandomMovementX = UnityEngine.Random.Range(-1.0F, 1.0F);
            RandomMovementZ = UnityEngine.Random.Range(-1.0F, 1.0F);
            // If the state queue contains danger and there are no incoming spells, enqueue the idle state.
        } else if(CurrentAction.Contains(ValidStates.DANGER) && !CurrentAction.Contains(ValidStates.PRESHOOT) && isInDanger().Count == 0 ) {
            CurrentAction.Enqueue(ValidStates.PRESHOOT);
        }
        
        if (CurrentAction.Count <= 0)
        {
            //Something went wrong with starting the AI
            Debug.LogWarning("currentAction size is 0, meaning awake() was not called before fixedUpdate()");
            return;
        }
        if (CurrentAction.Count != 1 && timeUntilChange <= Time.time)
        {
            //Pop leading item off the queue and call justLeft with it
            justLeft(CurrentAction.Dequeue(), getCurrentState());
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
    private void justLeft(ValidStates oldState, ValidStates newState)
    {
        Debug.Log("Changing state from  " + oldState + " to " + newState + " at time " + Time.time);
        switch (newState)
        {
            case ValidStates.IDLE:
                // Schedule interruptable state change in {{ IdlePauseTime }} seconds
                timeUntilChange = Time.time + IdlePauseTime;
                CurrentAction.Enqueue(ValidStates.PRESHOOT);
                break;
            case ValidStates.PRESHOOT:
                //Stop movement
                RandomMovementX = 0;
                RandomMovementZ = 0;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                // Wait {{ PreshootDelay }} seconds. If the AI doesn't get in danger, move this will move into SHOOTING stage.
                timeUntilChange = Time.time + PreshootDelayMod;
                CurrentAction.Enqueue(ValidStates.SHOOTING);
                break;
            case ValidStates.SHOOTING:
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
                            this.gameObject.GetComponent<Animation>().Play("Sky_View");
                        }
                        else if(this.UltimateCounter >= this.UltimateChargeTrigger) {
                            this.CastUltimate(this.CurrentTarget().gameObject, Ultimates[UnityEngine.Random.Range(0,Ultimates.Length)]);
                            this.gameObject.GetComponent<Animation>().Play("Sky_magic");
                        }
                        else {
                            CastHex(h, gameObject.transform.FindChild("Bip01").FindChild("LaunchPoint").gameObject, this.CurrentTarget().GetComponent<Targetable>().gameObject, 4, 5);
                            this.gameObject.GetComponent<Animation>().Play("Sky_Attack1");
                            this.GetComponentInChildren<AudioSource>().clip = PainSounds[UnityEngine.Random.Range(0, AggroSounds.Length)];
                            this.GetComponentInChildren<AudioSource>().Play();

                        }
                        CurrentAction.Enqueue(ValidStates.POSTSHOOT);
                    }
                    else
                    {
                        CurrentAction.Enqueue(ValidStates.IDLE);
                    }
                }
                else
                {
                    Debug.LogWarning("No spells to choose from, defaulting back to POSTSHOOT");
                    CurrentAction.Enqueue(ValidStates.POSTSHOOT);
                }
                break;
            case ValidStates.POSTSHOOT:
                //For now head straight into IDLE
                CurrentAction.Enqueue(ValidStates.IDLE);
                break;
            case ValidStates.DEAD:
                influenceText.GetComponent<Text>().text = "Dead";
                //Kill this script, so that it doesn't keep running this loop
                enabled = false;
                break;
            default:
                break;
        }
        this.gameObject.GetComponent<Animation>().CrossFade("Levitate_sky");

    }

    //Called every time, but only if a state change did not occur.
    private void currentExclusive(ValidStates state) {
       
    }

    //Pick the best hex for the situation
    //TODO: This
    private Hex pickHex()
    {
        System.Random rnd = new System.Random();
        return spellsToShoot[rnd.Next(0, spellsToShoot.Length)];
    }

    //Called every time, even if a state change occurred
    private object currentInclusive(ValidStates state)
    {
        // This doesn't pop anything off the queue, so we don't enqueue anything in here.
        if (state == ValidStates.DANGER) {
                //Move out of danger
                ArrayList dangerousSpells = isInDanger();
            if (dangerousSpells.Count > 0) {
                //Try to move out of the way
                float xValue = RandomMovementX;
                if (this.originalPosition.x - this.gameObject.transform.position.x < -2) {
                    this.gameObject.GetComponent<Animation>().CrossFade("Levitate_R");
                    if(RandomMovementX > 0) {
                        xValue = -RandomMovementX;
                    } else {
                        xValue = RandomMovementX;
                    }
                }  if (this.originalPosition.x - this.gameObject.transform.position.x > 2) {
               
                    this.gameObject.GetComponent<Animation>().CrossFade("Levitate_L");
                    if (RandomMovementX < 0) {
                        xValue = -RandomMovementX;
                    }
                    else {
                        xValue = RandomMovementX;
                    }
                }

                float zValue = 0;
               
                Vector3 direction = new Vector3(xValue, 0, zValue).normalized;
                gameObject.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Acceleration);
               
            }
            
        }
        else if (state == ValidStates.STARTUP) {
            // TODO: Only move into IDLE if the player has a wand
            CurrentAction.Enqueue(ValidStates.IDLE);
        }
     
        return null;
    }
    
    private ValidStates getCurrentState()
    {
        return CurrentAction.Peek();
    }
    
    public override void Awake()
    {
        base.Awake();
        Difficulty = PlayerPrefs.GetInt("difficulty", 3);
        
        CurrentAction = new Queue<ValidStates>();
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
        CurrentAction.Enqueue(ValidStates.STARTUP);
        this.gameObject.GetComponent<Animation>().wrapMode = WrapMode.Loop;
        this.gameObject.GetComponent<Animation>()["Sky_magic"].wrapMode = WrapMode.Once;
        this.gameObject.GetComponent<Animation>()["Sky_Attack1"].wrapMode = WrapMode.Once;
        this.gameObject.GetComponent<Animation>()["Sky_View"].wrapMode = WrapMode.Once;
        this.gameObject.GetComponent<Animation>()["Sky_magic"].layer = 2;
        this.gameObject.GetComponent<Animation>()["Sky_Attack1"].layer = 2;
        this.gameObject.GetComponent<Animation>()["Sky_View"].layer = 2;
        this.gameObject.GetComponent<Animation>()["Levitate_R"].normalizedSpeed = 0.5F;
        this.gameObject.GetComponent<Animation>()["Levitate_L"].normalizedSpeed = 0.5F;
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
        this.GetComponentInChildren<AudioSource>().clip = PainSounds[UnityEngine.Random.Range(0, PainSounds.Length)];
        this.GetComponentInChildren<AudioSource>().Play();
        h.aiCollide(gameObject);
        ApplyDamage(h.Damage);
        h.Destroy();

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
