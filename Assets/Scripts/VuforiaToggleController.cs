using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic; // Needed for Lists

public class VuforiaToggleController : MonoBehaviour
{
    [Header("Vuforia Components")]
    public VuforiaBehaviour vuforiaBehaviour;

    [Header("Cleanup Settings")]
    [Tooltip("Drag all your Image Targets (or the 3D models inside them) here.")]
    public List<GameObject> arObjectsToClean; 

    private bool isVuforiaRunning = true;

    void Start()
    {
        // Automatic setup if you forgot to drag the camera
        if (vuforiaBehaviour == null)
        {
            GameObject arCamera = GameObject.Find("ARCamera");
            if (arCamera != null)
                vuforiaBehaviour = arCamera.GetComponent<VuforiaBehaviour>();
        }

        if (vuforiaBehaviour != null)
            isVuforiaRunning = vuforiaBehaviour.enabled;
    }

    public void TurnCameraOff()
    {
        if (isVuforiaRunning && vuforiaBehaviour != null)
        {
            // 1. HIDE ALL AR OBJECTS FIRST (Clean the screen)
            ToggleARObjects(false);

            // 2. Stop the engine
            vuforiaBehaviour.enabled = false;
            isVuforiaRunning = false;
            
            Debug.Log("Vuforia OFF and Screen Cleaned");
        }
    }

    public void TurnCameraOn()
    {
        if (!isVuforiaRunning && vuforiaBehaviour != null)
        {
            // 1. Start the engine
            vuforiaBehaviour.enabled = true;
            isVuforiaRunning = true;

            // 2. ALLOW OBJECTS TO BE SEEN AGAIN
            // (They will stay hidden until Vuforia tracks the image again)
            ToggleARObjects(true);

            // 3. RESET TRACKING (Fresh start)
            if (vuforiaBehaviour.DevicePoseBehaviour != null)
            {
                vuforiaBehaviour.DevicePoseBehaviour.Reset();
            }

            // 4. Autofocus
            StartCoroutine(TriggerAutofocus());

            Debug.Log("Vuforia ON and Fresh");
        }
    }

    // Helper function to loop through your list
    private void ToggleARObjects(bool state)
    {
        foreach (GameObject obj in arObjectsToClean)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    private IEnumerator TriggerAutofocus()
    {
        yield return new WaitForSeconds(0.5f); 
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
}