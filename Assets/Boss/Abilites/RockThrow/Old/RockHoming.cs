using System;
using System.Collections;
using UnityEngine;

public class RockHoming : MonoBehaviour
{

    public GameObject player;
    private float speed = 0.3f;
    private Rigidbody body;

    public bool stopForce = true;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        player = LevelManager.instance.player;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        stopForce = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
//        if (stopForce) return;
//        
//        Vector3 dir = player.transform.position - transform.position;
//        dir.y = 0;
//        float mult = Vector3.Distance(transform.position, player.transform.position);
//        
//        body.velocity += dir.normalized * speed * mult * Time.fixedDeltaTime;

    }

    private void OnCollisionEnter(Collision other)
    {
        stopForce = true;
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
   
        if (dir > 0.0) {
            return 1.0f;
        } 
        else if (dir < 0.0) 
        {
            return -1.0f;
        } 
        else 
        {
            return 0.0f;
        }
    }
}
