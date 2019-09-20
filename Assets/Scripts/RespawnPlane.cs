using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerModel p = other.gameObject.GetComponent<PlayerModel>();
        Health h = other.gameObject.GetComponent<Health>();

        if (p == null)
        {
            if (h == null) Destroy(other.gameObject);
            else
            {
                int dmg = h.health * 2;
                h.DoDamage(dmg);
            }
        }
        else
        {
            int dmg = (int)(h.health * 0.2f);
            h.DoDamage(dmg);
            p.MovePlayerToSolidGround();
        }

    }
    
}
