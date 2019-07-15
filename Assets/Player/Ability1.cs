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
    
    // bullet should be spawned client side then sent to server. server bullet that is sent to other clients should only be visual
    // client side should manage damage and everything else
    // server side doesn't need to be 100% positional accurate 
    

    // Start is called before the first frame update
    private void Start()
    {
        rangeInMetres = player.attackRange * projectileSpeed;
    }

    [Command]
    void CmdFire(float lifeTime, Vector3 target)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.Euler(90,90,0));
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        
        Destroy(bullet, lifeTime);
        
        NetworkServer.Spawn(bullet);
        //Debug.Log("test");
    }

    public override void Enter()
    {
        if(!isLocalPlayer) return; 
        
        // clients now accurately shoot but there's still a delay
        CmdFire(player.attackRange, player.target);
        
        //https://vis2k.github.io/Mirror/Concepts/GameObjects/SpawnObject
    }
}
