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
    
    [Command]
    void CmdFire(float lifeTime, Vector3 target)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.Euler(90,90,0));
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        
        Destroy(bullet, lifeTime);
        
        NetworkServer.Spawn(bullet);
        
        bullet.GetComponent<Projectile>().RpcSetVelocity(bulletRb.velocity);
        //Debug.Log("test");
    }

    [Command]
    public void CmdColourChange()
    {
        Debug.Log("Changin Colour");
        RpcColourChange();
        Material mat = bulletPref.GetComponent<Renderer>().material;
        mat.color = Color.red;
        bulletPref.GetComponent<Renderer>().material = mat;
        bulletPref.transform.localScale *= 2;
    }

    [ClientRpc]
    private void RpcColourChange()
    {
        Debug.Log("Fuckertyy");
        Material mat = bulletPref.GetComponent<Renderer>().material;
        mat.color = Color.red;
        bulletPref.AddComponent<BoxCollider>();
        bulletPref.GetComponent<Renderer>().material = mat;
    }
    
    public override void Enter()
    {
        if(!isLocalPlayer) return; 
        
        // clients now accurately shoot but there's still a delay
        CmdFire(player.attackRange, player.target);
        
        //https://vis2k.github.io/Mirror/Concepts/GameObjects/SpawnObject
    }
}
