using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UltimateHandler : MonoBehaviour {

    public Dictionary<GameObject, UltimateControllerData> ControllerMap;
    public List<UltimateZoneChange> LoggedZones;
    public List<UltimateInspectorEntry> UltimatePatterns;


    public void ZoneEntered(GameObject zone, GameObject controller) {
       UltimateControllerData controllerData;
        if(ControllerMap.TryGetValue(zone, out controllerData)) {
            controllerData.TouchedZones.Enqueue(zone.GetComponent<UltimateZone>());
        } else {
            controllerData = new UltimateControllerData();
            controllerData.TouchedZones.Enqueue(zone.GetComponent<UltimateZone>());
            ControllerMap.Add(controller, controllerData);
        }

        foreach (KeyValuePair<GameObject, UltimateControllerData> controllerEntry in ControllerMap) {
            // If we have more than 1 zone enqueued.
            if(controllerEntry.Value.TouchedZones.Count > 1) {

                ZoneChange(controllerEntry.Value.TouchedZones.Dequeue(), controllerEntry.Value.TouchedZones.Dequeue(), controllerEntry.Key);
            }
        }


    }

    public void ZoneChange(UltimateZone from, UltimateZone to, GameObject controller) {
            LoggedZones.Add(new UltimateZoneChange(from.ZoneID, to.ZoneID));
            
           
    }

    [System.Serializable]
    public class UltimateInspectorEntry {
        public UltimateZoneChange[] Changes;
        public GameObject Ultimate;
    }


    public class UltimateControllerData {
        public Queue<UltimateZone> TouchedZones;
        public UltimateControllerData() {
            TouchedZones = new Queue<UltimateZone>();
        }

    }

    public struct UltimateZoneChange {
        public int from;
        public int to;

        public UltimateZoneChange(int from, int to) {
            this.from = from;
            this.to = to;
        }
    }


}
