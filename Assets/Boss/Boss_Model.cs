using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
        //currentAbility = lavaFountain;
        StartCoroutine(TestCast());
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
        if (LevelManager.instance.player != null)
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
                currentAbility.Cast();
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

}
