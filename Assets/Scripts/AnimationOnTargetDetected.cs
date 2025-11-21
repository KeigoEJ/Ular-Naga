using UnityEngine;
using Vuforia;

public class AnimationOnTargetDetected : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Play";

    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += HandleTargetStatusChanged;
    }

    private void HandleTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
