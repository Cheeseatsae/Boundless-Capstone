using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Ability1 : AbilityBase
{
    public PlayerModel player;
    public GameObject bulletPref;
    [HideInInspector] public float rangeInMetres;
    
    
    public float projectileSpeed;
    // Start is called before the first frame update
    private void Start()
    {
        rangeInMetres = player.attackRange * projectileSpeed;
    }

    [Command]
    void CmdFire(float lifeTime)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.Euler(90,90,0));
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * projectileSpeed;

        Transform t = bullet.transform;
        t.LookAt(player.target);
        
        Destroy(bullet, lifeTime);
        
        NetworkServer.Spawn(bullet);
        Debug.Log("test");
    }

    public override void Enter()
    {
        if(!isLocalPlayer)
            return; 
        
        CmdFire(player.attackRange);
    }
}
