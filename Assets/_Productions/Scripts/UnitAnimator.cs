using Elendow.SpritedowAnimator;
using Sirenix.OdinInspector;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationState
{
    Idle, Hit, Block, Dodge, Clash
}

public class UnitAnimator : MonoBehaviour
{
    public SpriteAnimator Animator => spriteAnimator;

    [Title("ANIMATOR")]
    [SerializeField]
    private SpriteAnimator spriteAnimator;

    [Title("ANIMATIONS")]
    public SpriteAnimation idleAnimation;
    public SpriteAnimation dodgeAnimation;
    public SpriteAnimation blockAnimation;
    public SpriteAnimation hitAnimation;
    public SpriteAnimation clashAnimation;

    public UnityEvent onAttackFinished = new();

    private const string IDLE = "Idle";
    private const string DODGE = "Dodge";
    private const string BLOCK = "Block";
    private const string HIT = "Hit";
    private const string CLASH = "Anticipation";

    private void Start()
    {
        PlayIdle();
    }

    public void ChangeAnimation(AnimationState newState)
    {
        SpriteAnimation spriteAnimation = clashAnimation;
        switch (newState)
        {
            case AnimationState.Hit:
                spriteAnimation = hitAnimation;
                break;

            case AnimationState.Block:
                spriteAnimation = blockAnimation;
                break;

            case AnimationState.Dodge:
                spriteAnimation = dodgeAnimation;
                break;

            case AnimationState.Clash:
                spriteAnimation = clashAnimation;
                break;
        }

        spriteAnimator.Play(spriteAnimation, true, false);
    }

    public void PlayIdle()
    {
        spriteAnimator.Play(idleAnimation);
    }

    public void PlayHitAnimation(HitData hit)
    {
        if(hit.IsInvulnerable == false)
        {
            ChangeAnimation(AnimationState.Hit);
            return;
        }

        switch (hit.InvulnerableType)
        {
            case InvulnerableType.Parry:
            case InvulnerableType.Block:
            case InvulnerableType.Immortal:
            case InvulnerableType.Immune:
                ChangeAnimation(AnimationState.Block);
                break;
            case InvulnerableType.Dodge:
                ChangeAnimation(AnimationState.Dodge);
                break;
        }
    }
}
