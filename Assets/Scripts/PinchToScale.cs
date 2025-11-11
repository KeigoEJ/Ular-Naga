using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToScale : MonoBehaviour
{
    [Header("Main Settings")]
    [Tooltip("Is pinching allowed? Set this to true in the Inspector or call SetPinchEnabled(true).")]
    public bool PinchEnabled = false;

    [Tooltip("How fast or 'sensitive' the scaling is.")]
    public float scaleSpeed = 0.05f;

    [Header("Scale Limits")]
    [Tooltip("The smallest the object can get (e.g., 0.1 for 10%).")]
    public float minScale = 0.1f;

    [Tooltip("The biggest the object can get (e.g., 3 for 300%).")]
    public float maxScale = 3.0f;

    // A private variable to store the original starting scale
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        // Store the object's starting scale when the app loads
        originalScale = transform.localScale;
    }

    // Call this from your Vuforia Event Handler
    public void SetPinchEnabled(bool isEnabled)
    {
        PinchEnabled = isEnabled;

        // Optional: Reset scale when tracking is lost
        if (!isEnabled)
        {
            // Uncomment this line if you want the object to reset
            // its size every time you look away
            // transform.localScale = originalScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If pinch isn't enabled OR we don't have two fingers, do nothing.
        if (!PinchEnabled || Input.touchCount != 2)
        {
            return;
        }

        // We have two fingers, let's process them
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position of each touch in the *previous* frame
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude (distance) between the touches in this frame and the last
        float prevTouchDistance = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentTouchDistance = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in distance between this frame and the last
        float distanceDelta = currentTouchDistance - prevTouchDistance;

        // --- Apply the scale ---
        
        // Create a scale amount based on the delta and our speed
        float scaleAmount = distanceDelta * scaleSpeed;

        // Get the current scale and add the new scale amount to all 3 axes
        Vector3 newScale = transform.localScale + new Vector3(scaleAmount, scaleAmount, scaleAmount);

        // --- Clamp the Scale (Very Important!) ---
        // This stops it from getting too big, too small, or inverting
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

        // Apply the final, clamped scale to the object
        transform.localScale = newScale;
    }
}