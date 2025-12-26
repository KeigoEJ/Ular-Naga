using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    public Material newMaterial;
    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
    }

    // Call this function from your Image Target's OnTargetFound event
    public void ChangeMyMaterial()
    {
        if (newMaterial != null)
        {
            cubeRenderer.material = newMaterial;
            Debug.Log("Material swapped! Looks sick. 😎");
        }
    }
}