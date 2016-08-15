using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UltimateHandler : MonoBehaviour {
    [HideInInspector]
    public UltimatePattern UserPattern = new UltimatePattern();
    [HideInInspector]
    public Player UltimatePlayer;
    public UltimatePattern[] PatternTemplates;
    public List<UltimateZoneChange> ControllerChange = new List<UltimateZoneChange>();
    public Dictionary<GameObject, Queue<UltimateZone>> ControllerMap = new Dictionary<GameObject, Queue<UltimateZone>>();

    public void ZoneEntered(GameObject zone, GameObject controller) {
        if(controller.GetComponentInChildren<ParticleSystem>() == null) {
            ControllerMap.Clear();
            ControllerChange.Clear();
            return;
        }

        Queue<UltimateZone> controllerData;
        if (ControllerMap.TryGetValue(controller, out controllerData)) {
            controllerData.Enqueue(zone.GetComponent<UltimateZone>());
        }
        else {
            controllerData = new Queue<UltimateZone>();
            controllerData.Enqueue(zone.GetComponent<UltimateZone>());
            ControllerMap.Add(controller, controllerData);
        }

        foreach (KeyValuePair<GameObject, Queue<UltimateZone>> controllerEntry in ControllerMap) {
            // If we have more than 1 zone enqueued.
            if (controllerEntry.Value.Count > 1) {
                UltimateZone from = controllerEntry.Value.Dequeue();
                UltimateZone to = controllerEntry.Value.Dequeue();
                ZoneChange(from, to, controllerEntry.Key);
                controllerEntry.Value.Enqueue(to);
            }
        }

    }

    public void ZoneChange(UltimateZone from, UltimateZone to, GameObject controller) {
        Debug.Log("Zone change: " + from + " - " + to + " - " + controller);
        UltimateZoneChange change = new UltimateZoneChange(from.ZoneID, to.ZoneID, Time.time + 0.5F, controller);
        ControllerChange.Add(change);

     

        for(int i = ControllerChange.Count - 1; i >= 0; i--) {
            // If the point is expired, then remove it from the list and continue.
            if (Time.time > ControllerChange[i].InvalidTime) {
                ControllerChange.RemoveAt(i);
                continue;
            }

            if((i-1) > -1 && ControllerChange[i].Movement == ControllerChange[i-1].Movement && ControllerChange[i].Controller != ControllerChange[i-1].Controller) {
                ZoneMovement m = ControllerChange[i].Movement;
                if(m == ZoneMovement.LEFT && UserPattern.First == ZoneMovement.UNDEFINED) {
                    UserPattern.First = ZoneMovement.BOTH_LEFT;
                }
                else if (m == ZoneMovement.RIGHT && UserPattern.First == ZoneMovement.UNDEFINED) {
                    UserPattern.First = ZoneMovement.BOTH_RIGHT;
                }
                else if (m == ZoneMovement.UP && UserPattern.Second == ZoneMovement.UNDEFINED) {
                    UserPattern.Second = ZoneMovement.BOTH_UP;
                }
                else if (m == ZoneMovement.DOWN && UserPattern.Second == ZoneMovement.UNDEFINED) {
                    UserPattern.Second = ZoneMovement.BOTH_DOWN;
                }
                ControllerChange.RemoveAt(i);
                ControllerChange.RemoveAt(i - 1);
                // Decrement i by one since we remove an index.
                i--;
            }

            foreach(UltimatePattern pattern in PatternTemplates) {
                if(pattern.First == UserPattern.First && pattern.Second == UserPattern.Second) {
                    Debug.Log("Casting: " + pattern.Name);
                    UltimatePlayer.CastUltimate(UltimatePlayer.CurrentTarget().gameObject, pattern.Ultimate);
                    // Have not proven the need of Immediate, forethought.
                    Destroy(this.gameObject);
                }
            }
        }



        

    }

    [System.Serializable]
    public class UltimatePattern {
        public string Name;
        public ZoneMovement First;
        public ZoneMovement Second;
        public GameObject Ultimate;
        
        public void Clear() {
            this.First = ZoneMovement.UNDEFINED;
            this.Second = ZoneMovement.UNDEFINED;
        }
    }

    public class UltimateZoneChange {
        public int From;
        public int To;
        // A time in the future when the zone change should be discarded from the master queue.
        public float InvalidTime;
        public GameObject Controller;
        public ZoneMovement Movement;

        public UltimateZoneChange(int from, int to, float invalidTime, GameObject controller) {
            this.From = from;
            this.To = to;
            this.InvalidTime = invalidTime;
            this.Controller = controller;

            if(from - to == -1) {
                this.Movement = ZoneMovement.LEFT;
            } else if(from - to == 1) {
                this.Movement = ZoneMovement.RIGHT;
            } else if(from - to == 2) {
                this.Movement = ZoneMovement.UP;
            } else if(from - to == -2) {
                this.Movement = ZoneMovement.DOWN;
            }
        }
    }

    
    public enum ZoneMovement {
        UNDEFINED,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        BOTH_LEFT,
        BOTH_RIGHT,
        BOTH_UP,
        BOTH_DOWN,
    }
}
