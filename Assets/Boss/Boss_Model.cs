using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Model : MonoBehaviour
{



    public GameObject target;
    
    //Abilities
    public Boss_Ability_Base currentAbility;
    
    public LavaFountain lavaFountain;

    public FireBreath fireBreath;

    public RockWall rockWall;
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
    }
    
    //Testing
    public IEnumerator TestCast()
    {
        yield return new WaitForSeconds(5);
        currentAbility.Cast();
    }
    
    private void Targeting()
    {
        if (LevelManager.instance.player != null)
        {
            target = LevelManager.instance.player;
        }
        

    }

}
