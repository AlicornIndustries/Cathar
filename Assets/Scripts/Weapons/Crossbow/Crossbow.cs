using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Weapon {

    public float refireDelay;
    public float muzzleVelocity;
    public Transform barrel;

    private float timeLastFired;

    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        timeLastFired = 0f;
    }

    public override void Fire(Vector3 playerVelocity)
    {
        if(CanFire())
        {
            timeLastFired = Time.time;
            GameObject ironBolt = objectPooler.SpawnFromPool("Iron Bolt", barrel.position, barrel.rotation);
            ironBolt.GetComponent<IronBolt>().OnFire(barrel, muzzleVelocity, playerVelocity);
        }
    }

    private bool CanFire()
    {
        if(Time.time > refireDelay + timeLastFired)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
