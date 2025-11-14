using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZetcilTouchLook : MonoBehaviour
{
    [Header("Touch Settings")]
    public bool TouchLookEnabled;

    [Header("Movement Settings")]
    [SerializeField] private float sensitivity = 0.1f; // sensitivitas gerakan kamera
    [SerializeField] private float smoothness = 0.5f; // kehalusan gerakan kamera
    private Vector2 lastTouchPosition; // posisi touch terakhir
    private Vector2 currentTouchPosition; // posisi touch saat ini
    private Vector2 smoothDeltaPosition; // perpindahan touch yang telah disampirkan

    void Update()
    {
        if (TouchLookEnabled && Input.touchCount > 0) // jika terdapat input touch
        {
            Touch touch = Input.GetTouch(0); // ambil input touch pertama
            if (touch.phase == TouchPhase.Moved) // jika touch bergerak
            {
                currentTouchPosition = touch.position; // simpan posisi touch saat ini
                Vector2 deltaPosition = currentTouchPosition - lastTouchPosition; // hitung perpindahan touch
                smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smoothness); // sampaikan perpindahan touch yang telah disampirkan
                transform.Rotate(-smoothDeltaPosition.y * sensitivity, 0, 0);
                transform.Rotate(0, smoothDeltaPosition.x * sensitivity, 0, Space.World); // putar kamera sesuai perpindahan touch
                lastTouchPosition = currentTouchPosition; // simpan posisi touch terakhir
            }
            else if (touch.phase == TouchPhase.Began) // jika touch dimulai
            {
                lastTouchPosition = touch.position; // simpan posisi touch terakhir
                smoothDeltaPosition = Vector2.zero; // reset perpindahan touch yang telah disampirkan
            }
            else if (touch.phase == TouchPhase.Ended) // jika touch berakhir
            {
                smoothDeltaPosition = Vector2.zero; // reset perpindahan touch yang telah disampirkan
            }
        }
    }
}
