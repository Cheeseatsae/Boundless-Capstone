using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    protected Animator animator;
    private PlayerModel model;

    public bool IKActive = true;

    private Vector3 projectedDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        model = GetComponentInParent<PlayerModel>();

        model.controller.OnLeftInput += InputLeft;
        model.controller.OnRightInput += InputRight;
        model.controller.OnForwardInput += InputForward;
        model.controller.OnBackwardInput += InputBackward;

        model.controller.OnLeftInputUp += InputLeftUp;
        model.controller.OnRightInputUp += InputRightUp;
        model.controller.OnForwardInputUp += InputForwardUp;
        model.controller.OnBackInputUp += InputBackUp;
    }

    private void OnDestroy()
    {
        model.controller.OnLeftInput -= InputLeft;
        model.controller.OnRightInput -= InputRight;
        model.controller.OnForwardInput -= InputForward;
        model.controller.OnBackwardInput -= InputBackward;

        model.controller.OnLeftInputUp -= InputLeftUp;
        model.controller.OnRightInputUp -= InputRightUp;
        model.controller.OnForwardInputUp -= InputForwardUp;
        model.controller.OnBackInputUp -= InputBackUp;
    }

    void OnAnimatorIK()
    {

        RotateTowards(transform.position + projectedDir);
        
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


        animator.SetBoneLocalRotation(HumanBodyBones.Spine, spineTransform.localRotation);

    }

    private const float turnSpeed = 10;
    
    private void RotateTowards(Vector3 t)
    {
        Vector3 targetDir = t - transform.position;

        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.deltaTime;

        // Move our position a step closer to the target.
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
        
        float counterXRot = -model.view.rotation.eulerAngles.x;
        transform.localRotation = Quaternion.Euler(counterXRot,newRot.y,0);
//        
//        Quaternion rot = Quaternion.Euler(0,newRot.y,0);
//
//        transform.rotation = rot;
    }

    // taking our inputs and getting a projected direction to turn towards
    private void InputLeft(float i)
    {
        if (i > 0) projectedDir -= transform.right;
        
        ClampProjectedDir();
    }

    private void InputRight(float i)
    {
        if (i > 0) projectedDir += transform.right;
        
        ClampProjectedDir();
    }

    private void InputForward(float i)
    {
        if (i > 0) projectedDir += transform.forward;
        
        ClampProjectedDir();
    }

    private void InputBackward(float i)
    {
        if (i > 0) projectedDir -= transform.forward;

        ClampProjectedDir();
    }

    private void InputLeftUp()
    {
        projectedDir += transform.right;
    }
    
    private void InputRightUp()
    {
        projectedDir -= transform.right;
    }
    
    private void InputForwardUp()
    {
        projectedDir -= transform.forward;
    }
    
    private void InputBackUp()
    {
        projectedDir += transform.forward;
    }
    
    private void ClampProjectedDir()
    {
        projectedDir.x = Mathf.Clamp(projectedDir.x, -1,1);
        projectedDir.z = Mathf.Clamp(projectedDir.z, -1,1);
        
        // because we arent using vertical projection
        projectedDir.y = 0;
    }
}
