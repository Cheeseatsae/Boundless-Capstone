using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnitTest
    {
        [UnityTest]
        public IEnumerator AITargetTestEnumerator()
        {
            SceneManager.LoadScene("RussellUnitTest");

            
            
            yield return new WaitForSeconds(1f);
            //setup ai
            GameObject ai = new GameObject();

            ai.AddComponent<AirAiModel>();
            yield return new WaitForSeconds(1f);

            //setup target
            
            GameObject target = new GameObject();
            target.AddComponent<Health>();
            target.GetComponent<Health>().health = 10;

            ai.GetComponent<AirAiModel>().target = target;

            //bullet setup
            yield return new WaitForSeconds(1f);
            GameObject aiBullet = new GameObject();
            aiBullet.AddComponent<AIBullet>();
            ai.GetComponent<AirAiModel>().projectilePref = aiBullet;
            ai.GetComponent<AirAiModel>().bulletPos1 = ai.transform;
            ai.GetComponent<AirAiModel>().bulletPos2 = ai.transform;
            ai.GetComponent<AirAiModel>().minTargetRange = 30f;
            ai.GetComponent<AirAiModel>().projSpeed = 50f;

            aiBullet.GetComponent<AIBullet>().damage = 10;
            ai.GetComponent<AirAiModel>().targetDirection = (target.transform.position - ai.transform.position).normalized;

            ai.GetComponent<AirAiModel>().FlakAttack();

        }


    }


}
