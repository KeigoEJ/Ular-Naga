using UnityEngine;
using Vuforia;

public class PlayAnimationOnFound : DefaultObserverEventHandler
{
    public Animator animator;
    public string animationTrigger = "Play";

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        animator.SetTrigger(animationTrigger);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        // Optional: stop or reset animation
        // animator.Play("Idle", 0, 0);
    }
}
