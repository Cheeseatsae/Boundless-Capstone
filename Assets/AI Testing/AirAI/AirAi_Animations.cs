using UnityEngine;

public class AirAi_Animations : MonoBehaviour
{

    private AirAiModel model;
    private Rigidbody body;
    
    private float turnSpeed = 4;
    float zRotation = 0;

    private void Awake()
    {
        model = GetComponentInParent<AirAiModel>();
        body = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localVel =  transform.InverseTransformDirection(body.velocity);

        if (localVel.z > 0) zRotation = Mathf.Lerp(zRotation, -8.5f * model.dodgeDirection.magnitude, Time.deltaTime);
        if (localVel.z < 0) zRotation = Mathf.Lerp(zRotation, 8.5f * model.dodgeDirection.magnitude, Time.deltaTime);
        
        RotateTowards(model.target.transform.position);
    }

    private void RotateTowards(Vector3 t)
    {
        Vector3 targetDir = t - transform.position;

        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        // Move our position a step closer to the target.
        Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
        
        //Vector3 newRot = transform.rotation.eulerAngles;
        Quaternion y = Quaternion.Euler(newRot.x,newRot.y,newRot.z + zRotation);
        
        transform.SetPositionAndRotation(transform.position, y);
    }
}
