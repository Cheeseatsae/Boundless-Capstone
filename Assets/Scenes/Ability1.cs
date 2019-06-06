using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Ability1 : AbilityBase
{

    public GameObject bulletPref;

    public float projectileSpeed;
    // Start is called before the first frame update
    [Command]
    void CmdFire(float lifeTime)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.right, Quaternion.identity);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.right * projectileSpeed;
        
        Destroy(bullet,lifeTime);
        
        NetworkServer.Spawn(bullet);
        Debug.Log("test");
    }

    public override void Enter()
    {
        
        if(!isLocalPlayer)
            return; 
        CmdFire(3.0f);
    }
}
