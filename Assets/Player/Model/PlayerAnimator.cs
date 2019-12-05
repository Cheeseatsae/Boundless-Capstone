using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerModel model;

    private Ability1 a1;
    private Ability2 a2;
    private Ability3 a3;
    private Ability4 a4;

    public bool IKActive = true;
    
    private float yRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        model = GetComponentInParent<PlayerModel>();
//        animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(180, 0, 0));

        model.AnimationEventJump += PlayAnimJump;
        model.AnimationEventLand += PlayAnimLand;
//        model.AnimationEventSprint += PlayAnimSprint;
//        model.AnimationEventRun += PlayAnimRun;

//        a1 = model.ability1 as Ability1;
//        a1.AnimationEventAbility1 += PlayAnimAbility1;
//        a2 = model.ability2 as Ability2;
//        a2.AnimationEventAbility2 += PlayAnimAbility2;
        a3 = model.ability3 as Ability3;
        a3.AnimationEventAbility3 += PlayAnimAbility3;
//        a4 = model.ability4 as Ability4;
//        a4.AnimationEventAbility4 += PlayAnimAbility4;
        
    }

    private void OnDestroy()
    {
        model.AnimationEventJump -= PlayAnimJump;
        model.AnimationEventLand -= PlayAnimLand;
//        model.AnimationEventSprint -= PlayAnimSprint;
//        model.AnimationEventRun -= PlayAnimRun;
//        a1.AnimationEventAbility1 -= PlayAnimAbility1;
//        a2.AnimationEventAbility2 -= PlayAnimAbility2;
        a3.AnimationEventAbility3 -= PlayAnimAbility3;
//        a4.AnimationEventAbility4 -= PlayAnimAbility4;
    }

    void OnAnimatorIK()
    {
        Vector3 vel = model.body.velocity;
        vel.y = 0;
        
        if (vel.magnitude > 0.1f) RotateTowards(transform.position + vel);
        else RotateTowards(model.target);
        
        
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

    private void FixedUpdate()
    {
        if (!model.grounded) return;
        
        if (model.body.velocity.magnitude > 0.1f)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Running");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private void RotateTorso(Vector3 velocity)
    {
        // -90 for right, 90 for left
        float angle = Vector3.SignedAngle(velocity, model.view.forward, Vector3.up);
       
        // For some goddam reason the spine transform is not only backwards it is also sideways so
        // x-axis = y-axis and 180 degrees = 0 degrees
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

    private void PlayAnimJump()
    {
        animator.SetTrigger("Jump");
    }

    private bool spawnCompensation = false;
    private void PlayAnimLand()
    {
        if (!spawnCompensation)
        {
            spawnCompensation = true;
            return;
        }

        animator.SetTrigger("Land");
    }

    public void PlayStepSound()
    {
        model.audio.PlaySound(6);
    }
    
//    private void PlayAnimSprint()
//    {
//        animator.SetTrigger("Running");
//    }
//    
//    private void PlayAnimRun()
//    {
//        animator.SetTrigger("Running");
//    }
    
//    private void PlayAnimAbility1()
//    {
//        animTimer = animTimerMark;
//        
//    }
//    
//    private void PlayAnimAbility2()
//    {
//        animTimer = animTimerMark;
//        
//    }
    
    private void PlayAnimAbility3()
    {
        animator.SetTrigger("Jump");
    }
    
//    private void PlayAnimAbility4()
//    {
//        animTimer = animTimerMark;
//        
//    }
}
