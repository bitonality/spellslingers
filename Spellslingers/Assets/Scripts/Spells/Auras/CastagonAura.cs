using UnityEngine;
using System.Collections;

public class CastagonAura : MonoBehaviour {

    public GameObject AuraTemplate;

    public GameObject CreateAura(Vector3 position, ControlEntity ce) {
        GameObject aura = Instantiate(AuraTemplate, position, Quaternion.identity) as GameObject;
        aura.transform.SetParent(ce.gameObject.transform);
        return aura;
    }
}
