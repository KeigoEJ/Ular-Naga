using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia; // Don't forget this!

public class CardInteractionManager : MonoBehaviour
{
    [Header("Vuforia Targets")]
    public ObserverBehaviour card1Target; // Drag your Image Target 1 here
    public ObserverBehaviour card2Target; // Drag your Image Target 2 here

    [Header("Characters")]
    public Animator char1Animator; // Drag Char 1's Animator here
    public Animator char2Animator; // Drag Char 2's Animator here

    [Header("Animation Settings")]
    public string paramName = "isInteracting"; // The name of your Bool parameter

    private bool isTriggered = false;

    void Update()
    {
        // Check the status of both cards
        bool card1Seen = IsTracked(card1Target);
        bool card2Seen = IsTracked(card2Target);

        // LOGIC: If BOTH are seen, trigger the special animation
        if (card1Seen && card2Seen)
        {
            if (!isTriggered)
            {
                ActivateInteraction(true);
                isTriggered = true;
            }
        }
        // If one or both are lost, go back to default
        else
        {
            if (isTriggered)
            {
                ActivateInteraction(false);
                isTriggered = false;
            }
        }
    }

    // Helper function to check if Vuforia sees the target
    bool IsTracked(ObserverBehaviour target)
    {
        if (target == null) return false;
        return target.TargetStatus.Status == Status.TRACKED || 
               target.TargetStatus.Status == Status.EXTENDED_TRACKED;
    }

    // Helper function to switch animations
    void ActivateInteraction(bool state)
    {
        if (char1Animator != null) char1Animator.SetBool(paramName, state);
        if (char2Animator != null) char2Animator.SetBool(paramName, state);
        
        Debug.Log(state ? "Special Move Activated!" : "Back to Idle");
    }
}