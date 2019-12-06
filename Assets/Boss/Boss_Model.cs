using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss_Model : MonoBehaviour
{



    public GameObject target;
    public float playerDist;
    
    //Abilities
    public Boss_Ability_Base currentAbility;
    public Animator anim;
    public Boss_Ability_Base[] abilities;
    public LavaFountain lavaFountain;
    public FireBreath fireBreath;
    public RockThrow rockThrow;
    public float flameDist;
    public float postCastTime;
    public bool abilityCheck = false;
    public bool canCast = false;

    public bool alive = true;

    public CharacterAudio audio;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnHealthChange += PlayerUI.instance.UpdateBossHealth;
        //currentAbility = lavaFountain;
        StartCoroutine(TestCast());
        audio = GetComponent<CharacterAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        Targeting();
        
        if (canCast && !abilityCheck)
        {

            AbilitySelect();
        }
    }
    
    //Testing
    public IEnumerator TestCast()
    {
        yield return new WaitForSeconds(10);
        canCast = true;
    }
    
    private void Targeting()
    {
        if (LevelManager.instance.player != null && alive )
        {
            target = LevelManager.instance.player;
            playerDist = Vector3.Distance(gameObject.transform.position, target.transform.position);
            Vector3 dir = target.transform.position - gameObject.transform.position;
            
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void AbilitySelect()
    {
        abilityCheck = true;
        if (playerDist <= flameDist)
        {
            //fireBreath.Cast();
            anim.SetTrigger("FireBreathStart");
            Debug.Log("im casting fire breath");
        }
        else
        {
            int abilitySelection = Random.Range(0, abilities.Length);
            currentAbility = abilities[abilitySelection];
            if (!currentAbility.onCd)
            {
                if (currentAbility == rockThrow)
                {
                    anim.SetTrigger("Throw");
                }else if (currentAbility == lavaFountain)
                {
                    anim.SetTrigger("Vent");
                }
                Debug.Log("im casting" + currentAbility);
                
            } else AbilitySelect();
        }

        
        canCast = false;

    }

    public IEnumerator CastDelay()
    {
        
        yield return new WaitForSeconds(postCastTime);
        canCast = true;
        abilityCheck = false;
    }

    private void OnDestroy()
    {
        
        GetComponent<Health>().OnHealthChange -= PlayerUI.instance.UpdateBossHealth;
    }
}
