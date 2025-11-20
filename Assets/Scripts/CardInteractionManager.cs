using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CardInteractionManager : MonoBehaviour
{
    // 1. DEEPEST LEVEL: The Individual Character
    [System.Serializable]
    public class CharacterData
    {
        public string name = "Character Name"; // Just for label
        public Animator animator;
        public float myDanceDuration = 3.0f; // <-- UNIQUE DURATION FOR THIS SPECIFIC CHAR
    }

    // 2. MID LEVEL: The Interaction Set (Cards + Characters)
    [System.Serializable]
    public class InteractionSet
    {
        public string setName = "Card Pair Set";
        
        [Header("The Cards Required")]
        public List<ObserverBehaviour> cardTargets;

        [Header("The Characters in this Set")]
        // We use the new List<CharacterData> instead of just Animators
        public List<CharacterData> characters; 
        
        [HideInInspector] public bool isCurrentlyActive = false;
    }

    [Header("Configuration")]
    [SerializeField] private List<InteractionSet> interactionSets;

    [Header("Animation Triggers")]
    [SerializeField] private string interactionBool = "isInteracting";
    [SerializeField] private string danceTrigger = "Dance";

    private bool isBusyDancing = false;

    void Update()
    {
        if (isBusyDancing) return;

        CheckAllSets();
    }

    void CheckAllSets()
    {
        foreach (var set in interactionSets)
        {
            bool allCardsVisible = true;

            // Check Card Visibility
            foreach (var target in set.cardTargets)
            {
                if (!IsTracked(target))
                {
                    allCardsVisible = false;
                    break;
                }
            }

            // Logic: Switch between Interaction and Idle
            if (allCardsVisible)
            {
                if (!set.isCurrentlyActive)
                {
                    // Loop through the custom CharacterData list
                    foreach (var charData in set.characters)
                    {
                        if (charData.animator != null && charData.animator.isActiveAndEnabled)
                            charData.animator.SetBool(interactionBool, true);
                    }
                    set.isCurrentlyActive = true;
                }
            }
            else
            {
                if (set.isCurrentlyActive)
                {
                    foreach (var charData in set.characters)
                    {
                        if (charData.animator != null && charData.animator.isActiveAndEnabled)
                            charData.animator.SetBool(interactionBool, false);
                    }
                    set.isCurrentlyActive = false;
                }
            }
        }
    }

    public void TriggerAllDance()
    {
        if (!isBusyDancing)
        {
            StartCoroutine(DanceRoutine());
        }
    }

    IEnumerator DanceRoutine()
    {
        isBusyDancing = true;
        float globalLongestWaitTime = 0f;

        // 1. Reset Interaction & Trigger Dance
        foreach (var set in interactionSets)
        {
            // Turn off interaction boolean first
            foreach (var charData in set.characters)
            {
                if (charData.animator != null) 
                    charData.animator.SetBool(interactionBool, false);
            }
            set.isCurrentlyActive = false;

            // Trigger Dance & Calculate Wait Time
            foreach (var charData in set.characters)
            {
                if (charData.animator != null && charData.animator.gameObject.activeInHierarchy)
                {
                    // Trigger the dance
                    charData.animator.SetTrigger(danceTrigger);

                    // COMPARE: Is this character's dance longer than the current record holder?
                    // If yes, we must wait for THIS character to finish.
                    if (charData.myDanceDuration > globalLongestWaitTime)
                    {
                        globalLongestWaitTime = charData.myDanceDuration;
                    }
                }
            }
        }

        // Safety net: if no one is visible, wait 1 sec just in case
        if (globalLongestWaitTime <= 0.1f) globalLongestWaitTime = 1.0f;

        Debug.Log($"Dancing! System locked for {globalLongestWaitTime} seconds (longest clip).");

        // 2. Wait for the LONGEST character to finish
        yield return new WaitForSeconds(globalLongestWaitTime);

        // 3. Unlock
        isBusyDancing = false;
    }

    bool IsTracked(ObserverBehaviour target)
    {
        if (target == null) return false;
        return target.TargetStatus.Status == Status.TRACKED ||
               target.TargetStatus.Status == Status.EXTENDED_TRACKED;
    }
}