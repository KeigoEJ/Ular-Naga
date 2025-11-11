using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TargetImage
{
    public string TargetName;  
    public int Weight;          
}

[System.Serializable]
public class TargetEvent
{
    public int RequiredTotalWeight; 
    public UnityEvent OnMatchEvent; 
}

public class ZetcilVuforiaTarget : MonoBehaviour
{
    [Header("Target Images")]
    public TargetImage[] Images; 

    [Header("Target Events")]
    public TargetEvent[] Events; 

    [Header("Monitoring")]
    public int CurrentTotal = 0; 

    private HashSet<string> activeTargets = new HashSet<string>();

    public void OnTargetFound(string targetName)
    {
        if (!activeTargets.Contains(targetName))
        {
            activeTargets.Add(targetName);
            RecalculateTotal();
        }
    }

    public void OnTargetLost(string targetName)
    {
        if (activeTargets.Contains(targetName))
        {
            activeTargets.Remove(targetName);

           RecalculateTotal();
        }
    }

    public void SetTarget(int weight)
    {
        CurrentTotal += weight;
        CheckCombination();
    }

    private void RecalculateTotal()
    {
        CurrentTotal = 0;

        foreach (string t in activeTargets)
        {
            foreach (var img in Images)
            {
                if (img.TargetName == t)
                {
                    CurrentTotal += img.Weight;
                }
            }
        }

        CheckCombination();
    }

    private void CheckCombination()
    {
        foreach (var ev in Events)
        {
            if (CurrentTotal == ev.RequiredTotalWeight)
            {
                ev.OnMatchEvent.Invoke();
            }
        }
    }
}

