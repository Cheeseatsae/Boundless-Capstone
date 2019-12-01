using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerModel model;

    public bool IKActive = true;
    
    private float yRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        model = GetComponentInParent<PlayerModel>();
        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(180, 0,0));
    }

    void OnAnimatorIK()
    {
        Vector3 vel = model.body.velocity;
        vel.y = 0;

//        float myAngle = Mathf.Atan2 (Input.GetAxis ("Horizontal"),Input.GetAxis ("Vertical")) * Mathf.Rad2Deg;
//        float bodyRotation = myAngle + Camera.main.transform.eulerAngles.y;
//        transform.rotation = Quaternion.Euler(0, bodyRotation, 0);

        RotateTowards(transform.position + vel);
        
        if (!IKActive)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0); 
            animator.SetLookAtWeight(0,0,0,0,0);
            return;
        }
            
        animator.SetLookAtWeight(1, 1, 1, 0, 1);
        animator.SetLookAtPosition(model.target);
        
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);

        animator.SetIKPosition(AvatarIKGoal.RightHand,model.target);
        animator.SetIKPosition(AvatarIKGoal.LeftHand,model.target);

        RotateTorso(vel);
    }

    private void RotateTorso(Vector3 velocity)
    {
        // rotate bottom half
        Transform spineTransform = animator.GetBoneTransform(HumanBodyBones.Spine);
        
        // -90 for right, 90 for left
        float angle = Vector3.SignedAngle(velocity, model.view.forward, Vector3.up);
        Debug.Log(angle + " and " + spineTransform.rotation.eulerAngles);
       
        if (angle > 15) yRotation = Mathf.Lerp(180, 90, angle / 60);
        else if (angle < -15) yRotation = Mathf.Lerp(180, 270, Mathf.Abs(angle) / 60);
        else yRotation = Mathf.Lerp(yRotation, 180, Time.fixedDeltaTime * 8);
        
        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(yRotation, 0,0));
    }

    private const float turnSpeed = 25;
    private void RotateTowards(Vector3 t)
    {
        Vector3 targetDir = t - transform.position;

        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        // Move our position a step closer to the target.
        Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
        
        //Vector3 newRot = transform.rotation.eulerAngles;
        Quaternion y = Quaternion.Euler(0,newRot.y,0);

        transform.rotation = y;
    }
    
}
