using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerModel model;

    public bool IKActive = true;

    private Vector3 projectedDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        model = GetComponentInParent<PlayerModel>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 vel = model.body.velocity;
        vel.y = 0;
        Gizmos.DrawSphere(transform.position + vel, 1);
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

        // rotate bottom half
        Transform spineTransform = animator.GetBoneTransform(HumanBodyBones.Spine);

        // based on model.view.forward rotate torso towards with a clamp
        
        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(180+45,0,0));

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
