using UnityEngine;
using System.Collections;
public enum AnimationState
{
    Walking,
    Running,
    Lightattack,
    Heavyattack,
    Block,
    Dodge,
    Idle
};
public class AnimationStateMachine : MonoBehaviour {
    [SerializeField]
    Animator animator;

    AnimationState currentAnimation;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void SetAnimation(AnimationState _setState)
    {
        switch (_setState)
        {
            case (AnimationState.Walking):
                {
                    if (currentAnimation == AnimationState.Idle)
                    {
                        animator.SetBool("Walking", true);
                        this.currentAnimation = AnimationState.Walking;
                    }
                    break;
                }
            case (AnimationState.Running):
                {
                    if (currentAnimation == AnimationState.Walking)
                    {
                        this.animator.SetBool("Running", true);
                        this.currentAnimation = AnimationState.Running;
                    }
                    break;
                }
            case (AnimationState.Idle):
                {
                    if (currentAnimation == AnimationState.Walking || currentAnimation == AnimationState.Running)
                    {
                        this.animator.SetBool("Running", false);
                        this.animator.SetBool("Walking", false);
                        this.currentAnimation = AnimationState.Idle;
                    }
                    break;
                }
            case (AnimationState.Dodge):
                {
                    if (currentAnimation != AnimationState.Lightattack &&
                        currentAnimation != AnimationState.Heavyattack && currentAnimation != AnimationState.Dodge)
                    {
                        this.animator.SetTrigger("Dodge");
                        this.currentAnimation = AnimationState.Dodge;
                    }
                    break;
                }
            case (AnimationState.Lightattack):
                {
                    if (currentAnimation != AnimationState.Dodge &&
                        currentAnimation != AnimationState.Heavyattack)
                    {
                        this.animator.SetTrigger("LightAttack");
                        this.currentAnimation = AnimationState.Dodge;
                    }
                    break;
                }
            case (AnimationState.Heavyattack):
                {
                    if (currentAnimation != AnimationState.Dodge &&
                        currentAnimation != AnimationState.Lightattack)
                    {
                        this.animator.SetTrigger("HeavyAttack");
                        this.currentAnimation = AnimationState.Dodge;
                    }
                    break;
                }

        }
    }
}
