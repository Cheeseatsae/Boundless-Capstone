using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [ClientRpc]
    public void RpcSetVelocity(Vector3 v)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        if (r != null) r.velocity = v;
        else Destroy(this.gameObject);
    }
}
