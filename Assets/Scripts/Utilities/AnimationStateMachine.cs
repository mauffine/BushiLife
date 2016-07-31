using UnityEngine;
using System.Collections;
public enum CustomAnimationState
{
    Walking,
    Running,
    LightAttack,
    LightAttack2,
    HeavyAttack,
    Block,
    Dodge,
    Idle,
    Jump,
    Dead
};
public class AnimationStateMachine : MonoBehaviour {
    
    [SerializeField]
    Animator animator;

    public float lightAttackTime;
    public float lightAttackTime2;
    public float heavyAttackTime;
    public float dodgeTime;
    public float blockTime;

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
            case (CustomAnimationState.LightAttack):
                {
                    if (animationTimer > lightAttackTime)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            case (CustomAnimationState.LightAttack2):
                {
                    if (animationTimer > lightAttackTime2)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            case (CustomAnimationState.HeavyAttack):
                {
                    if (animationTimer > heavyAttackTime)
                    {
                        this.currentAnimation = CustomAnimationState.Idle;
                    }
                    break;
                }
            case (CustomAnimationState.Block):
                {
                    if (animationTimer > blockTime)
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
                    if (currentAnimation != CustomAnimationState.LightAttack &&
                        currentAnimation != CustomAnimationState.HeavyAttack && currentAnimation != CustomAnimationState.Dodge
                         && currentAnimation != CustomAnimationState.Block)
                    {
                        this.animator.SetTrigger("Dodge");
                        this.animator.SetBool("Running", false);
                        this.animator.SetBool("Walking", false);
                        this.currentAnimation = CustomAnimationState.Dodge;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.LightAttack):
                {
                    if (currentAnimation != CustomAnimationState.Dodge &&
                        currentAnimation != CustomAnimationState.HeavyAttack &&
                        currentAnimation != CustomAnimationState.LightAttack2
                         && currentAnimation != CustomAnimationState.Block)
                    {
                        if (currentAnimation == CustomAnimationState.Walking || currentAnimation == CustomAnimationState.Jump)
                        {
                            this.animator.SetBool("Walking", false);
                        }
                        this.animator.SetTrigger("LightAttack");
                        if (!(currentAnimation == CustomAnimationState.Running))
                            this.currentAnimation = CustomAnimationState.LightAttack;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.LightAttack2):
                {
                    if (currentAnimation == CustomAnimationState.LightAttack)
                    {
                        this.animator.SetTrigger("LightAttack2");
                        this.currentAnimation = CustomAnimationState.LightAttack2;
                        this.animationTimer = 0;
                        return true;
                    }
                }
                break;
            case (CustomAnimationState.HeavyAttack):
                {
                    if (currentAnimation != CustomAnimationState.Dodge &&
                        currentAnimation != CustomAnimationState.LightAttack
                         && currentAnimation != CustomAnimationState.Block)
                    {
                        this.animator.SetTrigger("HeavyAttack");
                        this.currentAnimation = CustomAnimationState.HeavyAttack;
                        this.animationTimer = 0;
                        return true;
                    }
                    break;
                }
            case (CustomAnimationState.Jump):
                {
                    if (currentAnimation != CustomAnimationState.LightAttack &&
                         currentAnimation != CustomAnimationState.HeavyAttack && currentAnimation != CustomAnimationState.Dodge
                         && currentAnimation != CustomAnimationState.Block)
                    {
                        this.animator.SetBool("Jump", true);
                        this.currentAnimation = CustomAnimationState.Jump;
                        return true;
                    }
                }
                break;
            case (CustomAnimationState.Block):
                {
                    if (currentAnimation != CustomAnimationState.LightAttack &&
                        currentAnimation != CustomAnimationState.HeavyAttack && currentAnimation != CustomAnimationState.Dodge
                        && currentAnimation != CustomAnimationState.Block)
                    {
                        this.animator.SetTrigger("Block");
                        this.currentAnimation = CustomAnimationState.Block;
                        this.animationTimer = 0;
                        return true;
                    }
                }
                break;
            case (CustomAnimationState.Dead):
                {
                    this.currentAnimation = CustomAnimationState.Dead;
                    this.animator.SetTrigger("Dead");
                }
                break;
            default:
                break;

        }
        return false;
    }
}