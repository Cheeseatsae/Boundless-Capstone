using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void PlayerDamageInstance(GameObject obj, int dmg, Vector3 loc);
    public static event PlayerDamageInstance OnPlayerDamageInstance;

    public static void CallPlayerDamageEvent(GameObject obj, int dmg, Vector3 loc)
    {
        OnPlayerDamageInstance?.Invoke(obj, dmg, loc);
    }
}
