using System.Collections;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class HarryUnitTestScript
    {

        [UnityTest]
        public IEnumerator PlayerTestEnumerator()
        {
            // scene load
            SceneManager.LoadScene("HarryUnitTest");
            
            yield return new WaitForSecondsRealtime(1);
            
            // test object setup
            GameObject testController = new GameObject();
            
            Ability1 a = testController.AddComponent<Ability1>();
            
            yield return new WaitForSecondsRealtime(1);
            
            // quick check
            Assert.NotNull(testController);

            // player obj setup
            GameObject p = new GameObject();
            p.AddComponent<Rigidbody>();
            p.AddComponent<NetworkIdentity>();
            p.AddComponent<PlayerController>();
            p.AddComponent<PlayerModel>();
            p.GetComponent<PlayerModel>().viewObject = p;
            p.GetComponent<PlayerModel>().attackSpeed = 2;
            p.GetComponent<PlayerModel>().attackDamage = 2;
            
            yield return new WaitForSecondsRealtime(0.4f);
            
            // bullet prefab setup
            GameObject b = new GameObject();
            b.AddComponent<Rigidbody>();
            b.AddComponent<Projectile>();
            b.AddComponent<Damager>();

            yield return new WaitForSecondsRealtime(0.4f);
            
            // ability setup
            a.baseCooldown = 1;
            a.bulletPref = b;
            a.player = p.GetComponent<PlayerModel>();
            
            yield return new WaitForSecondsRealtime(0.4f);

            // test fire
            a.CmdFire(10, Vector3.zero);

            yield return new WaitForSecondsRealtime(0.4f);
            
            // finding bullets
            Projectile[] bullet = GameObject.FindObjectsOfType<Projectile>();

            Assert.Greater(bullet.Length, 1);
        }

        
    }
}
