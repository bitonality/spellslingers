﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileBarrage : Ultimate {

    public GameObject HexTemplate;
    public int NumberOfHexes;
    private List<MissileBarrageProjectile> Hexes = new List<MissileBarrageProjectile>();

    public override void Cast(GameObject source, GameObject target) {
        base.Cast(source, target);



        Vector3 center = source.transform.position;
        for (int i = 0; i < NumberOfHexes; i++) {
            Vector3 pos = RandomCircle(center, 5.0f);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
            GameObject hex = Instantiate(HexTemplate, pos, rot) as GameObject;
            hex.GetComponent<Rigidbody>().AddForce(Vector3.up);
            hex.GetComponent<Hex>().Source = source.GetComponent<ControlEntity>();
            hex.GetComponent<Hex>().MaxRotation = 179;
        }

        Invoke("EnableHoming", 2F);
    }

    private class MissileBarrageProjectile {
        public GameObject Hex;
        public GameObject Source;
        public GameObject Target;
    }

    private void EnableHoming() {
        foreach(MissileBarrageProjectile proj in Hexes) {
            proj.Hex.GetComponent<HomingProjectile>().LaunchProjectile(proj.Hex.GetComponent<Hex>(), proj.Source.transform, proj.Target.transform, 10F, 4F);
        }
    }



    Vector3 RandomCircle(Vector3 center, float radius) {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

}
