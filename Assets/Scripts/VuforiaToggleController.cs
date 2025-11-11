using UnityEngine;
using Vuforia; // Make sure you have this using directive!

public class VuforiaToggleController : MonoBehaviour
{
    [Header("Vuforia Components")]
    // You MUST drag your ARCamera from the scene here
    public VuforiaBehaviour vuforiaBehaviour;

    private bool isVuforiaRunning = true;

    void Start()
    {
        // Check if the user dragged the component in the inspector
        if (vuforiaBehaviour == null)
        {
            Debug.Log("VuforiaBehaviour not set, trying to find ARCamera...");
            // If not, try to find it on the ARCamera
            GameObject arCamera = GameObject.Find("ARCamera"); // Assumes your camera is named "ARCamera"
            if (arCamera != null)
            {
                vuforiaBehaviour = arCamera.GetComponent<VuforiaBehaviour>();
            }
        }

        if (vuforiaBehaviour == null)
        {
            Debug.LogError("FATAL ERROR: VuforiaBehaviour component not found! " +
                           "Please drag your ARCamera object to the 'Vuforia Behaviour' slot in the Inspector.");
        }
        else
        {
            // Assume it's running by default
            isVuforiaRunning = vuforiaBehaviour.enabled;
        }
    }

    /// <summary>
    /// Call this from your "Camera Off" UI Button
    /// </summary>
    public void TurnCameraOff()
    {
        if (isVuforiaRunning && vuforiaBehaviour != null)
        {
            // This is the NEW correct way to stop Vuforia
            vuforiaBehaviour.enabled = false;
            isVuforiaRunning = false;
            Debug.Log("Vuforia Toggled OFF");
        }
    }

    /// <summary>
    /// Call this from your "Camera On" UI Button
    /// </summary>
    public void TurnCameraOn()
    {
        if (!isVuforiaRunning && vuforiaBehaviour != null)
        {
            // This is the NEW correct way to start Vuforia
            vuforiaBehaviour.enabled = true;
            isVuforiaRunning = true;
            Debug.Log("Vuforia Toggled ON");
        }
    }
}