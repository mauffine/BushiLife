using UnityEngine;
using System.Collections;
public enum CustomAnimationState
{
    Walking,
    Running,
    Lightattack,
    Heavyattack,
    Block,
    Dodge,
    Idle,
    Jump
};
public class AnimationStateMachine : MonoBehaviour {
    
    [SerializeField]
    Animator animator;

    [SerializeField]
    float lightAttackTime;
    [SerializeField]
    float heavyAttackTime;
    [SerializeField]
    float dodgeTime;

    float animationTimer;

    public CustomAnimationState currentAnimation;
    
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.animationTimer += Time.deltaTime;
        switch (currentAnimation)
        {
            case (CustomAnimationState.Dodge):
                {
                    if (animationTimer > dodgeTime)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            case (CustomAnimationState.Lightattack):
                {
                    if (animationTimer > lightAttackTime)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            case (CustomAnimationState.Heavyattack):
                {
                    if (animationTimer > heavyAttackTime)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            default:
                break;
        }
	}
    public bool SetAnimation(CustomAnimationState _setState)
    {
        switch (_setState)
        {
            case (CustomAnimationState.Walking):
                {
                    if (currentAnimation == CustomAnimationState.Idle)
                    {
                        this.animator.SetBool("Walking", true);
                        this.currentAnimation = CustomAnimationState.Walking;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Running):
                {
                    if (currentAnimation == CustomAnimationState.Walking)
                    {
                        this.animator.SetBool("Running", true);
                        this.currentAnimation = CustomAnimationState.Running;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Idle):
                {
                    if (currentAnimation == CustomAnimationState.Walking || currentAnimation == CustomAnimationState.Running || currentAnimation == CustomAnimationState.Jump)
                    {
                        this.animator.SetBool("Running", false);
                        this.animator.SetBool("Walking", false);
                        this.currentAnimation = CustomAnimationState.Idle;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Dodge):
                {
                    if (currentAnimation != CustomAnimationState.Lightattack &&
                        currentAnimation != CustomAnimationState.Heavyattack && currentAnimation != CustomAnimationState.Dodge)
                    {
                        this.animator.SetTrigger("Dodge");
                        this.currentAnimation = CustomAnimationState.Dodge;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Lightattack):
                {
                    if (currentAnimation != CustomAnimationState.Dodge &&
                        currentAnimation != CustomAnimationState.Heavyattack)
                    {
                        this.animator.SetTrigger("LightAttack");
                        this.currentAnimation = CustomAnimationState.Lightattack;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Heavyattack):
                {
                    if (currentAnimation != CustomAnimationState.Dodge &&
                        currentAnimation != CustomAnimationState.Lightattack)
                    {
                        this.animator.SetTrigger("HeavyAttack");
                        this.currentAnimation = CustomAnimationState.Heavyattack;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Jump):
                {
                    if (currentAnimation != CustomAnimationState.Lightattack &&
                        currentAnimation != CustomAnimationState.Heavyattack && currentAnimation != CustomAnimationState.Dodge)
                    {
                        //TODO: add jump animations
                        this.animator.SetBool("Walking", true);
                        this.currentAnimation = CustomAnimationState.Jump;
                    }
                }
                break;

            default:
                break;

        }
        return false;
    }
}